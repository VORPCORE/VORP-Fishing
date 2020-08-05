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

        public static Dictionary<int, string> FishModels = new Dictionary<int, string>()
        {
            {API.GetHashKey("a_c_fishbluegil_01_ms"), "a_c_fishbluegil_01_ms"},
            {API.GetHashKey("a_c_fishbluegil_01_sm"), "a_c_fishbluegil_01_sm"},
            {API.GetHashKey("a_c_fishbullheadcat_01_ms"), "a_c_fishbullheadcat_01_ms"},
            {API.GetHashKey("a_c_fishbullheadcat_01_sm"), "a_c_fishbullheadcat_01_sm"},
            {API.GetHashKey("a_c_fishchainpickerel_01_ms"), "a_c_fishchainpickerel_01_ms"},
            {API.GetHashKey("a_c_fishchainpickerel_01_sm"), "a_c_fishchainpickerel_01_sm"},
            {API.GetHashKey("a_c_fishchannelcatfish_01_xl"), "a_c_fishchannelcatfish_01_xl"},
            {API.GetHashKey("a_c_fishchannelcatfish_01_lg"), "a_c_fishchannelcatfish_01_lg"},
            {API.GetHashKey("a_c_fishlakesturgeon_01_lg"), "a_c_fishlakesturgeon_01_lg"},
            {API.GetHashKey("a_c_fishlargemouthbass_01_lg"), "a_c_fishlargemouthbass_01_lg"},
            {API.GetHashKey("a_c_fishlargemouthbass_01_ms"), "a_c_fishlargemouthbass_01_ms"},
            {API.GetHashKey("a_c_fishlongnosegar_01_lg"), "a_c_fishlongnosegar_01_lg"},
            {API.GetHashKey("a_c_fishmuskie_01_lg"), "a_c_fishmuskie_01_lg"},
            {API.GetHashKey("a_c_fishnorthernpike_01_lg"), "a_c_fishnorthernpike_01_lg"},
            {API.GetHashKey("a_c_fishperch_01_ms"), "a_c_fishperch_01_ms"},
            {API.GetHashKey("a_c_fishperch_01_sm"), "a_c_fishperch_01_sm"},
            {API.GetHashKey("a_c_fishredfinpickerel_01_ms"), "a_c_fishredfinpickerel_01_ms"},
            {API.GetHashKey("a_c_fishredfinpickerel_01_sm"), "a_c_fishredfinpickerel_01_sm"},
            {API.GetHashKey("a_c_fishrockbass_01_ms"), "a_c_fishrockbass_01_ms"},
            {API.GetHashKey("a_c_fishrockbass_01_sm"), "a_c_fishrockbass_01_sm"},
            {API.GetHashKey("a_c_fishsmallmouthbass_01_lg"), "a_c_fishsmallmouthbass_01_lg"},
            {API.GetHashKey("a_c_fishsmallmouthbass_01_ms"), "a_c_fishsmallmouthbass_01_ms"},
            {API.GetHashKey("a_c_fishsalmonsockeye_01_lg"), "a_c_fishsalmonsockeye_01_lg"},
            {API.GetHashKey("a_c_fishsalmonsockeye_01_ms"), "a_c_fishsalmonsockeye_01_ms"},
            {API.GetHashKey("a_c_fishrainbowtrout_01_lg"), "a_c_fishrainbowtrout_01_lg"},
            {API.GetHashKey("a_c_fishrainbowtrout_01_ms"), "a_c_fishrainbowtrout_01_ms"}
        };

        public static float Lerp(float a, float b, float t)
        {
            return a * (1 - t) + b * t;
        }

        public static float fishOzs(uint model, float size)
        {
            Vector3 minDim = new Vector3();
            Vector3 maxDim = new Vector3();
            API.GetModelDimensions(model, ref minDim, ref maxDim);
            Vector3 dim = maxDim - minDim;
            float area = dim.X * dim.Y * dim.Z;
            return size * area * 1726.80833367f;
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
