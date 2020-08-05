using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vorp_fishing_cl
{
    class Fishing:BaseScript
    {
        public Fishing()
        {
            Tick += DebugFishing;
        }

        private async Task DebugFishing()
        {
            await Delay(1000);
            try
            {
                FishingMinigame.GetMiniGameState();
                Debug.WriteLine(FishingMinigame.State.ToString());
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            
        }
    }
}
