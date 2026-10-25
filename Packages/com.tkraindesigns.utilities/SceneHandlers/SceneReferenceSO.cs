using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TkrainDesigns.SceneHandlers
{
    [CreateAssetMenu(fileName = "New Scene Reference", menuName = "Scene Reference SO", order = 0)]
    public class SceneReferenceSO : ScriptableObject
    {
        #if UNITY_EDITOR
        [SerializeField] SceneAsset scene;
        #endif
        [field: SerializeField] public string SceneName { get; private set; }

        #if UNITY_EDITOR
        public void OnValidate()
        {
            if(scene)
            {
                SceneName = scene.name;
            }
        }
        #endif
    }
}
