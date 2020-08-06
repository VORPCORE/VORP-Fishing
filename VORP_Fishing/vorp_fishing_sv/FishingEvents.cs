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
        public FishingEvents()
        {
            EventHandlers["vorp_fishing:FishToInventory"] += new Action<Player, string>(FishToInventory);
        }

        public void FishToInventory([FromSource]Player source, string modelName)
        {
            Debug.WriteLine(modelName);
            int _source = int.Parse(source.Handle);

            TriggerEvent("vorpCore:canCarryItem", _source, modelName, 1, new Action<bool>((can) =>
            {
                if (!can)
                {
                    source.TriggerEvent("vorp:TipRight", LoadConfig.Langs["CantCarryMore"], 4000);
                }
                else
                {
                    TriggerEvent("vorpCore:addItem", _source, modelName, 1);
                }

            }));

        }
    }
}
