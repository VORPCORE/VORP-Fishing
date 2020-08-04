using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace vorp_fishing_cl
{
    public class Utils : BaseScript
    {

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

            public FishingMiniGameState GetData()
            {
                return new FishingMiniGameState(state,throwdistance,distance,curvature, unknown0,hookflag,transitionflag,fishentity,fishweight,
                    fishpower,scriptimer,bobberentity,hookentity,rodshakemultiplier,unknown1,unknown2,unknown3,shakefightmultiplier,fishsizeindex,
                    unknown4,unknown5,tension,roddirx,roddiry,unknown6,unknown7,unknown8,unknown9);
            }
        }

        public struct FishingMiniGameState
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

            public FishingMiniGameState(int state, float throwdistance, float distance, float curvature, float unknown0, int hookflag, int transitionflag,
                int fishentity, float fishweight, float fishpower, int scriptimer, int bobberentity, int hookentity, float rodshakemultiplier, float unknown1, float unknown2,
                int unknown3, float shakefightmultiplier, int fishsizeindex, float unknown4, float unknown5, float tension, float roddirx, float roddiry, float unknown6,
                float unknown7, float unknown8, float unknown9)
            {
                State = state;
                ThrowDistance = throwdistance;
                Distance = distance;
                Curvature = curvature;
                Unknown0 = unknown0;
                HookFlag = hookflag;
                TransitionFlag = transitionflag;
                FishEntity = fishentity;
                FishWeight = fishweight;
                FishPower = fishpower;
                ScriptTimer = scriptimer;
                BobberEntity = bobberentity;
                HookEntity = hookentity;
                RodShakeMultiplier = rodshakemultiplier;
                Unknown1 = unknown1;
                Unknown2 = unknown2;
                Unknown3 = unknown3;
                ShakeFightMultiplier = shakefightmultiplier;
                FishSizeIndex = fishsizeindex;
                Unknown4 = unknown4;
                Unknown5 = unknown5;
                Tension = tension;
                RodDirX = roddirx;
                RodDirY = roddiry;
                Unknown6 = unknown6;
                Unknwon7 = unknown7;
                Unknwon8 = unknown8;
                Unknwon9 = unknown9;
            }

            public FishingMiniGameStateUnsafe GetStruct()
            {
                return new FishingMiniGameStateUnsafe
                {
                    state = State,
                    throwdistance = ThrowDistance,
                    distance = Distance,
                    curvature = Curvature,
                    unknown0 = Unknown0,
                    hookflag = HookFlag,
                    transitionflag = TransitionFlag,
                    fishentity = FishEntity,
                    fishweight = FishWeight,
                    fishpower = FishPower,
                    scriptimer = ScriptTimer,
                    bobberentity = BobberEntity,
                    hookentity = HookEntity,
                    rodshakemultiplier = RodShakeMultiplier,
                    unknown1 = Unknown1,
                    unknown2 = Unknown2,
                    unknown3 = Unknown3,
                    shakefightmultiplier = ShakeFightMultiplier,
                    fishsizeindex = FishSizeIndex,
                    unknown4 = Unknown4,
                    unknown5 = Unknown5,
                    tension = Tension,
                    roddirx = RodDirX,
                    roddiry = RodDirY,
                    unknown6 = Unknown6,
                    unknown7 = Unknwon7,
                    unknown8 = Unknwon8,
                    unknown9 = Unknwon9
                };
            }
        }

        public static FishingMiniGameState GetFishingMiniGameState()
        {
            var data = new FishingMiniGameStateUnsafe();
            unsafe
            {
                Debug.WriteLine(((object)new IntPtr(&data)).ToString());
                Function.Call((Hash)0xF3735ACD11ACD500, API.PlayerPedId(), (new IntPtr(&data).ToInt32()));
                return data.GetData();
            }
        }

        public static async Task DrawTxt(string text, float x, float y, float fontscale, float fontsize, int r, int g, int b, int alpha, bool textcentred, bool shadow)
        {
            long str = Function.Call<long>(Hash._CREATE_VAR_STRING, 10, "LITERAL_STRING", text);
            Function.Call(Hash.SET_TEXT_SCALE, fontscale, fontsize);
            Function.Call(Hash._SET_TEXT_COLOR, r, g, b, alpha);
            Function.Call(Hash.SET_TEXT_CENTRE, textcentred);
            if (shadow) { Function.Call(Hash.SET_TEXT_DROPSHADOW, 1, 0, 0, 255); }
            Function.Call(Hash.SET_TEXT_FONT_FOR_CURRENT_COMMAND, 1);
            Function.Call(Hash._DISPLAY_TEXT, str, x, y);
        }
    }
}
