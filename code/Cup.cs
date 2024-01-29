using Sandbox;

namespace Bimbasic;

public partial class Cup : Prop, IUse
{
    public Drink? drink;

    public override void Spawn()
    {
        if (Game.IsServer)
        {
            base.Spawn();
            Predictable = true;
            PhysicsEnabled = true;
            UsePhysicsCollision = true;
            ClearMaterialOverride();
            SetModel("models/cup_and_drink/cup.vmdl");
            SetupPhysicsFromModel(PhysicsMotionType.Dynamic);
        }
    }

    protected override void OnDestroy()
    {
        if (Game.IsServer)
        {
            drink?.Delete();
            this?.Delete();
        }
    }

    public bool OnUse(Entity user)
    {
        if (Game.IsServer)
        {
            UseCup(user);
        }
        return false;
    }

    public bool IsUsable(Entity user)
    {
        return true;
    }

    void UseCup(Entity user)
    {
        drink?.UseDrink(user);
    }

    [GameEvent.Tick.Server]
    public void Tick()
    {
    }
}