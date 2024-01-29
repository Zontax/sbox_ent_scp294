using System.Buffers;
using Sandbox;
using System;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;

namespace Bimbasic;

[Library
("ent_scp_294", Title = "SCP-294", Description =
"Item SCP-294 appears to be a standard coffee vending machine, the only noticeable difference being an entry touchpad with buttons corresponding to an English QWERTY keyboard. Upon depositing fifty cents US currency into the coin slot, the user is prompted to enter the name of any liquid using the touchpad. Upon doing so, a standard 12-ounce paper drinking cup is placed and the liquid indicated is poured.")]
public partial class Scp294 : ModelEntity, IUse
{
    Keyboard clientKeyboard;
    DrinkInfoPanel panel;
    Particles particle;
    [Net, Change] public string FindingName { get; set; }
    [Net, Predicted] int drinkIndex { get; set; } = 0;
    [Net, Predicted] TimeSince useReloadMillis { get; set; }
    [Net, Predicted] Sound sound_use { get; set; }
    [Net, Predicted] bool isTouched { get; set; }
    [Net, Predicted] bool isUse { get; set; }
    [Net, Predicted] bool isOutOfRange { get; set; }
    Vector3 offset = new(-17.05f, 11.3f, 32.5f);
    Vector3 offsetDrinkPanel = new(-20.485f, -11.7f, 62f);
    float useReload = 0.5f;
    float spawnTime;

    string[] drinkNames = { "Drink" };
    string[] drinkNamesRu = { "Drink" };
    string[] drinkNamesUk = { "Drink" };
    int r, g, b = 255; float alpha = 1;
    bool glow = false;
    string message = "Tasty drink.";
    string messageRu = "Вкусный напиток.";
    string messageUk = "Смачний напій.";
    bool lethal = false;
    float deathTime = 0;
    string deathMessage = "";
    string deathMessageRu = "";
    string deathMessageUk = "";
    bool explosion = false;
    float explosionTime = 0;
    bool stomachAche = false;
    int blur = 0;
    bool refuse = false;
    string refuseMessage = "I don't wont it drink";
    string refuseMessageRu = "Я не хочу это пить";
    string refuseMessageUk = "Я не хочу це пити";
    bool heal = false;
    int damage = 0;
    bool godmode = false;
    bool vomit = false;
    string sound = "slurp";
    string dispenseSound = "dispense1";

    void OnFindingNameChanged(string oldValue, string newValue)
    {
        // Log.Info($"findingName changed [{oldValue}], now it is [{newValue}]");
        DeletePanel();
        SpawnPanel("Dispensing...");
    }

    public override void Spawn()
    {
        base.Spawn();
        PhysicsEnabled = true;
        UsePhysicsCollision = true;
        EnableTouchPersists = true;
        SetModel("models/scp_294/scp_294_rotated.vmdl");
        SetupPhysicsFromModel(PhysicsMotionType.Dynamic);
        // this.Rotation = this.Rotation * Rotation.From(0, 180f, 0);
    }

    public override void ClientSpawn()
    {
        base.ClientSpawn();
        SpawnPanel();
    }

    [ClientRpc]
    public void SpawnPanel(string name = "")
    {
        panel = new DrinkInfoPanel();
        panel.scp294 = this;
        panel.Position = this.Position + (offsetDrinkPanel * this.Rotation);
        panel.Rotation = this.Rotation * Rotation.From(0f, 180f, 0f);
        panel.SetLabel(name);
    }

    [ClientRpc]
    public void DeletePanel()
    {
        panel.Delete(true);
    }

    [ClientRpc]
    protected void AddKeyboard()
    {
        Keyboard[] keys = Entity.All.OfType<SandboxHud>().FirstOrDefault().RootPanel.ChildrenOfType<Keyboard>().ToArray();

        if (keys.Count() == 0)
        {
            clientKeyboard = Entity.All.OfType<SandboxHud>().FirstOrDefault().RootPanel.AddChild<Keyboard>();
            clientKeyboard.scp = this;
        }
    }

    [ClientRpc]
    public void DeleteKeyboard()
    {
        clientKeyboard.Delete();
    }

    public override void StartTouch(Entity other)
    {
        if (other is Cup cup)
        {
            isTouched = true;
        }
    }

