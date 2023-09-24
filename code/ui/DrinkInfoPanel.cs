using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using Bimbasic;

class DrinkInfoPanel : WorldPanel
{   
    public Scp294 scp294;
    Vector3 offsetDrinkPanel = new(-20.485f, -11.8f, 62f);
    Label drink;

    public DrinkInfoPanel()
    {
        var bounds = new Rect() { Width = 140f, Height = 29.5f };
        PanelBounds = bounds;
        StyleSheet.Load("/code/ui/DrinkInfoPanel.scss");
    }
    
    public void SetLabel(string name)
    {
        drink = Add.Label(name.ToUpper(), "drink");
    }

    public override void Tick()
	{
        Scale = scp294.Scale;
        Position = scp294.Position + (offsetDrinkPanel * scp294.Rotation);
        Rotation = scp294.Rotation * Rotation.From(0, 180, 0);
    }
}