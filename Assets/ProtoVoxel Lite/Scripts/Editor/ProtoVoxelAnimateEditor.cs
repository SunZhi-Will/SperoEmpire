using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ProtoVoxelLite
{
    [CustomEditor(typeof(ProtoVoxelAnimate))]
    public class ProtoVoxelAnimateEditor : Editor
    {

      

        void OnEnable()
        {
           
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}
