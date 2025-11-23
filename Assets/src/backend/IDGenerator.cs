using UnityEngine;

namespace src.backend
{
    public class IDGenerator
    {
        private static uint runtimeAssetsID = 1; 
        private static uint editorAssetsID = 1;

        public static uint GetRuntimeAssetID()
        {
            if (runtimeAssetsID == uint.MaxValue)
            {
                runtimeAssetsID = 1;
                Debug.LogError("Runtime Asset ID overflow. Resetting to 1.");
            }

            return runtimeAssetsID++;
        }
        
         

        public static uint GetEditorAssetID()
        {
            return editorAssetsID++;
        }
    }
}
