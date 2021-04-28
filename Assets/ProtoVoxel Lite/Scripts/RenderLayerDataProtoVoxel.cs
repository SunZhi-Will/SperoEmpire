using UnityEngine;

namespace ProtoVoxelLite
{
    public struct RenderLayerDataProtoVoxel
    {
        public GameObject layer;
        public float offset;
        public RenderLayerDataProtoVoxel(GameObject layer, float offset)
        {

            this.layer = layer;
            this.offset = offset;
        }
    }
}
