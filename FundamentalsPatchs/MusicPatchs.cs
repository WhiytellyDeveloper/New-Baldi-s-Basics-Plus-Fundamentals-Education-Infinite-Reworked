using HarmonyLib;
using MTM101BaldAPI.Reflection;
using nbbpfei_reworked.FundamentalsManagers;
using UnityEngine;

namespace nbbpfei_reworked.FundamentalsPatchs
{
    [HarmonyPatch(typeof(PitstopGameManager), nameof(PitstopGameManager.BeginPlay))]
    internal class PitStopMusic
    {
        public static void Prefix()
        {
            if (Singleton<CoreGameManager>.Instance.nextLevel == null)
                return;

            string floor = Singleton<CoreGameManager>.Instance.nextLevel.levelTitle;
            var midi = FundamentalsMainLoader.GetRandomFloorByName(floor).pitStopMusic;
            Singleton<MusicManager>.Instance.PlayMidi(midi, true);
        }
    }

    [HarmonyPatch(typeof(PitstopGameManager), "FieldTripTransition")]
    internal class PitStopMusicFix
    {
        public static void Postfix(bool entering, bool teleport)
        {
            if (Singleton<CoreGameManager>.Instance.nextLevel == null)
                return;

            if (entering == false && teleport == true)
            {
                string floor = Singleton<CoreGameManager>.Instance.nextLevel.levelTitle;
                var midi = FundamentalsMainLoader.GetRandomFloorByName(floor).pitStopMusic;
                Singleton<MusicManager>.Instance.PlayMidi(midi, true);
                Singleton<MusicManager>.Instance.SetSpeed(1f);
            }
            else if (entering == true && teleport == true)
                Singleton<MusicManager>.Instance.SetSpeed(0.7f);
            
        }
    }

    [HarmonyPatch(typeof(MainGameManager), nameof(MainGameManager.BeginPlay))]
    internal class StartMusic
    {
        public static void Postfix()
        {
            var scene = GameObject.FindObjectOfType<GameInitializer>().ReflectionGetVariable("sceneObject") as SceneObject;
            string floor = scene.levelTitle;

            if (FundamentalsMainLoader.GetRandomFloorByName(floor) == null)
                return;

            if (FundamentalsMainLoader.GetRandomFloorByName(floor).schoolhouseMusic == null)
                return;

            var midi = FundamentalsMainLoader.GetRandomFloorByName(floor).schoolhouseMusic;
            Singleton<MusicManager>.Instance.StopMidi();
            Singleton<MusicManager>.Instance.PlayMidi(midi, true);
        }
    }

    [HarmonyPatch(typeof(MainGameManager), "AllNotebooks")]
    internal class EndMusic
    {
        public static void Postfix()
        {
            var scene = GameObject.FindObjectOfType<GameInitializer>().ReflectionGetVariable("sceneObject") as SceneObject;
            string floor = scene.levelTitle;
            var gameMode = Singleton<CoreGameManager>.Instance.currentMode;

            if (FundamentalsMainLoader.GetRandomFloorByName(floor) == null)
            {
                Singleton<MusicManager>.Instance.PlayMidi("Level_1_End", true);
                Singleton<MusicManager>.Instance.SetSpeed(gameMode == Mode.Free ? 0.5f : 1);
                return;
            }

            var midi = FundamentalsMainLoader.GetRandomFloorByName(floor).escapeMusic;

            Singleton<MusicManager>.Instance.StopMidi();
            Singleton<MusicManager>.Instance.PlayMidi(FundamentalsMainLoader.GetRandomFloorByName(floor).escapeMusic == null ? "Level_1_End" : midi, true);
            Singleton<MusicManager>.Instance.SetSpeed(gameMode == Mode.Free ? 0.5f : 1);
        }
    }
}
