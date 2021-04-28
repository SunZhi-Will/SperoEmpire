using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ProtoVoxelLite
{
    /// <summary>
    /// Save and configure the Property block of a GameObject
    /// </summary>
    [ExecuteInEditMode]
    public class ProtoSetPropertyBlock:MonoBehaviour
    {
        public Color color;
        public Color multiplier;
        void OnEnable()
        {
            UpdateBlock();
        }

        public void UpdateBlock()
        {
            Renderer render = GetComponentInChildren<Renderer>();
            MaterialPropertyBlock block = new MaterialPropertyBlock();

            block.SetColor("_Color", color * multiplier);
            render.SetPropertyBlock(block);
        }

    }
}
