using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vorp_fishing_cl
{
    public class FishingMinigame:BaseScript
    {
        public int State { get; set; }
        public float ThrowDistance { set; get; }
        public float Distance { set; get; }
        public float Curvature { set; get; }
        public float Unknown0 { set; get; }
        public int HookFlag { set; get; }
        public int TransitionFlag { set; get; }
        public int FishEntity { set; get; }
        public float FishWeight { set; get; }
        public float FishPower { set; get; }
        public int ScriptTimer { set; get; }
        public int BobberEntity { set; get; }
        public int HookEntity { set; get; }
        public float RodShakeMultiplier { set; get; }
        public float Unknown1 { set; get; }
        public float Unknown2 { set; get; }
        public int Unknown3 { set; get; }
        public float ShakeFightMultiplier { set; get; }
        public int FishSizeIndex { set; get; }
        public float Unknown4 { set; get; }
        public float Unknown5 { set; get; }
        public float Tension { set; get; }
        public float RodDirX { set; get; }
        public float RodDirY { set; get; }
        public float Unknown6 { set; get; }
        public float Unknwon7 { set; get; }
        public float Unknwon8 { set; get; }
        public float Unknwon9 { set; get; }

        public FishingMinigame() { }
    }
}
