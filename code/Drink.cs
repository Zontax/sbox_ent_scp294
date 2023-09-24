using Sandbox;

namespace Bimbasic;

public partial class Drink : Prop, IUse
{
    BlurEffect clientBlur;
    [Net, Predicted] public Cup cup { get; set; }
    [Net, Predicted] public TimeSince explosionMillis { get; set; }
    [Net, Predicted] Sound soundDrink { get; set; }
    [Net, Predicted] Sound soundStomach { get; set; }
    public string name = "Drink";
    public string nameRu = "Drink";
    public string nameUk = "Drink";
    public int r = 0;
    public int g = 0;
    public int b = 0;
    public float alpha = 1;
    public bool glow;
    public string message = "";
    public string messageRu = "";
    public string messageUk = "";
    public bool refuse;
    public string refuseMessage = "";
    public string refuseMessageRu = "";
    public string refuseMessageUk = "";
    public bool lethal;
    public float deathTime = 0;
    public string deathMessage = "";
    public string deathMessageRu = "";
    public string deathMessageUk = "";
    public bool explosion;
    public bool explosionNow;
    public bool stomachAche;
    public int blur = 0;
    public bool heal;
    public float damage = 0;
    public bool godmode;
    public bool vomit;
    public string sound = "";

    public float explosionTime = 0;
    public int explosionDamage { get; set; } = 150;
    public int explosionRadius { get; set; } = 120;
    public float explosionForce { get; set; } = 7f;
    public string explosionSound { get; set; } = "explosion";

    public override void Spawn()
    {
        if (Game.IsServer)
        {
            base.Spawn();
            Predictable = true;
            PhysicsEnabled = true;
            UsePhysicsCollision = true;
            ClearMaterialOverride();
            SetModel("models/cup_and_drink/drink.vmdl");
            SetupPhysicsFromModel(PhysicsMotionType.Dynamic);
            explosionMillis = 0;
        }
    }

    public bool OnUse(Entity user)
    {
        if (Game.IsServer)
        {
            UseDrink(user);
        }
        return false;
    }

    public bool IsUsable(Entity user)
    {
        return true;
    }

    public void UseDrink(Entity user)
    {
        //if (blur > 0) AddBlur(To.Single(user));

        if (sound == "cup_of_music")
        {
            Sound.FromEntity("slurp", user);
            soundDrink = Sound.FromEntity(sound, user);
            soundDrink.SetVolume(0.1f);
        }
        else
        {
            soundDrink = Sound.FromEntity(sound, user);
        }

        if (stomachAche) soundStomach = Sound.FromEntity("stomach", user);
        if (explosion) Explode();

        if (damage > 0)
        {
            if ((user.Health - damage) <= 0) Kill(user);
            else user.Health -= damage;
        }

        if (refuse)
        {
            switch (Language.SelectedCode)
            {
                case "en":
                    Scp294Console.SayChat("You", refuseMessage);
                    break;
                case "ru":
                    Scp294Console.SayChat("Вы", refuseMessageRu);
                    break;
                case "uk":
                    Scp294Console.SayChat("Ви", refuseMessageUk);
                    break;
                default:
                    Scp294Console.SayChat("You", refuseMessage);
                    break;
            }
            return;
        }
        else if (lethal)
        {
            switch (Language.SelectedCode)
            {
                case "en":
                    Scp294Console.SayChat("You", message);
                    break;
                case "ru":
                    Scp294Console.SayChat("Вы", messageRu);
                    break;
                case "uk":
                    Scp294Console.SayChat("Ви", messageUk);
                    break;
                default:
                    Scp294Console.SayChat("You", message);
                    break;
            }

            DrinkDelay killDelay = new()
            {
                player = user,
                deathMillis = 0,
                deathTime = deathTime,
                deathMessage = deathMessage,
                deathMessageRu = deathMessageRu,
                deathMessageUk = deathMessageUk,
                onDeath = true,
            };

            cup.drink = null;
            Delete();
        }
        else
        {
            switch (Language.SelectedCode)
            {
                case "en":
                    Scp294Console.SayChat("You", message);
                    break;
                case "ru":
                    Scp294Console.SayChat("Вы", messageRu);
                    break;
                case "uk":
                    Scp294Console.SayChat("Ви", messageUk);
                    break;
                default:
                    Scp294Console.SayChat("You", message);
                    break;
            }
            cup.drink = null;
            Delete();
        }
    }

    void Kill(Entity user)
    {
        user.Health = 0;
        user.OnKilled();
        Scp294Console.SayChat("", deathMessage);
        Scp294Console.SayChat("", deathMessageRu);
        Scp294Console.SayChat("", deathMessageUk);
    }

    void ExplodeDelay()
    {
        if (explosionMillis > explosionTime)
        {
            explosionTime = 0;
            Explode();
        }
    }

    void Explode()
    {
        ExplosionEntity boom = new()
        {
            Transform = this.Transform,
            Damage = explosionDamage,
            Radius = explosionRadius,
            ForceScale = explosionForce,
            RemoveOnExplode = true,
            SoundOverride = explosionSound,
        };
        boom.Explode(this);
        cup.drink = null;
        Delete();
    }

    [ClientRpc]
    public void AddBlur() // TODO: Add effects system
    {
        //Log.Error($"blur={blur}");
        // BlurEffect[] blurs = Entity.All.OfType<SandboxHud>().FirstOrDefault().RootPanel.ChildrenOfType<BlurEffect>().ToArray();

        // if (blurs.Count() == 0)
        // {
        //     clientBlur = Entity.All.OfType<SandboxHud>().FirstOrDefault().RootPanel.AddChild<BlurEffect>();
        //     clientBlur.blurTime = blur;
        // }
    }

    [GameEvent.Tick.Server]
    public void Tick()
    {
        if (explosionTime > 0) ExplodeDelay();
    }
}