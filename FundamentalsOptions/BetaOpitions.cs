using MTM101BaldAPI.OptionsAPI;
using MTM101BaldAPI.Reflection;
using nbbpfei_reworked.FundamentalsContent.RoomFunctions;
using nbbpfei_reworked.FundamentalsManagers;
using System.IO;
using System.Linq;
using UnityEngine;

namespace nbbpfei_reworked.FundamentalsOptions
{
    public class BetaOpitions : CustomOptionsCategory
    {
        public override void Build()
        {
            string output = File.ReadAllText(FilePathsManager.GetPath(FilePaths.Options, Singleton<PlayerFileManager>.Instance.fileName + "_FundamentalsOptionsData.optionData"));
            string decrypt = UnityCipher.RijndaelEncryption.Decrypt(output, "fundamentalsiscool!");
            data = JsonUtility.FromJson<OptionsData>(decrypt);

            var toggle = CreateToggle("OutlineRooms", "Outiline Rooms", data.outlineRooms, new(0, 50, 0), 200);
            AddTooltip(toggle, "<b><color=red>*UNDER DEVELOPMENT*</b></color>\nThis is an experimental generation of floors\nthat forces the halls to connect the rooms\nleaving the halls more open and avoiding\nhaving to change paths to one much further away.");

            CreateText("Section1", "-------[Content Disables]-------", new(0, 22), MTM101BaldAPI.UI.BaldiFonts.ComicSans24, TMPro.TextAlignmentOptions.Center, new(350, 0), Color.black);

            var disableToggle = CreateToggle("Disable1", "Desactive rooms textures", data.disableTextures, new(117, -5, 0), 400);
            AddTooltip(disableToggle, "When activated, disable the appearance of any customized texture\nfor tiles/cells in rooms and hallways\nleaving only boring and monotone textures");

            var disableToggle1 = CreateToggle("Disable2", "Desactive rooms variants ", data.disableRoomsVariants, new(111, -35, 0), 400);
            AddTooltip(disableToggle1, "When activated, custom room variants from the base game\n(such as class, faculty, etc.) will not appear\nonly all the boring and repetitive rooms from the standard game will appear.");

            void OnApply()
            {
                Singleton<MusicManager>.Instance.PlaySoundEffect(GenericStuff.xylophoneSoundGeneric);

                if (Singleton<CoreGameManager>.Instance != null)
                    return;

                data.outlineRooms = toggle.Value;

                foreach (RoomAsset outlineRoom in Resources.FindObjectsOfTypeAll<RoomAsset>().Where(x => x.roomFunctionContainer != null))
                {
                    var copyCastFunction = outlineRoom.roomFunctionContainer.gameObject.GetComponent<OutilineRoomFunction>();
                    copyCastFunction.active = toggle.Value;
                }

                string output = JsonUtility.ToJson(data);
                string encrypted = UnityCipher.RijndaelEncryption.Encrypt(output, "fundamentalsiscool!");
                File.WriteAllText(FilePathsManager.GetPath(FilePaths.Options, Singleton<PlayerFileManager>.Instance.fileName + "_FundamentalsOptionsData.optionData"), encrypted);
            }

            OnApply();

            var apply = CreateApplyButton(OnApply);


            if (Singleton<CoreGameManager>.Instance != null)
            {
                Disable(toggle, new(-21, 0), new(300, 25));
                Disable(disableToggle, new(-115, 0), new(350, 25));
                Disable(disableToggle1, new(-111, 0), new(350, 25));
            }
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

        public OptionsData data = new();
    }
}