    public override void EndTouch(Entity other)
    {
        if (other is Cup cup)
        {
            isTouched = false;
        }
    }

    int FindDrinkInList(string name)
    {
        if (name == "random" || name == "random drink")
        {
            int randIndex = Game.Random.Next(0, DrinkDataList.drinks.Count());
            return randIndex;
        }

        if (name == "repeat" || name == "late returns" || name == "last choice" || name == "last" || name == "повтор")
        {
            return FindDrinkInList(drinkNames[0]);
        }

        foreach (string drink in DrinkDataList.drinks)
        {
            string[] lines = drink.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            foreach (string line in lines)
            {
                string pattern = @"\[([^\]]+)\]";
                MatchCollection matches = Regex.Matches(line, pattern);

                foreach (Match match in matches)
                {
                    string fullName = match.Groups[1].Value;
                    string[] nameParts = fullName.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string part in nameParts)
                    {
                        if (part.Trim().Equals(name, StringComparison.OrdinalIgnoreCase))
                        {
                            int index = DrinkDataList.drinks.IndexOf(drink);
                            return index;
                        }
                    }
                }
            }
        }
        return -1;
    }

    void ParseDrinkProperties(int index)
    {
        if (index < 0 || index >= DrinkDataList.drinks.Count) return;

        T GetValueOrDefault<T>(Group group, T defaultValue = default)
        {
            if (group.Success)
            {
                try
                {
                    return (T)Convert.ChangeType(group.Value, typeof(T));
                }
                catch (Exception)
                {
                    return defaultValue;
                }
            }
            return defaultValue;
        }

        bool TryParseBool(Group group, bool defaultValue)
        {
            return group.Success && bool.TryParse(group.Value, out bool result) ? result : defaultValue;
        }

        string pattern = @"\[([^\]]+)\]\s*ru(\[[^\]]+\])?\s*uk(\[[^\]]+\])?(\s*color=(\d+),(\d+),(\d+))?\s*(alpha=([\d.]+))?\s*(glow=(true|false))?\s*(message=\{(.*?)\})?\s*(messageRu=\{(.*?)\})?\s*(messageUk=\{(.*?)\})?\s*(lethal=(true|false))?\s*(deathTimer=([\d.]+))?\s*(deathMessage=\{(.*?)\})?\s*(deathMessageRu=\{(.*?)\})?\s*(deathMessageUk=\{(.*?)\})?\s*(explosion=(true|false))?\s*(explosionTimer=([\d.]+))?\s*(stomachAche=(true|false))?\s*(blur=([\d.]+))?\s*(refuse=(true|false))?\s*(refuseMessage=\{(.*?)\})?\s*(refuseMessageRu=\{(.*?)\})?\s*(refuseMessageUk=\{(.*?)\})?\s*(heal=(true|false))?\s*(damage=([\d.]+))?\s*(godmode=(true|false))?\s*(vomit=(true|false))?\s*(sound=\{([^\}]*)\}\s*)?\s*(dispenseSound=\{([^\}]*)\}\s*)?";
        Match match = Regex.Match(DrinkDataList.drinks[index], pattern, RegexOptions.Singleline);

        if (match.Success)
        {
            drinkNames = match.Groups[1].Value.Replace("]", "").Replace("[", "").Split('|');
            drinkNamesRu = match.Groups[2].Value.Replace("]", "").Replace("[", "").Split('|');
            drinkNamesUk = match.Groups[3].Value.Replace("]", "").Replace("[", "").Split('|');
            r = GetValueOrDefault(match.Groups[5], 255);
            g = GetValueOrDefault(match.Groups[6], 255);
            b = GetValueOrDefault(match.Groups[7], 255);
            alpha = GetValueOrDefault(match.Groups[9], 1);
            glow = TryParseBool(match.Groups[11], false);
            message = GetValueOrDefault(match.Groups[13], "");
            messageRu = GetValueOrDefault(match.Groups[15], message);
            messageUk = GetValueOrDefault(match.Groups[17], message);
            lethal = TryParseBool(match.Groups[19], false);
            deathTime = GetValueOrDefault(match.Groups[21], 0);
            deathMessage = GetValueOrDefault(match.Groups[23], "");
            deathMessageRu = GetValueOrDefault(match.Groups[25], deathMessage);
            deathMessageUk = GetValueOrDefault(match.Groups[27], deathMessage);
            explosion = TryParseBool(match.Groups[29], false);
            explosionTime = GetValueOrDefault(match.Groups[31], 0);
            stomachAche = TryParseBool(match.Groups[33], false);
            blur = GetValueOrDefault(match.Groups[35], 0);
            refuse = TryParseBool(match.Groups[37], false);
            refuseMessage = GetValueOrDefault(match.Groups[39], "");
            refuseMessageRu = GetValueOrDefault(match.Groups[41], "");
            refuseMessageUk = GetValueOrDefault(match.Groups[43], "");
            heal = TryParseBool(match.Groups[45], false);
            damage = GetValueOrDefault(match.Groups[47], 0);
            godmode = TryParseBool(match.Groups[49], false);
            vomit = TryParseBool(match.Groups[51], false);
            sound = GetValueOrDefault(match.Groups[53], "slurp");
            dispenseSound = GetValueOrDefault(match.Groups[55], "dispense1");
        }
    }

    void SpawnDrink()
    {
        Drink drink = new()
        {
            Position = this.Position + (offset * this.Rotation),
            Rotation = this.Rotation,
            RenderColor = Color.FromBytes(r, g, b, Map((int)(alpha * 1000), 0, 1000, 0, 255)),

            name = this.drinkNames[0],
            nameRu = this.drinkNamesRu[0],
            nameUk = this.drinkNamesUk[0],
            r = this.r,
            g = this.g,
            b = this.b,
            alpha = this.alpha,
            glow = this.glow,
            message = this.message,
            messageRu = this.messageRu,
            messageUk = this.messageUk,
            lethal = this.lethal,
            deathTime = this.deathTime,
            deathMessage = this.deathMessage,
            deathMessageRu = this.deathMessageRu,
            deathMessageUk = this.deathMessageUk,
            explosion = this.explosion,
            explosionTime = this.explosionTime,
            explosionDamage = this.damage,
            stomachAche = this.stomachAche,
            blur = this.blur,
            refuse = this.refuse,
            refuseMessage = this.refuseMessage,
            refuseMessageRu = this.refuseMessage,
            refuseMessageUk = this.refuseMessage,
            heal = this.heal,
            damage = this.damage,
            godmode = this.godmode,
            vomit = this.vomit,
            sound = this.sound,
        };

        Cup cup = new()
        {
            Position = this.Position + (offset * this.Rotation),
            Rotation = this.Rotation,
            drink = drink,
        };

        if (drinkNames[0] == "Air")
        {
            drink.Delete();
            drink = null;
        }
        else
        {
            drink.cup = cup;
            drink.Weld(cup);
        }
    }

    public bool OnUse(Entity user)
    {
        if (useReloadMillis > useReload)
        {
            useReloadMillis = 0;

            if (clientKeyboard == null && !sound_use.IsPlaying) AddKeyboard(To.Single(user));
        }
        return false;
    }

    public bool IsUsable(Entity user)
    {
        return true;
    }

    public void UseLogic()
    {
        if (isTouched)
        {
            switch (Language.SelectedCode)
            {
                case "en":
                    Scp294Console.SayChat("SCP-294", "Remove the cup from the coffee machine!");
                    break;
                case "ru":
                    Scp294Console.SayChat("SCP-294", "Заберите стакан из кофемашины!");
                    break;
                case "uk":
                    Scp294Console.SayChat("SCP-294", "Заберіть стакан з кофемашини!");
                    break;
                default:
                    Scp294Console.SayChat("SCP-294", "Remove the cup from the coffee machine!");
                    break;
            }
        }
        else if (!sound_use.IsPlaying)
        {
            drinkIndex = FindDrinkInList(StripDrinkName(FindingName));

            if (drinkIndex != -1)
            {
                ParseDrinkProperties(drinkIndex);
                sound_use = Sound.FromEntity(dispenseSound, this);
                isUse = true;
                // SpawnParticle();
                DeletePanel();
                SpawnPanel("Dispensing...");
            }
            else
            {
                sound_use = Sound.FromEntity("outofrange", this);
                isOutOfRange = true;
                DeletePanel();
                SpawnPanel("Dispensing...");
            }
        }
    }

    public void DelaySpawn()
    {
        SpawnDrink();
    }

    void SpawnParticle()
    {
        particle = Particles.Create(Cloud.ParticleSystem("penguins/water").ResourcePath);
        particle.SetPosition(0, this.Position + (offset * this.Rotation));
        // var particle = Particles.Create("particles/dispence.vpcf", this, "effect", true);
        // particle.SetPosition(0, new Vector3(100, 200, 100));
    }

    protected override void OnDestroy()
    {
        sound_use.Stop();
        panel?.Delete(true);
        DeleteKeyboard();
        base.OnDestroy();
    }

    [GameEvent.Tick.Server]
    public void Tick()
    {
        if (dispenseSound == "dispense0") spawnTime = 1.5f; // TODO: Костиль бля (fucker sound name)
        if (dispenseSound == "dispense1") spawnTime = 2.7f;
        if (dispenseSound == "dispense2") spawnTime = 6.3f;
        if (dispenseSound == "dispense3") spawnTime = 6.6f;
        if (dispenseSound == "outofrange") spawnTime = 3.5f;

        if (sound_use.ElapsedTime > spawnTime && isUse)
        {
            DelaySpawn();
            DeletePanel();
            SpawnPanel(FindingName);
            isUse = false;
        }
        else if (sound_use.ElapsedTime > spawnTime && isOutOfRange)
        {
            DeletePanel();
            SpawnPanel("OUT OF RANGE");
            Scp294Console.SayChat("SCP-294", "OUT OF RANGE");
            isOutOfRange = false;
        }
    }

    string StripDrinkName(string name)
    {
        name = name.ToLower()
                    .Trim('.')
                    .Trim(',')
                    .Trim('\'')
                    .Replace("fluid ", "")
                    .Replace("liquid ", "")
                    .Replace("жидкий ", "")
                    .Replace("жидкая ", "")
                    .Replace("жидкое ", "")
                    .Replace("рідкий ", "")
                    .Replace("рідка ", "")
                    .Replace("рідке ", "")
                    .Replace("the", "")
                    .Replace("a cup of ", "")
                    .Replace("a cup ", "")
                    .Replace("cup of ", "")
                    .Replace("cup ", "")
                    .Replace("чашка ", "")
                    .Replace("чашечка ", "")
                    .Replace("стакан ", "")
                    .Replace("стаканчик ", "")
                    .Replace("склянка ", "")
                    .Replace("drink ", "")
                    .ToLower();
        return name;
    }

    int Map(int value, int fromMin, int fromMax, int toMin, int toMax)
    {
        return (value - fromMin) * (toMax - toMin) / (fromMax - fromMin) + toMin;
    }

    void LogAll()
    {
        Log.Info($" ");
        Log.Info($" ");
        Log.Info($"FIND: [{FindingName.ToUpper().Trim('.').Trim(',').Trim()}]");
        Log.Info($"EN {drinkNames[0]}");
        //Log.Info($"RU {drinkNamesRu}");
        Log.Info($"UK {drinkNamesUk[0]}");
        //Log.Info($"Color {r}, {g}, {b}, {alpha}");
        Log.Info($"message: {message}");
        Log.Info($"messageRu: {messageRu}");
        Log.Info($"messageUk: {messageUk}");
        Log.Info($"lethal {lethal}");
        Log.Info($"deathTimer {deathTime}");
        Log.Info($"deathMessage: {deathMessage}");
        //Log.Info($"deathMessageRu: {deathMessageRu}");
        Log.Info($"deathMessageUk: {deathMessageUk}");
        Log.Info($"explosion {explosion}");
        Log.Info($"explosionTimer {explosionTime}");
        Log.Info($"stomachAche {stomachAche}");
        Log.Info($"blur {blur}");
        Log.Info($"glow {glow}");
        Log.Info($"refuse {refuse}");
        Log.Info($"refuseMessage: {refuseMessage}");
        //Log.Info($"refuseMessageRu: {refuseMessageRu}");
        //Log.Info($"refuseMessageUk: {refuseMessageUk}");
        Log.Info($"heal {heal}");
        Log.Info($"damage {damage}");
        //Log.Info($"godmode {godmode}");
        Log.Info($"vomit {vomit}");
        Log.Info($"{sound}");
        Log.Info($"{dispenseSound}");
    }
}