using UnityEditor;
using UnityEngine;

namespace src.backend
{
    [CreateAssetMenu(fileName = "BaseSO")]
    public class BaseSO : ScriptableObject
    {
        [SerializeField, HideInInspector]
        private uint uniqueID;
        public uint UniqueID => uniqueID;

#if UNITY_EDITOR
        [ContextMenu("Generate Unique ID")]
        private void GenerateUniqueID()
        {
            uniqueID = IDGenerator.GetEditorAssetID();
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            Debug.Log($"Generated ID {uniqueID} for {name}");
        }
#endif
        
    }
}
