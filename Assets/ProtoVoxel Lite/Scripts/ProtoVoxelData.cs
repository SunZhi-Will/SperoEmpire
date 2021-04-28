using System.Collections.Generic;
using UnityEngine;

namespace ProtoVoxelLite
{
    /// <summary>
    /// An ProtoVoxel Data Object, to be used as base to render objects in the scene.
    /// </summary>
    [CreateAssetMenu(fileName = "ProtoVoxelData", menuName = "ProtoVoxelData")]
    public class ProtoVoxelData : ScriptableObject
    {
        /// <summary>
        /// Used in the editor, to select the type of render.
        /// </summary>
        public enum EditorBackdrop
        {
            none, lowerLayer, upperLayer
        }
        /// <summary>
        /// Types of avaiable brushes for the ProtoVoxel
        /// </summary>
        public enum BrushShapes
        {
            cube, FullVerOnly
        }

        public enum BrushType
        {
            voxel, procgen
        }


        public enum ProcTypes
        {
            empty, border, colored, empty_colored, colored_border, empty_border
        }

        public enum ProcColorMode
        {
            random, FullVerOnly
        }

        public enum ProcColorBase
        {
            original, FullVerOnly
        }


        public bool updateSceneObjects = true;
        public bool generateColliders = false;
        public ProtoVoxelLayer layers;
        public EditorBackdrop backdrop;
        public Material customMaterial;

        public int currLayer = 0;

        public BrushType currBrushType = BrushType.voxel;
        public Color currBrushColor = Color.white;
        public BrushShapes currBrushShape;
        public int currBrushSize = 1;
        public GameObject currBrushPrefab;
        public ProcTypes selectedProcType = ProcTypes.colored;
        public int currProcSeedGroup = 0;

        public bool mirrorX = false;
        public bool mirrorY = false;
        public bool mirrorZ = false;

        public bool procgen = true;

        public bool procUseBorder = true;
        public Color procBorderColor = Color.black;
        public float procColorRange = 0f;
        public ProcColorBase procRandomColorBase = ProcColorBase.original;
        public ProcColorMode procColorMode = ProcColorMode.random;


        /// <summary>
        /// Do the setup for all layers
        /// </summary>
        public void SetLayers()
        {
            if (layers == null)
                layers = new ProtoVoxelLayer();


        }
        /// <summary>
        /// Reset all the layer data
        /// </summary>
        public void ResetAll()
        {
            if (layers != null)
                layers.data = null;

            SetResolutionAll();
        }

        /// <summary>
        /// Set the resolution for all layers
        /// </summary>
        public void SetResolutionAll()
        {

                if (layers.data == null)
                    SetResolution();
            
        }

        public void SetResolution()
        {

            layers.data = new ProtoLayerVoxelData[81];

            for (int i = 0; i < layers.data.Length; i++)
            {
                layers.data[i] = new ProtoLayerVoxelData();
                //layers.data[i].procType = ProcTypes.empty;
                layers.data[i].procSeedGroup = 0;
                layers.data[i].procColor = Color.green;
                layers.data[i].isNotTransparent = true;
            }

        }

        /// <summary>
        /// Update all the GameObjects in the scene that use this ProtoVoxel data.
        /// </summary>
        public void UpdateObjectsInScene()
        {
            RenderProtoVoxel[] list = GameObject.FindObjectsOfType<RenderProtoVoxel>();

            for (int i = 0; i < list.Length; i++)
            {
                if (list[i].data == this && list[i].renderOnChange)
                {
                    list[i].RenderVoxel();
                }
            }
        }

    }
}
