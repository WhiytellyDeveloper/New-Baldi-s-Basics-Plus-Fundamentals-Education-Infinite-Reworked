using MTM101BaldAPI;
using MTM101BaldAPI.AssetTools;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

namespace nbbpfei_reworked.FundamentalsManagers
{
    public static class AssetsHelper
    {
        public static AssetManager assetMan = new();

        public static Texture2D LoadTexture(string fileName, string referencialName = "", params string[] paths)
        {
            string name = string.IsNullOrEmpty(referencialName) ? fileName : referencialName;

            if (assetMan.ContainsKey(name))
                return assetMan.Get<Texture2D>(name);

            var texture = AssetLoader.TextureFromMod(Plugin.instance, Path.Combine(Path.Combine(paths), fileName + ".png"));
            texture.name = name;

            assetMan.Add(name, texture);
            return texture;
        }

        public static Sprite LoadSprite(string fileName, string referencialName = "", int pixelPerUnit = 1, params string[] paths)
        {
            string name = string.IsNullOrEmpty(referencialName) ? fileName : referencialName;

            if (assetMan.ContainsKey(name))
                return assetMan.Get<Sprite>(name);

            var sprite = AssetLoader.SpriteFromMod(Plugin.instance, Vector2.one / 2, pixelPerUnit, Path.Combine(Path.Combine(paths), fileName + ".png"));
            sprite.name = name;

            assetMan.Add(name, sprite);
            return sprite;
        }

        public static Texture2D[] LoadTextures(params string[] paths)
        {
            var textures = AssetLoader.TexturesFromMod(Plugin.instance, "*.png", Path.Combine(Path.Combine(paths)));
            return textures;
        }

        public static Sprite[] LoadSprite(int pixelPerUnit, params string[] paths)
        {
            var textures = AssetLoader.TexturesFromMod(Plugin.instance, "*.png", Path.Combine(Path.Combine(paths)));
            return textures.ToSprites(pixelPerUnit);
        }

        public static SoundObject LoadSound(string fileName, string referencialName = "", SoundType type = SoundType.Effect, Color color = new(), string subtitle = "", params string[] paths)
        {
            string name = string.IsNullOrEmpty(referencialName) ? fileName : referencialName;

            if (assetMan.ContainsKey(name))
                return assetMan.Get<SoundObject>(name);

            Debug.Log(Path.Combine(AssetLoader.GetModPath(Plugin.instance), Path.Combine(paths), fileName + ".wav"));
            var soundObject = ObjectCreators.CreateSoundObject(AssetLoader.AudioClipFromMod(Plugin.instance, Path.Combine(Path.Combine(paths), fileName + ".wav")), subtitle, SoundType.Effect, color);
            soundObject.name = name;
            soundObject.subtitle = !string.IsNullOrEmpty(subtitle);

            assetMan.Add<SoundObject>(name, soundObject);

            return soundObject;
        }

        public static string LoadMidiFile(string fileName, string referncialName, params string[] paths)
        {
            string midiFile = AssetLoader.MidiFromMod(referncialName, Plugin.instance, Path.Combine(Path.Combine(paths), fileName + ".midi"));
            return midiFile;
        }

        public static T Get<T>(string name) { return assetMan.Get<T>(name); } 
    }
}
