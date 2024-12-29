using BepInEx;
using HarmonyLib;
using KipoTupiniquimEngine.Extenssions;
using MTM101BaldAPI;
using MTM101BaldAPI.UI;
using nbbpfei_reworked.FundamentalsPlayerData;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace nbbpfei_reworked.FundamentalsPatchs
{

    [HarmonyPatch(typeof(MainMenu), "Start")]
    public class BetaTesterPasswordScreen
    {
        [HarmonyPostfix]
        public static void Postfix(NameManager __instance)
        {
            try
            {
                var value = PlayerDataLoader.GetPlayer().isBetaTester;

                if (value && PlayerDataLoader.GetPlayer().password == "1MNH8I9XC77ASSMNG!")
                    return;

                Plugin.instance.StartCoroutine(LoadPasswordScreen(value));
            }
            catch (NullReferenceException ex)
            {
                MTM101BaldiDevAPI.CauseCrash(Plugin.instance.Info, ex);
            }
        }

        public static IEnumerator LoadPasswordScreen(bool value)
        {
            yield return new WaitForSeconds(1f);

            var blackCanvas = UIHelpers.CreateBlankUIScreen("PassWordCanvas", true);
            UIHelpers.AddBordersToCanvas(blackCanvas);

            var text1 = UIHelpers.CreateText<TextMeshProUGUI>(BaldiFonts.ComicSans24, "\"Click here to paste the beta password\"", blackCanvas.transform, Vector3.zero, false);
            text1.rectTransform.SetAnchorAndPosition(TextAnchor.MiddleCenter);
            text1.alignment = TextAlignmentOptions.Center;
            text1.color = Color.black;
            text1.raycastTarget = true;
            text1.rectTransform.sizeDelta = new(480, 360);

            var button = text1.gameObject.ConvertToButton<StandardMenuButton>(true);
            button.underlineOnHigh = true;
            button.OnPress.AddListener(delegate () {

                if (GUIUtility.systemCopyBuffer.IsNullOrWhiteSpace())
                    return;

                button.text.text = GUIUtility.systemCopyBuffer;

                if (button.text.text == "1MNH8I9XC77ASSMNG!")
                {
                    button.text.color = Color.green;

                    var player = PlayerDataLoader.GetPlayer();
                    player.isBetaTester = true;
                    player.password = "1MNH8I9XC77ASSMNG!";
                    PlayerDataLoader.CreatePlayerData(player.playerName, player);

                    GameObject.FindObjectOfType<MainMenu>(true).gameObject.SetActive(true);
                    Singleton<GlobalCam>.Instance.Transition(UiTransition.Dither, 0.01666667f);
                    blackCanvas.gameObject.SetActive(false);
                }
                else
                {
                    button.text.color = Color.red;
                    Singleton<InputManager>.Instance.Rumble(3f, 0.1f);
                }
            });

            var cursor = blackCanvas.gameObject.AddComponent<CursorInitiator>();
            cursor.cursorPre = GameObject.Find("Menu").GetComponent<CursorInitiator>().cursorPre;
            cursor.graphicRaycaster = blackCanvas.GetComponent<GraphicRaycaster>();
            cursor.Inititate();
            cursor.currentCursor.transform.SetSiblingIndex(int.MaxValue);

            Singleton<GlobalCam>.Instance.Transition(UiTransition.Dither, 0.01666667f);
            GameObject.Find("Menu").SetActive(false);
        }
    }
}
