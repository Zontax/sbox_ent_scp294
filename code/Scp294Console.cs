using Sandbox;
using System.Linq;

namespace Bimbasic;

public partial class Scp294Console : Entity
{
    [ConCmd.Server("set_drink")]
    public static void SetDrinkName(string scpName, string drinkName)
    {
        var scp = Entity.All.OfType<Scp294>().Where(scp => scp.Name == scpName).ToList().FirstOrDefault();
        if (scp != null) scp.FindingName = drinkName;
        scp?.UseLogic();
        scp?.DeletePanel();
        scp?.SpawnPanel(scp.FindingName);
    }

    [ClientRpc]
    public static void SayChat(string name = "", string message = "", long playerId = 0, bool isInfo = true)
    {
        if (message != "") Chat.AddChatEntry(name, message, playerId, isInfo); // 76561197960279927
    }
}