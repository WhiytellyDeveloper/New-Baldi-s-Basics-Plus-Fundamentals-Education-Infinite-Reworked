using nbbpfei_reworked.FundamentalsManagers;
using UnityEngine;

namespace nbbpfei_reworked.FundamentalsContent.RoomFunctions
{
    internal class CustomWoodFunction : RoomFunction
    {
        public override void OnGenerationFinished()
        {
            base.OnGenerationFinished();

            if (FindObjectOfType<LevelBuilder>() as LevelLoader)
                return;

            var data = FundamentalsMainLoader.GetRandomFloorByName(Singleton<CoreGameManager>.Instance.sceneObject.levelTitle);
            System.Random rng = new(Singleton<CoreGameManager>.Instance.Seed());
            Texture2D woodTex = WeightedSelection<Texture2D>.ControlledRandomSelection(data.woodTextures, rng);

            foreach (RendererContainer renderer in room.objectObject.GetComponentsInChildren<RendererContainer>())
            {
                foreach (Renderer _renderer in renderer.renderers)
                if (_renderer.material.GetTexture("_MainTex") != null)
                {
                    if (_renderer.material.GetTexture("_MainTex").name == "wood 1")
                        _renderer.material.SetTexture("_MainTex", woodTex);
                }
            }
        }
    }
}
