using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace vorp_fishing_cl
{
    public class FishingMinigame:BaseScript
    {
        public static FishingMiniGameStateUnsafe _fishingMinigame;

        [StructLayout(LayoutKind.Explicit, Size = 0xe0)]
        [SecurityCritical]
        public unsafe struct FishingMiniGameStateUnsafe
        {
            [FieldOffset(0x00)] public int state;
            [FieldOffset(0x08)] public float throwdistance;
            [FieldOffset(0x10)] public float distance;
            [FieldOffset(0x18)] public float curvature;
            [FieldOffset(0x20)] public float unknown0;
            [FieldOffset(0x28)] public int hookflag;
            [FieldOffset(0x30)] public int transitionflag;
            [FieldOffset(0x38)] public int fishentity;
            [FieldOffset(0x40)] public float fishweight;
            [FieldOffset(0x48)] public float fishpower;
            [FieldOffset(0x50)] public int scriptimer;
            [FieldOffset(0x58)] public int bobberentity;
            [FieldOffset(0x60)] public int hookentity;
            [FieldOffset(0x68)] public float rodshakemultiplier;
            [FieldOffset(0x70)] public float unknown1;
            [FieldOffset(0x78)] public float unknown2;
            [FieldOffset(0x80)] public int unknown3;
            [FieldOffset(0x88)] public float shakefightmultiplier;
            [FieldOffset(0x90)] public int fishsizeindex;
            [FieldOffset(0x98)] public float unknown4;
            [FieldOffset(0xA0)] public float unknown5;
            [FieldOffset(0xA8)] public float tension;
            [FieldOffset(0xB0)] public float roddirx;
            [FieldOffset(0xB8)] public float roddiry;
            [FieldOffset(0xC0)] public float unknown6;
            [FieldOffset(0xC8)] public float unknown7;
            [FieldOffset(0xD0)] public float unknown8;
            [FieldOffset(0xD8)] public float unknown9;
        }

        //Actualizar el stado del struct
        public static void GetMiniGameState()
        {
            var data = new FishingMiniGameStateUnsafe();
            unsafe
            {
                Function.Call((Hash)0xF3735ACD11ACD500, API.PlayerPedId(), new IntPtr(&data).ToInt32());
                _fishingMinigame = data;
            }
        }

        //Setear el struct con las cosas cambiadas
        public static void SetMiniGameState()
        {
            var data = new FishingMiniGameStateUnsafe();
            //Set current data state of changed things
            data.state = State;
            data.throwdistance = ThrowDistance;
            data.distance = Distance;
            data.curvature = Curvature;
            data.unknown0 = Unknown0;
            data.hookflag = HookFlag;
            data.transitionflag = TransitionFlag;
            data.fishentity = FishEntity;
            data.fishweight = FishWeight;
            data.fishpower = FishPower;
            data.scriptimer = ScriptTimer;
            data.bobberentity = BobberEntity;
            data.hookentity = HookEntity;
            data.rodshakemultiplier = RodShakeMultiplier;
            data.unknown1 = Unknown1;
            data.unknown2 = Unknown2;
            data.unknown3 = Unknown3;
            data.shakefightmultiplier = ShakeFightMultiplier;
            data.fishsizeindex = FishSizeIndex;
            data.unknown4 = Unknown4;
            data.unknown5 = Unknown5;
            data.tension = Tension;
            data.roddirx = RodDirX;
            data.roddiry = RodDirY;
            data.unknown6 = Unknown6;
            data.unknown7 = Unknown7;
            data.unknown8 = Unknown8;
            data.unknown9 = Unknown9;
            unsafe
            {
                Function.Call((Hash)0xF3735ACD11ACD501, API.PlayerPedId(), new IntPtr(&data).ToInt32());
            }
        }

        //Metodos para coger valores especificos y setearlos en el struct
        public static int State
        {
            set =>_fishingMinigame.state = value;
            get => _fishingMinigame.state;
        }

        public static float ThrowDistance
        {
            set => _fishingMinigame.throwdistance = value;
            get => _fishingMinigame.throwdistance;
        }

        public static float Distance
        {
            set => _fishingMinigame.distance = value;
            get => _fishingMinigame.distance;
        }

        public static float Curvature
        {
            set => _fishingMinigame.curvature = value;
            get => _fishingMinigame.curvature;
        }

        public static float Unknown0
        {
            set => _fishingMinigame.unknown0 = value;
            get => _fishingMinigame.unknown0;
        }

        public static int HookFlag
        {
            set => _fishingMinigame.hookflag = value;
            get => _fishingMinigame.hookflag;
        }

        public static int TransitionFlag
        {
            set => _fishingMinigame.transitionflag = value;
            get => _fishingMinigame.transitionflag;
        }

        public static int FishEntity
        {
            set => _fishingMinigame.fishentity = value;
            get => _fishingMinigame.fishentity;
        }

        public static float FishWeight
        {
            set => _fishingMinigame.fishweight = value;
            get => _fishingMinigame.fishweight;
        }

        public static float FishPower
        {
            set => _fishingMinigame.fishpower = value;
            get => _fishingMinigame.fishpower;
        }

        public static int ScriptTimer
        {
            set => _fishingMinigame.scriptimer = value;
            get => _fishingMinigame.scriptimer;
        }

        public static int BobberEntity
        {
            set => _fishingMinigame.bobberentity = value;
            get => _fishingMinigame.bobberentity;
        }

        public static int HookEntity
        {
            set => _fishingMinigame.hookentity = value;
            get => _fishingMinigame.hookentity;
        }

        public static float RodShakeMultiplier
        {
            set => _fishingMinigame.rodshakemultiplier = value;
            get => _fishingMinigame.rodshakemultiplier;
        }

        public static float Unknown1
        {
            set => _fishingMinigame.unknown1 = value;
            get => _fishingMinigame.unknown1;
        }

        public static float Unknown2
        {
            set => _fishingMinigame.unknown2 = value;
            get => _fishingMinigame.unknown2;
        }

        public static int Unknown3
        {
            set => _fishingMinigame.unknown3 = value;
            get => _fishingMinigame.unknown3;
        }

        public static float ShakeFightMultiplier
        {
            set => _fishingMinigame.shakefightmultiplier = value;
            get => _fishingMinigame.shakefightmultiplier;
        }

        public static int FishSizeIndex
        {
            set => _fishingMinigame.fishsizeindex = value;
            get => _fishingMinigame.fishsizeindex;
        }

        public static float Unknown4
        {
            set => _fishingMinigame.unknown4 = value;
            get => _fishingMinigame.unknown4;
        }

        public static float Unknown5
        {
            set => _fishingMinigame.unknown5 = value;
            get => _fishingMinigame.unknown5;
        }

        public static float Tension
        {
            set => _fishingMinigame.tension = value;
            get => _fishingMinigame.tension;
        }

        public static float RodDirX
        {
            set => _fishingMinigame.roddirx = value;
            get => _fishingMinigame.roddirx;
        }

        public static float RodDirY
        {
            set => _fishingMinigame.roddiry = value;
            get => _fishingMinigame.roddiry;
        }

        public static float Unknown6
        {
            set => _fishingMinigame.unknown6 = value;
            get => _fishingMinigame.unknown6;
        }

        public static float Unknown7
        {
            set => _fishingMinigame.unknown7 = value;
            get => _fishingMinigame.unknown7;
        }

        public static float Unknown8
        {
            set => _fishingMinigame.unknown8 = value;
            get => _fishingMinigame.unknown8;
        }

        public static float Unknown9
        {
            set => _fishingMinigame.unknown9 = value;
            get => _fishingMinigame.unknown9;
        }
    }
}
