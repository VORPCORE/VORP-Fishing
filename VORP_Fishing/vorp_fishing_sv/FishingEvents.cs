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
            TriggerEvent("vorpCore:registerUsableItem", "fishbait", new Action<dynamic>((data) =>
            {
                PlayerList pl = new PlayerList();
                Player p = pl[data.source];
                p.TriggerEvent("vorp_fishing:UseBait");
            }));
        }

        public void FishToInventory([FromSource]Player source, string modelName)
        {
            Debug.WriteLine("Model Name:" + modelName);
            int _source = int.Parse(source.Handle);


            TriggerEvent("vorpCore:addItem", _source, modelName, 1);

            //TriggerEvent("vorpCore:canCarryItem", _source, modelName, 1, new Action<bool>((can) =>
            //{
            //    if (!can)
            //    {
            //        source.TriggerEvent("vorp:TipRight", LoadConfig.Langs["CantCarryMore"], 4000);
            //    }
            //    else
            //    {
            //        TriggerEvent("vorpCore:addItem", _source, modelName, 1);
            //        source.TriggerEvent("vorp:TipRight", LoadConfig.Langs["CantCarryMore"], 4000);
            //    }

            //}));

        }
    }
}
