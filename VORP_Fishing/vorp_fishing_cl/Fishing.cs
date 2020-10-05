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
        private static bool hooked = false;

        private static int fish = -1;
        private static float fishDistance = 0.0f;
        static List<int> fishesInArea = new List<int>();

        private static float range = 15f;
        private static int volumenCilinder = -1;
        private static int itemSet = -1;

        private static float[] reelSpeeds = { 0.0125f, 0.0375f };
        private static float actualReelSpeed = 0.125f;

        private static bool isCatched = false;
        private static int catchedFish = -1;
        private static int catchedTime = 0;
        private static int lastFlee = -1;
        private static int startFlee = -1;
        private static bool FleeIng = false;

        private static int fleeSoundId = -1;

        private static int FX_Drip = -1;

        //Prompts
        private static bool showing = false;
        private static bool givedFish = false;
        private static int ResetCastPrompt = -1;
        private static int ReelInPrompt = -1;
        private static int SavePrompt = -1;
        private static int BackPrompt = -1;

        public Fishing()
        {
            API.DecorRegister("FSize", 1);
            API.DecorRegister("FBaitInt", 1);

            fleeSoundId = API.GetSoundId();

            volumenCilinder =
                Function.Call<int>((Hash) 0x0522D4774B82E3E6, 0f, 0f, 0f, 0f, 0f, 0f, range, range, range);
            itemSet = API.CreateItemset(true);

            //Tick += DebugFishingMG; //debug
            Tick += FeedFish;
            Tick += CheckFishingState;
            Tick += ControlFishingMG;
            Tick += ShowPrompts;

            EventHandlers["vorp_fishing:UseBait"] += new Action(UseBait);

            EventHandlers["onResourceStop"] += new Action<string>(ClearCache);
        }

        private void UseBait()
        {
            uint weaponHash = 0;
            API.GetCurrentPedWeapon(API.PlayerPedId(), ref weaponHash, true, 0, true);

            if (weaponHash != 0 && weaponHash == 0xABA87754)
            {
                Function.Call((Hash)0x2C28AC30A72722DA, API.PlayerPedId(), "p_baitBread01x", 0);
                TriggerServerEvent("vorp_fishing:baitUsed");
            }
            else
            {
                TriggerEvent("vorp:TipRight", GetConfig.Langs["NotRod"], 2000);
            }
        }

        private async Task FeedFish()
        {
            await Delay(300);
            fish = -1;
            fishDistance = range;
            fishesInArea.Clear();
            if (volumenCilinder != -1 && (FishingMinigame.State == 6 || FishingMinigame.State == 7))
            {
                int hookEntity = FishingMinigame.HookEntity;
                Vector3 hookCoords = API.GetEntityCoords(hookEntity, true, true);
                float hookHeading = API.GetEntityHeading(hookEntity);
                Vector3 fishingCoords = API.GetOffsetFromEntityInWorldCoords(API.PlayerPedId(), 0f, 20f, 0f);
                Function.Call((Hash)0x541B8576615C33DE, volumenCilinder, fishingCoords.X, fishingCoords.Y, fishingCoords.Z - (range / 2));
                Function.Call((Hash)0xA07CF1B21B56F041, volumenCilinder, 0f, 0f, hookHeading);
                Function.Call((Hash)0xA46E98BDC407E23D, volumenCilinder, range, range, range);
                int items = Function.Call<int>((Hash)0x886171A12F400B89, volumenCilinder, itemSet, 1);
                if (items != 0)
                {
                    for (int i = 0; i < items; i++)
                    {
                        int entity = API.GetIndexedItemInItemset(i, itemSet);
                        int model = API.GetEntityModel(entity);
                       
                        if (Utils.FishModels.ContainsKey(model) && !API.IsPedDeadOrDying(entity, true))
                        {
                            float size = API.DecorGetFloat(entity, "FSize");
                            if (size == 0f)
                            {
                                Random rnd = new Random();
                                size = rnd.Next(50, 150) / 100;
                                API.DecorSetFloat(entity, "FSize", size);
                            }
                            fishesInArea.Add(entity);
                            Vector3 entityCoords = API.GetEntityCoords(entity, true, true);
                            float distance = hookCoords.DistanceToSquared2D(entityCoords);
                            if (distance < fishDistance)
                            {
                                fish = entity;
                                fishDistance = distance;
                                //Function.Call((Hash) 0xb3426bcc, entityCoords.X, entityCoords.Y, entityCoords.Z,
                                //    entityCoords.X, entityCoords.Y, entityCoords.Z + 1.5F, 50, 255, 50, 255);
                            }

                            float baitInt = API.DecorGetFloat(entity, "FBaitInt");
                            if (distance < 8f &&  FishingMinigame.State == 6)
                            {
                                baitInt += Utils.Lerp(1f, 0.05f, distance / 1000);
                            }
                            else
                            {
                                baitInt = 0.0f;
                            }
                            if (baitInt >= 10f)
                            {
                                API.TaskGoToEntity(entity, hookEntity, 6000, 0.05f, 1f, 0, 0);
                            }

                            API.DecorSetFloat(entity, "FBaitInt", baitInt);
                        }
                    }
                }
                Function.Call((Hash)0x20A4BF0E09BEE146, itemSet);
            }

        }

        public async Task ControlFishingMG()
        {
            int actualState = FishingMinigame.State;

            if (actualState == 6 && actualState == 7)
            {
                catchedTime = -1;
                lastFlee = -1;
            }

            if (actualState == 0)
            {
                catchedFish = -1; //A REVISAR
                fish = -1;
            }

            if (actualState == 0)
            {
                await Delay(100);
            }

            if (actualState == 4)
            {
                actualReelSpeed = reelSpeeds[0];
            }

            if (actualState == 6 || actualState == 7)
            {
                if (actualState == 6)
                {
                    //+ Speed
                    if (API.IsControlPressed(0, 0xE30CD707))
                    {
                        if (actualReelSpeed < reelSpeeds[1])
                        {
                            actualReelSpeed = Math.Min(reelSpeeds[1], actualReelSpeed + 0.0025f);
                        }
                    }
                    //- Speed
                    if (API.IsControlPressed(0, 0XB2F377E8))
                    {
                        if (actualReelSpeed > reelSpeeds[0])
                        {
                            actualReelSpeed = Math.Max(reelSpeeds[1], actualReelSpeed - 0.0025f);
                        }
                    }
                }

                if (actualState == 7)
                {
                    if (API.IsControlPressed(2, 0xF84FA74F))
                    {
                        FishingMinigame.RodDirX = 1f - API.GetControlNormal(2, 0xD6C4ECDC);
                        FishingMinigame.RodDirY = 1f - API.GetControlNormal(2, 0xE4130778);
                    }
                    else
                    {
                        FishingMinigame.RodDirX = 0f;
                        FishingMinigame.RodDirY = 0f;
                    }
                    Random rnd = new Random();
                    if (!FleeIng && lastFlee != -1 && API.GetGameTimer() - lastFlee > rnd.Next(7000, 15000))
                    {
                        FleeIng = true;
                        startFlee = API.GetGameTimer();
                    }

                    if (FleeIng && lastFlee != -1 && API.GetGameTimer() - startFlee > 5000)
                    {
                        FleeIng = false;
                        startFlee = -1;
                        lastFlee = API.GetGameTimer();
                    }
                }

                if (API.PromptHasHoldModeCompleted(ResetCastPrompt))
                {
                    if (actualState == 7)
                    {
                        FishingMinigame.TransitionFlag = 2;
                        Function.Call((Hash)0x9B0C7FA063E67629, API.PlayerPedId(), "", false, true);
                    }
                    else
                    {
                        FishingMinigame.TransitionFlag = 128;
                    }

                    hooked = false;
                    isCatched = false;
                    FishingMinigame.SetMiniGameState();
                }

                if (FleeIng)
                {
                    Random rnd = new Random();
                    
                    if (rnd.NextDouble() > 0.85)
                    {
                        Vector3 fishCoords = API.GetEntityCoords(FishingMinigame.FishEntity, true, true);
                        API.UseParticleFxAsset("scr_mg_fishing");
                        Function.Call(Hash.START_NETWORKED_PARTICLE_FX_NON_LOOPED_AT_COORD, "scr_mg_fish_struggle",
                            fishCoords.X, fishCoords.Y, fishCoords.Z + 0.4F, 0f, 0f, (float) (rnd.NextDouble() * 360.0), 1f, 0, 0, 0);
                        Function.Call((Hash)0xDCF5BA95BBF0FABA, fleeSoundId, API.GetHashKey("VFX_SPLASH"), fishCoords.X, fishCoords.Y, fishCoords.Z, false, 0, 0, 1);
                        Function.Call((Hash)0x503703EC1781B7D6, fleeSoundId, API.GetHashKey("FishSize"), API.PlayerPedId(), 1f);
                    }

                    Vector3 playerCoords = API.GetEntityCoords(API.PlayerPedId(), true, true);

                    if (!API.IsPedFleeing(FishingMinigame.FishEntity))
                    {
                        API.ClearPedTasksImmediately(FishingMinigame.FishEntity, 1, 0);
                        Function.Call(Hash.TASK_SMART_FLEE_COORD, FishingMinigame.FishEntity, playerCoords.X, playerCoords.Y, playerCoords.Z, 10f, 6000, 8, 3f);
                    }

                    float actualTension = FishingMinigame.Tension;
                    if (API.PromptHasHoldModeCompleted(ReelInPrompt))
                    {
                        actualTension += 0.005f;
                        if (actualTension > 1f)
                        {
                            FishingMinigame.TransitionFlag = 2;
                            actualTension = 0f;
                            FleeIng = false;
                            lastFlee = -1;
                            startFlee = -1;

                            hooked = false;
                            isCatched = false;
                            Function.Call((Hash)0x9B0C7FA063E67629, API.PlayerPedId(), "", false, true);
                            TriggerEvent("vorp:TipRight", GetConfig.Langs["EscapeFish"], 2000);
                        }
                    }
                    else
                    {
                        actualTension = Math.Max(actualTension - 0.0025f, 0f);
                    }

                    FishingMinigame.Tension = actualTension;
                    FishingMinigame.RodShakeMultiplier = actualTension * 1.5f;
                    FishingMinigame.ShakeFightMultiplier = actualTension * 2.0f;
                }

                if (API.PromptHasHoldModeCompleted(ReelInPrompt))
                {
                    if (!FleeIng)
                    {
                        FishingMinigame.Tension = 0f;
                        FishingMinigame.RodShakeMultiplier = 0f;
                        FishingMinigame.ShakeFightMultiplier = 0f;

                        if (actualState == 7 && API.IsPedFleeing(FishingMinigame.FishEntity))
                            API.ClearPedTasksImmediately(FishingMinigame.FishEntity, 1, 0);

                        Vector3 playerCoords;

                        if (actualState == 7)
                        {
                            float rodX = (1 - FishingMinigame.RodDirX) * 2f - 1f;
                            playerCoords = API.GetOffsetFromEntityInWorldCoords(API.PlayerPedId(), rodX, 0f, 0f);
                        }
                        else
                        {
                            playerCoords = API.GetEntityCoords(API.PlayerPedId(), true, true);
                        }

                        Vector3 bobberCoords = API.GetEntityCoords(FishingMinigame.BobberEntity, true, true);

                        Vector3 dest = playerCoords - bobberCoords;
                        Vector3.Normalize(ref dest, out dest);
                        dest = bobberCoords + dest * actualReelSpeed;
                        API.SetEntityCoords(FishingMinigame.BobberEntity, dest.X, dest.Y, dest.Z, false, false, false, false);
                        if (FishingMinigame.Distance < 5f)
                        {
                            FishingMinigame.TransitionFlag = 8;
                            Function.Call((Hash)0x9B0C7FA063E67629, API.PlayerPedId(), "", false, true);
                        }
                        else if (FishingMinigame.Distance > 37f)
                        {
                            FishingMinigame.TransitionFlag = 2;
                            Function.Call((Hash)0x9B0C7FA063E67629, API.PlayerPedId(), "", false, true);
                            TriggerEvent("vorp:TipRight", GetConfig.Langs["EscapeFish"], 2000);
                        }
                    }
                }
                else
                {
                    if (actualState == 7)
                    {
                        Vector3 playerCoords = API.GetEntityCoords(API.PlayerPedId(), true, true);
                        if (!API.IsPedFleeing(FishingMinigame.FishEntity))
                        {
                            API.ClearPedTasksImmediately(FishingMinigame.FishEntity, 1, 0);
                            Function.Call(Hash.TASK_SMART_FLEE_COORD, FishingMinigame.FishEntity, playerCoords.X, playerCoords.Y, playerCoords.Z, 10f, 6000, 8, 3f);
                        }
                    }
                }

                FishingMinigame.SetMiniGameState();
            }

        }

        public async Task CheckFishingState()
        {

            FishingMinigame.GetMiniGameState();

            int actualState = FishingMinigame.State;
            int catchFish = fish;
            float catchFishDistance = fishDistance;
            if (FishingMinigame.State == actualState && !hooked)
            {
                if (catchFish != -1)
                {
                    float baitInt = API.DecorGetFloat(catchFish, "FBaitInt");

                    if (baitInt > 20f && catchFishDistance < 3.0f)
                    {
                        uint model = (uint)API.GetEntityModel(catchFish);
                        float size = API.DecorGetFloat(catchFish, "FSize");
                        float ozs = Utils.fishOzs(model, size);
                        actualReelSpeed = Utils.Lerp(reelSpeeds[1] * 0.7f, reelSpeeds[0] * 0.7f, ozs / 500f);
                        hooked = true;
                        isCatched = false;
                        API.SetEntityInvincible(catchFish, true);
                        API.SetPedConfigFlag(catchFish, 17, true);
                        API.SetBlockingOfNonTemporaryEvents(catchFish, true);
                        API.ClearPedTasksImmediately(catchFish, 0, 1);
                        Function.Call((Hash)0x1F298C7BD30D1240, API.PlayerPedId());
                        Function.Call((Hash)0x1A52076D26E09004, API.PlayerPedId(), catchFish);
                        FishingMinigame.TransitionFlag = 4;
                        FishingMinigame.FishEntity = catchFish;
                        FishingMinigame.FishWeight = 3.0f;
                        FishingMinigame.FishPower = 1.0f;
                        catchedFish = catchFish;
                        Vector3 hookCoords = API.GetEntityCoords(FishingMinigame.HookEntity, true, true);
                        Function.Call(Hash.TASK_SMART_FLEE_COORD ,catchFish, hookCoords.X, hookCoords.Y, hookCoords.Z, 10f, 1000, 8, 3f);
                        catchedTime = API.GetGameTimer();
                        lastFlee = catchedTime;
                        API.DecorSetFloat(catchedFish, "FBaitInt", 0f);
                        FishingMinigame.SetMiniGameState();
                    }
                }
            }
            else if (actualState == 12)
            {
                if (!isCatched)
                {
                    FX_Drip = Function.Call<int>(Hash.START_NETWORKED_PARTICLE_FX_LOOPED_ON_ENTITY,
                        "scr_mg_fishing_drips", catchFish, 0f, 0f, 0f, 0f, 0f, 0f, 1f, 0, 0, 0);
                    Function.Call(Hash.SET_PARTICLE_FX_LOOPED_EVOLUTION, FX_Drip, "fade", 0f, false);
                    isCatched = true;
                }

                if (API.PromptHasHoldModeCompleted(BackPrompt))
                {
                    FishingMinigame.TransitionFlag = 64;
                    FishingMinigame.SetMiniGameState();

                    await Delay(2000);

                    API.SetBlockingOfNonTemporaryEvents(catchFish, false);
                    API.SetEntityInvincible(catchFish, false);
                    API.SetPedConfigFlag(catchFish, 17, false);

                    hooked = false;

                    API.DeleteEntity(ref catchFish);
                    API.DeletePed(ref catchFish);
                }

                if (API.PromptHasHoldModeCompleted(SavePrompt) && !givedFish)
                {
                    API.PromptSetEnabled(SavePrompt, 0);
                    API.PromptSetVisible(SavePrompt, 0);
                    givedFish = true;
                    showing = false;
                    int fEnt = FishingMinigame.FishEntity;
                    if (Utils.FishModels.ContainsKey(API.GetEntityModel(fEnt)))
                    {
                        FishingMinigame.TransitionFlag = 32;
                        FishingMinigame.SetMiniGameState();

                        hooked = false;

                        if (Utils.FishModels[API.GetEntityModel(fEnt)].EndsWith("_lg")) //BigFish
                        {
                            API.SetBlockingOfNonTemporaryEvents(fEnt, false);
                            API.SetEntityInvincible(fEnt, false);
                            API.SetPedConfigFlag(fEnt, 17, false);
                        }
                        else
                        {
                            Debug.WriteLine(Utils.FishModels[API.GetEntityModel(fEnt)]);
                            TriggerServerEvent("vorp_fishing:FishToInventory", Utils.FishModels[API.GetEntityModel(fEnt)]);
                            API.DeleteEntity(ref fEnt);
                            API.DeletePed(ref fEnt);
                        }

                    }
                    givedFish = false;
                }
                await Delay(500);

            }
            else
            {
                if (FX_Drip != -1 && API.DoesParticleFxLoopedExist(FX_Drip))
                {
                    API.StopParticleFxLooped(FX_Drip, true);
                    FX_Drip = -1;
                }
            }

            await Delay(10);
        }

        public static void SetupPrompts()
        {
            ResetCastPrompt = API.PromptRegisterBegin();
            Function.Call((Hash)0xB5352B7494A08258, ResetCastPrompt, 0x156F7119);
            long resetCastSTR = Function.Call<long>(Hash._CREATE_VAR_STRING, 10, "LITERAL_STRING", GetConfig.Langs["Cancel"]);
            Function.Call((Hash)0x5DD02A8318420DD7, ResetCastPrompt, resetCastSTR);
            API.PromptSetEnabled(ResetCastPrompt, 0);
            API.PromptSetVisible(ResetCastPrompt, 0);
            API.PromptSetHoldMode(ResetCastPrompt, 1);
            API.PromptRegisterEnd(ResetCastPrompt);

            ReelInPrompt = API.PromptRegisterBegin();
            Function.Call((Hash)0xB5352B7494A08258, ReelInPrompt, 0x8FFC75D6);
            long reelInStr = Function.Call<long>(Hash._CREATE_VAR_STRING, 10, "LITERAL_STRING", GetConfig.Langs["ReelIn"]);
            Function.Call((Hash)0x5DD02A8318420DD7,ReelInPrompt, reelInStr);
            API.PromptSetEnabled(ReelInPrompt, 0);
            API.PromptSetVisible(ReelInPrompt, 0);
            API.PromptSetHoldMode(ReelInPrompt, 1);
            API.PromptRegisterEnd(ReelInPrompt);

            SavePrompt = API.PromptRegisterBegin();
            Function.Call((Hash)0xB5352B7494A08258, SavePrompt, 0xCEFD9220);
            long saveStr = Function.Call<long>(Hash._CREATE_VAR_STRING, 10, "LITERAL_STRING", GetConfig.Langs["SaveFish"]);
            Function.Call((Hash)0x5DD02A8318420DD7, SavePrompt, saveStr);
            API.PromptSetEnabled(SavePrompt, 0);
            API.PromptSetVisible(SavePrompt, 0);
            API.PromptSetHoldMode(SavePrompt, 1);
            API.PromptRegisterEnd(SavePrompt);

            BackPrompt = API.PromptRegisterBegin();
            Function.Call((Hash)0xB5352B7494A08258, BackPrompt, 0XB2F377E8);
            long backStr = Function.Call<long>(Hash._CREATE_VAR_STRING, 10, "LITERAL_STRING", GetConfig.Langs["FreeFish"]);
            Function.Call((Hash)0x5DD02A8318420DD7, BackPrompt, backStr);
            API.PromptSetEnabled(BackPrompt, 0);
            API.PromptSetVisible(BackPrompt, 0);
            API.PromptSetHoldMode(BackPrompt, 1);
            API.PromptRegisterEnd(BackPrompt);
        }

        public async Task ShowPrompts()
        {
            await Delay(10);
            int actualState = FishingMinigame.State;
            if (actualState == 7 || actualState == 6)
            {
                if (!showing)
                {
                    if (!API.PromptIsActive(ReelInPrompt))
                    {
                        API.PromptSetEnabled(ReelInPrompt, 1);
                        API.PromptSetVisible(ReelInPrompt, 1);
                    }
                    if (!API.PromptIsActive(ResetCastPrompt))
                    {
                        API.PromptSetEnabled(ResetCastPrompt, 1);
                        API.PromptSetVisible(ResetCastPrompt, 1);
                    }

                    showing = true;
                }
            }
            else if (actualState == 12 && FishingMinigame.TransitionFlag != 32 && showing)
            {
                API.PromptSetEnabled(ReelInPrompt, 0);
                API.PromptSetVisible(ReelInPrompt, 0);
                API.PromptSetEnabled(ResetCastPrompt, 0);
                API.PromptSetVisible(ResetCastPrompt, 0);
                API.PromptSetEnabled(SavePrompt, 1);
                API.PromptSetVisible(SavePrompt, 1);
                API.PromptSetEnabled(BackPrompt, 1);
                API.PromptSetVisible(BackPrompt, 1);
            }
            else
            {
                API.PromptSetEnabled(ReelInPrompt, 0);
                API.PromptSetVisible(ReelInPrompt, 0);
                API.PromptSetEnabled(ResetCastPrompt, 0);
                API.PromptSetVisible(ResetCastPrompt, 0);
                API.PromptSetEnabled(SavePrompt, 0);
                API.PromptSetVisible(SavePrompt, 0);
                API.PromptSetEnabled(BackPrompt, 0);
                API.PromptSetVisible(BackPrompt, 0);
                showing = false;
            }
        }

        private async Task DebugFishingMG()
        {
            string debugText = "";
            try
            {
                FishingMinigame.GetMiniGameState();


                debugText += $"State: {FishingMinigame.State} \n";
                debugText += $"TrowDist: {FishingMinigame.ThrowDistance} \n";
                debugText += $"Distance: {FishingMinigame.Distance} \n";
                debugText += $"Curvature: {FishingMinigame.Curvature} \n";
                debugText += $"N0: {FishingMinigame.Unknown0} \n";
                debugText += $"HookFlag: {FishingMinigame.HookFlag} \n";
                debugText += $"TransFlag: {FishingMinigame.TransitionFlag} \n";
                debugText += $"F Ent: {FishingMinigame.FishEntity} \n";
                debugText += $"F Weight: {FishingMinigame.FishWeight} \n";
                debugText += $"F Power: {FishingMinigame.FishPower} \n";
                debugText += $"Time: {FishingMinigame.ScriptTimer} \n";
                debugText += $"Bobber Ent: {FishingMinigame.BobberEntity} \n";
                debugText += $"Hook Ent: {FishingMinigame.HookEntity} \n";
                debugText += $"RShake*: {FishingMinigame.RodShakeMultiplier} \n";
                debugText += $"N1: {FishingMinigame.Unknown1} \n";
                debugText += $"N2: {FishingMinigame.Unknown2} \n";
                debugText += $"N3: {FishingMinigame.Unknown3} \n";
                debugText += $"ShakeFight*: {FishingMinigame.ShakeFightMultiplier} \n";
                debugText += $"FishSizeIx: {FishingMinigame.FishSizeIndex} \n";
                debugText += $"N4: {FishingMinigame.Unknown4} \n";
                debugText += $"N5: {FishingMinigame.Unknown5} \n";
                debugText += $"Tension: {FishingMinigame.Tension} \n";
                debugText += $"RodDirX: {FishingMinigame.RodDirX} \n";
                debugText += $"RodDirY: {FishingMinigame.RodDirY} \n";
                debugText += $"N6: {FishingMinigame.Unknown6} \n";
                debugText += $"N7: {FishingMinigame.Unknown7} \n";
                debugText += $"N8: {FishingMinigame.Unknown8} \n";
                debugText += $"N9: {FishingMinigame.Unknown9} \n";
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            string fishText = "";
            foreach (var ent in fishesInArea)
            {
                fishText += Utils.FishModels[API.GetEntityModel(ent)] + " Bait: " + API.DecorGetFloat(ent, "FBaitInt") + "\n";
            }

            await Utils.DrawTxt(debugText, 0.1f, 0.2f, 0.25f, 0.25f, 255, 255, 255, 255, false, false);

            await Utils.DrawTxt(fishText, 0.75f, 0.1f, 0.25f, 0.25f, 255, 255, 255, 255, false, false);
        }

        private void ClearCache(string resName)
        {
            if (resName == API.GetCurrentResourceName())
            {
                API.PromptSetEnabled(ReelInPrompt, 0);
                API.PromptSetVisible(ReelInPrompt, 0);
                API.PromptSetEnabled(ResetCastPrompt, 0);
                API.PromptSetVisible(ResetCastPrompt, 0);
                API.PromptSetEnabled(SavePrompt, 0);
                API.PromptSetVisible(SavePrompt, 0);
                API.PromptSetEnabled(BackPrompt, 0);
                API.PromptSetVisible(BackPrompt, 0);
                Function.Call((Hash)0x43F867EF5C463A53, volumenCilinder);
                API.DestroyItemset(itemSet);
            }
        }
    }
}
