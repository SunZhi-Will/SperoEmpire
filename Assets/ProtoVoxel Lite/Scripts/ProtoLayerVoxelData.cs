
using UnityEngine;

namespace ProtoVoxelLite
{
    /// <summary>
    /// Data for each "pixel" in a layer
    /// </summary>
    [System.Serializable]
    public struct ProtoLayerVoxelData
    {
        /// <summary>
        /// Only if a custom instace is used, otherwise is null
        /// </summary>
        public GameObject instance;

        public bool isNotTransparent;
        public ProtoVoxelData.ProcTypes procType;
        public Color procColor;
        public ProtoVoxelData.ProcTypes procSelType;
        public int procSeedGroup;
    }

}
