﻿using CampfireFrenzy;
using HarmonyLib;
using MTM101BaldAPI.Reflection;
using nbbpfei_reworked.FundamentalsOptions;
using nbbpfei_reworked.FundamentalsPlayerData;
using PicnicPanic;
using System.Collections;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace nbbpfei_reworked.FundamentalsPatchs
{
    [HarmonyPatch(typeof(BaseGameManager), nameof(BaseGameManager.BeginPlay))]
    public class StarterDebugPatches
    {
        public static void Postfix(BaseGameManager __instance)
        {
            if (!PlayerDataLoader.GetPlayer().debugMode)
                return;

            __instance.Ec.map.CompleteMap();

            if (!GameObject.FindObjectOfType<HappyBaldi>())
                return;

            if (Singleton<CoreGameManager>.Instance.sceneObject.levelTitle == "WIP")
                return;

            var happyBaldi = GameObject.FindObjectOfType<HappyBaldi>();
            Singleton<BaseGameManager>.Instance.BeginSpoopMode();
            __instance.Ec.SpawnNPCs();

            if (Singleton<CoreGameManager>.Instance.currentMode == Mode.Main) __instance.Ec.GetBaldi().transform.position = happyBaldi.transform.position;
            else __instance.Ec.GetBaldi().Despawn();

            __instance.Ec.StartEventTimers();

            happyBaldi.StartCoroutine(WaitASecond(happyBaldi.gameObject));
        }

        public static IEnumerator WaitASecond(GameObject happyBaldi)
        {
            yield return new WaitForSeconds(0.01f);

            if (SkipFloorPatches.skiped)
            {
                Singleton<BaseGameManager>.Instance.LoadNextLevel();
                SkipFloorPatches.skiped = false;
            }

            GameObject.Destroy(happyBaldi);
            Singleton<MusicManager>.Instance.StopMidi();
        }
    }

    [HarmonyPatch(typeof(ElevatorScreen), nameof(ElevatorScreen.SkipPitstop))]
    public class SkipFloorPatches
    {
        public static bool skiped;

        public static bool Prefix()
        {
            if (Singleton<BaseGameManager>.Instance as PitstopGameManager)
                return true;

            return false;
        }

        public static void Postfix(ElevatorScreen __instance)
        {
            if (Singleton<BaseGameManager>.Instance as PitstopGameManager)
                return;

            var buttonAnimator = __instance.ReflectionGetVariable("buttonAnimator") as Animator;
            buttonAnimator.Play("ButtonDrop", -1, 0f);

            var elevatorScreen = Singleton<ElevatorScreen>.Instance;
            Singleton<ElevatorScreen>.Instance.StartGame();
            skiped = true;
        }
        
    }

    [HarmonyPatch(typeof(ElevatorScreen), "Start")]
    public class SkipFloorPatchesFix
    {
        public static void Postfix()
        {
            var scene = Singleton<CoreGameManager>.Instance.sceneObject;

            if (scene.levelTitle == "PIT")
                return;

            if (!PlayerDataLoader.GetPlayer().debugMode)
            {
                scene.skippable = false;
                return;
            }

            scene.skippable = scene.nextLevel != null;
        }
    }

    [HarmonyPatch]
    public class PicnicPanicCheats
    {
        [HarmonyPatch(typeof(PlateController), nameof(PlateController.Open)), HarmonyPostfix]

        public static void ThrowBombs(PlateController __instance)
        {
            if (__instance.IsBomb || (bool)__instance.ReflectionGetVariable("valid") && PlayerDataLoader.GetPlayer().debugMode)
                __instance.Pressed();
        }
    }

    [HarmonyPatch]
    public class CampfireCheats
    {
        [HarmonyPatch(typeof(Minigame_Campfire), nameof(Minigame_Campfire.VirtualUpdate)), HarmonyPostfix]

        public static void ThrowBombs(Minigame_Campfire __instance)
        {
            var loadedFuelGroup = __instance.ReflectionGetVariable("loadedFuelGroup") as FuelGroup;

            if (__instance.launcherHasFuel && loadedFuelGroup.LandedFuelIsGood && PlayerDataLoader.GetPlayer().debugMode)
                __instance.ReflectionInvoke("Launch", []);
        }
    }
}
