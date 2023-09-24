using Sandbox;

namespace Bimbasic;

public partial class DrinkDelay : Entity
{
    [Net, Predicted] public TimeSince deathMillis { get; set; }
    [Net, Predicted] public Entity player { get; set; }
    public string deathMessage;
    public string deathMessageRu;
    public string deathMessageUk;
    public bool lethal;
    public float deathTime;
    public bool onDeath;

    public override void Spawn()
    {
        deathMillis = 0;
    }

    void Kill(Entity user)
    {
        user.Health = 0;
        user.OnKilled();

        switch (Language.SelectedCode)
        {
            case "en":
                Scp294Console.SayChat(message: deathMessage);
                break;
            case "ru":
                Scp294Console.SayChat(message: deathMessageRu);
                break;
            case "uk":
                Scp294Console.SayChat(message: deathMessageUk);
                break;
            default:
                Scp294Console.SayChat(message: deathMessage);
                break;
        }
    }

    void DelayKill(Entity user)
    {
        if (deathMillis >= deathTime)
        {
            deathMillis = 0;
            Kill(player);
            this.Delete();
        }
    }

    [GameEvent.Tick.Server]
    public void Tick()
    {
        if (onDeath) DelayKill(player);
    }
}