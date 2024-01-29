namespace Bimbasic.Models;

public class DrinkData
{
    public int Id { get; set; }
    public string DrinkName { get; set; } = string.Empty;
    public string DrinkNameRu { get; set; } = string.Empty;
    public string DrinkNameUk { get; set; } = string.Empty;
    public int R { get; set; }
    public int G { get; set; }
    public int B { get; set; }
    public float Alpha { get; set; }
    public bool Glow { get; set; }
    public string Message { get; set; } = string.Empty;
    public string MessageRu { get; set; } = string.Empty;
    public string MessageUk { get; set; } = string.Empty;
    public bool Lethal { get; set; }
    public float DeathTime { get; set; }
    public string DeathMessage { get; set; } = string.Empty;
    public string DeathMessageRu { get; set; } = string.Empty;
    public string DeathMessageUk { get; set; } = string.Empty;
    public bool Explosion { get; set; }
    public float ExplosionTime { get; set; }
    public bool StomachAche { get; set; }
    public int Blur { get; set; }
    public bool Refuse { get; set; }
    public string RefuseMessage { get; set; } = string.Empty;
    public string RefuseMessageRu { get; set; } = string.Empty;
    public string RefuseMessageUk { get; set; } = string.Empty;
    public bool Heal { get; set; }
    public int Damage { get; set; }
    public bool GodMode { get; set; }
    public bool Vomit { get; set; }
    public string Sound { get; set; } = string.Empty;
    public string DispenseSound { get; set; } = string.Empty;
}