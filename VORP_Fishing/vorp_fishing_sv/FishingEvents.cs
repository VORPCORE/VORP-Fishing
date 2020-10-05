using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;

namespace vorp_fishing_sv
{
    public class FishingEvents : BaseScript
    {
        public static dynamic VorpCore;

        public FishingEvents()
        {
            TriggerEvent("getCore", new Action<dynamic>((core) =>
            {
                VorpCore = core;
            }));

            EventHandlers["vorp_fishing:FishToInventory"] += new Action<Player, string>(FishToInventory);
            EventHandlers["vorp_fishing:baitUsed"] += new Action<Player>(BaitUsed);
            TriggerEvent("vorpCore:registerUsableItem", "fishbait", new Action<dynamic>((data) =>
            {
                PlayerList pl = new PlayerList();
                Player p = pl[data.source];
                p.TriggerEvent("vorp_fishing:UseBait");
            }));



        }

        private void BaitUsed(Player player)
        {
            int _source = int.Parse(player.Handle);
            TriggerEvent("vorpCore:subItem", _source, "fishbait", 1);
        }

        public void FishToInventory([FromSource]Player source, string modelName)
        {
            Debug.WriteLine("Model Name:" + modelName);
            int _source = int.Parse(source.Handle);

            source.TriggerEvent("vorp:TipRight", LoadConfig.Langs["CaughtFish"], 2000);
            TriggerEvent("vorpCore:addItem", _source, modelName, 1);
        }
    }
}
