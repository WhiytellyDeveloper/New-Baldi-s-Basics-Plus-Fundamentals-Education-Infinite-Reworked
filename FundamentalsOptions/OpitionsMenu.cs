using MTM101BaldAPI.OptionsAPI;
using MTM101BaldAPI.Reflection;
using System.Linq;
using UnityEngine;
using nbbpfei_reworked.FundamentalsPlayerData;
using System.Collections.Generic;

namespace nbbpfei_reworked.FundamentalsOptions
{
    public class OpitionsMenu : CustomOptionsCategory
    {
        public override void Build()
        {
            var data = PlayerDataLoader.GetPlayer();

            var toggle = CreateToggle("OutlineRooms", "Outiline Rooms", data.outlineRooms, new(0, 50, 0), 200);
            AddTooltip(toggle, "<b><color=red>*UNDER DEVELOPMENT*</b></color>\nThis is an experimental generation of floors\nthat forces the halls to connect the rooms\nleaving the halls more open and avoiding\nhaving to change paths to one much further away.");
            toggles.Add(toggle);

            var toggle1 = CreateToggle("Debug", "Debug Mode", data.debugMode, new(-28, 22, 0), 200);
            AddTooltip(toggle1, "Debug Mode contains:\n- Fastest player\n- Skip Baldi Hide-And-Seek Countdown\n- Always full map\n- The possibility of skipping any floor\n- Field Trip AutoPlay\n- And more things coming soon");
            toggles.Add(toggle1);

            var toggle2 = CreateToggle("FreeCam", "Free Player Camera", data.freeCamera, new(50, -6, 0), 300);
            AddTooltip(toggle2, "The player's camera will have the incredible possibility\nof seeing directly above and below! (ceiling and floor)\ninstead of looking at the faces of ugly\ncreatures at baldi's school");
            toggles.Add(toggle2);

            var toggle3 = CreateToggle("Mestizo", "Mestizo Textures", data.mestizoColors, new(31, -34, 0), 300);
            AddTooltip(toggle3, "Every room with the possibility of multiple textures will\nhave different textures from each other, this includes faculty, office and class\nand others that have more than one possible texture.");
            toggles.Add(toggle3);

            var toggle4 = CreateToggle("RandomizedDeath", "Parallel Universe", data.universeParallel, new(22, -62, 0), 300);
            AddTooltip(toggle4, "Every time you change floors (example: F1 -> PIT)\nthe seed will change to a random one, making the game more fun\nbecause you never know what to expect");
            toggles.Add(toggle4);

            CreateText("Ops", "MORE OPTIONS COMING SOON", new(0, -111), MTM101BaldAPI.UI.BaldiFonts.ComicSans18, TMPro.TextAlignmentOptions.Center, new(350, 0), Color.black);

            /*
            var disableToggle = CreateToggle("Disable1", "Desactive rooms textures", data.disableTextures, new(117, -65, 0), 400);
            AddTooltip(disableToggle, "When activated, disable the appearance of any customized texture\nfor tiles/cells in rooms and hallways\nleaving only boring and monotone textures");

            var disableToggle1 = CreateToggle("Disable2", "Desactive rooms variants ", data.disableRoomsVariants, new(111, -95, 0), 400);
            AddTooltip(disableToggle1, "When activated, custom room variants from the base game\n(such as class, faculty, etc.) will not appear\nonly all the boring and repetitive rooms from the standard game will appear");

            var disableToggle2 = CreateToggle("Disable2", "Desactive custom musics", data.disableMusics, new(105, -125, 0), 400);
            AddTooltip(disableToggle2, "When activated, this removes all the music at the beginning of floors and escape\nplaces that don't have music yet will play the customized songs in the same way.");
            */

            if (Singleton<CoreGameManager>.Instance != null)
            {
                Disable(toggle, new(-21, 54), new(300, 36));
                Disable(toggle1, new(3, 19), new(300, 50));
                Disable(toggle2, new(-25, -13), new(300, 50));
                Disable(toggle3, new(-54, -8), new(300, 36));
                Disable(toggle4, new(-27, 0), new(300, 36));
                //Disable(disableToggle, new(-115, 0), new(350, 25));
                // Disable(disableToggle1, new(-111, 0), new(350, 25));
                //Disable(disableToggle2, new(-105, 0), new(350, 25));
            }
        }

        private void Update()
        {
            try
            {
                var player = PlayerDataLoader.GetPlayer();

                if (toggles[0].Value != player.outlineRooms || toggles[1].Value != player.debugMode || toggles[2].Value != player.freeCamera || toggles[3].Value != player.mestizoColors && toggles[4].Value != player.universeParallel)
                {
                    player.outlineRooms = toggles[0].Value;
                    player.debugMode = toggles[1].Value;
                    player.freeCamera = toggles[2].Value;
                    player.mestizoColors = toggles[3].Value;
                    player.universeParallel = toggles[4].Value;
                    PlayerDataLoader.CreatePlayerData(player.playerName, player);
                }
            }
            catch { }
        }


        public static void Disable(MenuToggle tog, Vector3 pos, Vector2 size)
        {
            GameObject gameObject = Instantiate<GameObject>(Resources.FindObjectsOfTypeAll<GameObject>().First(x => x.name == "DisabledCover"));
            tog.ReflectionSetVariable("disableCover", gameObject);
            gameObject.transform.SetParent(tog.transform, false);
            gameObject.GetComponent<RectTransform>().sizeDelta = size;
            gameObject.transform.localPosition = pos;
            tog.Disable(true);
        }

        public List<MenuToggle> toggles = new();
    }
}
