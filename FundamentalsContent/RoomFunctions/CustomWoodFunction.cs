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
            Texture2D woodTex = WeightedSelection<Texture2D>.ControlledRandomSelection(data.woodTextures, GameObject.FindObjectOfType<LevelBuilder>().controlledRNG);

            foreach (RendererContainer renderer in room.objectObject.GetComponentsInChildren<RendererContainer>())
            {
                if (renderer.renderers[0].material.GetTexture("_MainTex") != null)
                {
                    if (renderer.renderers[0].material.GetTexture("_MainTex").name == "wood 1")
                        renderer.renderers[0].material.SetTexture("_MainTex", woodTex);
                }
            }
        }
    }
}
