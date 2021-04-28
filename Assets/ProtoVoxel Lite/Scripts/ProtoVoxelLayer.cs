using UnityEngine;

namespace ProtoVoxelLite
{
    /// <summary>
    /// Data for a single layer of the ProtoVoxel
    /// </summary>
    [System.Serializable]
    public class ProtoVoxelLayer
    {
        /// <summary>
        /// Data for each "pixel" of this layer
        /// </summary>
        public ProtoLayerVoxelData[] data;
        public float scale = 1f;
        public float height = 0f;
        public int repeat = 1;
        public bool stackHeight = true;
        public Material customMaterial;
    }
}
