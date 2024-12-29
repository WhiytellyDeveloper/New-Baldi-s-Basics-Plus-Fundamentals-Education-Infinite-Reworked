using HarmonyLib;
using KipoTupiniquimEngine.Extenssions;
using MTM101BaldAPI.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace nbbpfei_reworked.FundamentalsPatchs
{
    [HarmonyPatch(typeof(ElevatorScreen), nameof(ElevatorScreen.Initialize))]
    public class ElevatorScreenPatch
    {
        [HarmonyPrefix]
        public static void ChangeElevatorScreen(ElevatorScreen __instance)
        {
            random = new System.Random(Singleton<CoreGameManager>.Instance.Seed());
            chosedElevator = random.Next(0, elevatorSprite.Count);

            var allImages = __instance.ReflectionGetVariable("allImages") as Image[];
            var elvSprite = allImages.FirstOrDefault(x => x.name == "BG");
            elvSprite.sprite = elvSprite.sprite.OverlaySprite(elevatorSprite[chosedElevator]);
            elvSprite.sprite = elvSprite.sprite.OverlaySprite(elevatorFlowerSprite[UnityEngine.Random.Range(0, elevatorFlowerSprite.Count)]);
            elvSprite.sprite = elvSprite.sprite.OverlaySprite(elevatorPaintBordersSprite[UnityEngine.Random.Range(0, elevatorPaintBordersSprite.Count)]);

            var elvPaintSprite = allImages.FirstOrDefault(x => x.name == "Image");
            elvPaintSprite.sprite = elevatorPaintingsSprite[UnityEngine.Random.Range(0, elevatorPaintingsSprite.Count)].ConvertTexToSpriteWithBase(elvPaintSprite.sprite);


            var seedText = __instance.ReflectionGetVariable("seedText") as TMP_Text;
            seedText.text = Singleton<CoreGameManager>.Instance.Seed().ToString();
        }

        public static List<Texture2D> elevatorPaintingsSprite = [];
        public static List<Texture2D> elevatorSprite = [];
        public static List<Texture2D> elevatorTubes = [];
        public static List<Texture2D> elevatorFloor = [];
        public static List<Texture2D> elevatorFlowerSprite = [];
        public static List<Texture2D> elevatorPaintBordersSprite = [];
        public static int chosedElevator = 0;
        protected static System.Random random;
    }

    [HarmonyPatch(typeof(ElevatorScreen), "UpdateLives")]
    public class ElevatorScreenPatchFix
    {
        [HarmonyPrefix]
        public static void ChangeLives(ElevatorScreen __instance)
        {
            var allImages = __instance.ReflectionGetVariable("allImages") as Image[];
            var elevatorTubes = ElevatorScreenPatch.elevatorTubes;
            var livesSprite = allImages.FirstOrDefault(x => x.name == "Lives");
            __instance.ReflectionSetVariable("lifeImages", new Sprite[] { elevatorTubes[0].ConvertTexToSpriteWithBase(livesSprite.sprite), elevatorTubes[1].ConvertTexToSpriteWithBase(livesSprite.sprite), elevatorTubes[2].ConvertTexToSpriteWithBase(livesSprite.sprite), elevatorTubes[3].ConvertTexToSpriteWithBase(livesSprite.sprite) });
        }
    }

    [HarmonyPatch(typeof(Elevator), "Initialize")]
    public class ElevatorPatch
    {
        [HarmonyPrefix]
        public static void ChangeFloor(Elevator __instance)
        {
            var quads = __instance.GetComponentsInChildren<Transform>().First(x => x.name == "Quads");
            var floor = quads.GetComponentsInChildren<MeshRenderer>().First(x => x.name == "Floor");
            floor.material.SetTexture("_MainTex", ElevatorScreenPatch.elevatorFloor[ElevatorScreenPatch.chosedElevator]);
        }
    }
}
