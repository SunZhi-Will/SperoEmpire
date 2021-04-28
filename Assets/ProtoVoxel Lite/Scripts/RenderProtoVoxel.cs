using System.Collections.Generic;
using UnityEngine;

namespace ProtoVoxelLite
{
    /// <summary>
    /// Get a ProtoVoxelData and render the object in the scene
    /// </summary>
    [SelectionBaseAttribute]
    public class RenderProtoVoxel : MonoBehaviour
    {
        private readonly string CHILD_NAME = "VoxelLayers";
        public ProtoVoxelData data;

        public Color color = new Color(1f, 1f, 1f, 1f);
        public bool renderOnChange = true;

        public bool modifiers = false;

        public float modScaleMult = 1;
        public float modOffsetXMult = 1;
        public float modOffsetYMult = 1;
        public float modBendXMult = 1;
        public float modBendYMult = 1;
        public float modTwistMult = 1;

        public AnimationCurve modScale;
        public AnimationCurve modOffsetX;
        public AnimationCurve modOffsetY;
        public AnimationCurve modBendX;
        public AnimationCurve modBendY;
        public AnimationCurve modTwist;

        private float maxHeight;

        [HideInInspector]
        public GameObject voxelLayerGO;

        [HideInInspector]
        public List<RenderLayerDataProtoVoxel> voxelLayerGOLayers;


        public int randomSeed = -1;


        void Reset()
        {
            modScale = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));

            modOffsetX = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 0));
            modOffsetY = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 0));

            modBendX = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 0));
            modBendY = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 0));
            modTwist = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 0));


        }

        public void NewSeed()
        {
            randomSeed = Random.Range(0, int.MaxValue);
        }

        /// <summary>
        /// Render the ProtoVoxel in the scene
        /// </summary>
        [ContextMenu("Render Voxel")]
        public void RenderVoxel()
        {
            if (data == null)
            {
                Debug.LogError("RenderProtoVoxel require a ProtoVoxelData to render", this);
                return;
            }

            if (voxelLayerGO != null)
            {
                DestroyImmediate(voxelLayerGO);
                voxelLayerGO = null;
            }


            if (data.procgen)
            {
                if (randomSeed == -1)
                    NewSeed();
            }
            else
            {
                randomSeed = -1;
            }

            voxelLayerGO = new GameObject(CHILD_NAME);
            voxelLayerGO.transform.parent = transform;
            voxelLayerGO.transform.localPosition = Vector3.zero;
            voxelLayerGO.transform.localRotation = Quaternion.identity;
            voxelLayerGO.transform.localScale = Vector3.one;


            voxelLayerGOLayers = new List<RenderLayerDataProtoVoxel>();

            if (data.procgen)
            {
                Random.InitState(randomSeed);
                GenerateProcGen();
            }

            maxHeight = 0;
            float stackedY = 0;

            Vector3 normal = new Vector3(1, 1, 1);
            Vector3 mirrorX = new Vector3(-1, 1, 1);
            Vector3 mirrorY = new Vector3(1, -1, 1);
            Vector3 mirrorZ = new Vector3(1, 1, -1);
            Vector3 mirrorXY = new Vector3(-1, -1, 1);
            Vector3 mirrorXZ = new Vector3(-1, 1, -1);
            Vector3 mirrorYZ = new Vector3(1, -1, -1);
            Vector3 mirrorXYZ = new Vector3(-1, -1, -1);



            for (int r = 0; r < data.layers.repeat; r++)
            {
                float height = (data.layers.stackHeight) ? stackedY : 0;
                stackedY = RenderLayer("Layer " + 0 + " " + r, data.layers,
                   height, normal);

                if (data.mirrorX)
                    RenderLayer("Layer " + 0 + " " + r, data.layers,
                  height, mirrorX);

                if (data.mirrorY)
                    RenderLayer("Layer " + 0 + " " + r, data.layers,
                  height, mirrorY);

                if (data.mirrorZ)
                    RenderLayer("Layer " + 0 + " " + r, data.layers,
                  height, mirrorZ);

                if (data.mirrorX && data.mirrorY)
                    RenderLayer("Layer " + 0 + " " + r, data.layers,
                 height, mirrorXY);

                if (data.mirrorX && data.mirrorZ)
                    RenderLayer("Layer " + 0 + " " + r, data.layers,
                 height, mirrorXZ);

                if (data.mirrorY && data.mirrorZ)
                    RenderLayer("Layer " + 0 + " " + r, data.layers,
                 height, mirrorYZ);

                if (data.mirrorX && data.mirrorY && data.mirrorZ)
                    RenderLayer("Layer " + 0 + " " + r, data.layers,
                 height, mirrorXYZ);


                if (stackedY > maxHeight)
                    maxHeight = stackedY;

            }


            if (modifiers)
                ApplyMods();
        }

        public void GenerateProcGen()
        {
            Color emptyColor = new Color(0, 0, 0, 0);
            Color randomColor = new Color(0, 0, 0, 0);

            bool seedGroup1 = Random.Range(0, 2) == 1;
            bool seedGroup2 = Random.Range(0, 2) == 1;
            bool seedGroup3 = Random.Range(0, 2) == 1;

            Color voxelColor = Random.ColorHSV();


            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    int index = (y * 9) + x;
                    ProtoLayerVoxelData layerData = data.layers.data[index];

                    Color coloredColor = Color.gray;


                    float force;
                    switch (data.procColorMode)
                    {
                        case ProtoVoxelData.ProcColorMode.random:
                            force = Random.Range(-data.procColorRange, data.procColorRange);
                            if (force < 0)
                                coloredColor = Color.Lerp(coloredColor, Color.white, -force);
                            else
                                coloredColor = Color.Lerp(coloredColor, Color.black, force);
                            break;

                    }

                    bool seedValue = true;

                    switch (layerData.procSeedGroup)
                    {
                        case 0:
                            seedValue = Random.Range(0, 2) == 1;
                            break;
                        case 1:
                            seedValue = seedGroup1;
                            break;
                        case 2:
                            seedValue = seedGroup2;
                            break;
                        case 3:
                            seedValue = seedGroup3;
                            break;
                    }

                    switch (layerData.procType)
                    {
                        case ProtoVoxelData.ProcTypes.empty:
                            data.layers.data[index].procColor = emptyColor;
                            data.layers.data[index].procSelType = ProtoVoxelData.ProcTypes.empty;
                            break;
                        case ProtoVoxelData.ProcTypes.border:
                            data.layers.data[index].procColor = data.procBorderColor;
                            data.layers.data[index].procSelType = ProtoVoxelData.ProcTypes.border;
                            break;
                        case ProtoVoxelData.ProcTypes.colored:
                            data.layers.data[index].procColor = coloredColor;
                            data.layers.data[index].procSelType = ProtoVoxelData.ProcTypes.colored;
                            break;
                        case ProtoVoxelData.ProcTypes.empty_border:
                            if (seedValue)
                            {
                                data.layers.data[index].procColor = data.procBorderColor;
                                data.layers.data[index].procSelType = ProtoVoxelData.ProcTypes.border;
                            }
                            else
                            {
                                data.layers.data[index].procColor = emptyColor;
                                data.layers.data[index].procSelType = ProtoVoxelData.ProcTypes.empty;
                            }
                            break;
                        case ProtoVoxelData.ProcTypes.empty_colored:
                            if (seedValue)
                            {
                                data.layers.data[index].procColor = coloredColor;
                                data.layers.data[index].procSelType = ProtoVoxelData.ProcTypes.colored;
                            }
                            else
                            {
                                data.layers.data[index].procColor = emptyColor;
                                data.layers.data[index].procSelType = ProtoVoxelData.ProcTypes.empty;
                            }
                            break;
                        case ProtoVoxelData.ProcTypes.colored_border:
                            if (seedValue)
                            {
                                data.layers.data[index].procColor = coloredColor;
                                data.layers.data[index].procSelType = ProtoVoxelData.ProcTypes.colored;
                            }
                            else
                            {
                                data.layers.data[index].procColor = data.procBorderColor;
                                data.layers.data[index].procSelType = ProtoVoxelData.ProcTypes.border;
                            }
                            break;
                    }
                }
            }


            if (!data.procUseBorder)
                return;




            //Calculate borders

            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    int layerData = (y * 9) + x;

                    if (data.layers.data[layerData].procSelType != ProtoVoxelData.ProcTypes.colored)
                        continue;

                    int top = ((y - 1) * 9) + x;
                    int bot = ((y + 1) * 9) + x;
                    int lef = ((y) * 9) + x - 1;
                    int rig = ((y) * 9) + x + 1;

                    if (y > 0 && IsBorderValid(top))
                    {
                        data.layers.data[top].procColor = data.procBorderColor;
                        data.layers.data[top].procSelType = ProtoVoxelData.ProcTypes.border;
                        //data.layers.data[top].isNotTransparent = true;
                    }
                    if (y < 9 - 1 && IsBorderValid(bot))
                    {
                        data.layers.data[bot].procColor = data.procBorderColor;
                        data.layers.data[bot].procSelType = ProtoVoxelData.ProcTypes.border;
                       // data.layers.data[bot].isNotTransparent = true;

                    }
                    if (x > 0 && IsBorderValid(lef))
                    {
                        data.layers.data[lef].procColor = data.procBorderColor;
                        data.layers.data[lef].procSelType = ProtoVoxelData.ProcTypes.border;
                       // data.layers.data[lef].isNotTransparent = true;
                    }
                    if (x < 9 - 1 && IsBorderValid(rig))
                    {
                        data.layers.data[rig].procColor = data.procBorderColor;
                        data.layers.data[rig].procSelType = ProtoVoxelData.ProcTypes.border;
                       // data.layers.data[rig].isNotTransparent = true;
                    }

                }
            }

        }

        public bool IsBorderValid(int pos)
        {
            ProtoLayerVoxelData layerData = data.layers.data[pos];
            if (layerData.procSelType != ProtoVoxelData.ProcTypes.empty)
                return false;

            return true;

        }

        public void ApplyMods()
        {
            if (voxelLayerGO == null || voxelLayerGOLayers == null)
                RenderVoxel();

            //float totalHeight = GetComponent<Renderer>().bounds.size.y;
            //float totalHeight = 0;



            //totalHeight != 0 ? (height + data.layers[i].height) / totalHeight : 0
            float stackedY = 0;
            for (int i = 0; i < voxelLayerGOLayers.Count; i++)
            {
                stackedY = ModLayer(
                    voxelLayerGOLayers[i], stackedY,
                    maxHeight != 0 ? voxelLayerGOLayers[i].offset / maxHeight : 0);
            }


        }

        public float ModLayer(RenderLayerDataProtoVoxel layerData, float height, float modPercentage)
        {
            layerData.layer.transform.localRotation = Quaternion.identity;


            float scaleMult = modScale.Evaluate(modPercentage) * modScaleMult;

            float innerY = 0;
            if (layerData.layer.transform.childCount > 0)
                innerY = layerData.layer.transform.GetChild(0).transform.localPosition.y;

            layerData.layer.transform.localScale = new Vector3(scaleMult, scaleMult, scaleMult);

            float diff = ((innerY - 0.5f) * (1 - scaleMult));
            layerData.layer.transform.localPosition = new Vector3(0, diff - height, 0);

            layerData.layer.transform.Translate(new Vector3(modOffsetX.Evaluate(modPercentage) * modOffsetXMult, 0, modOffsetY.Evaluate(modPercentage)) * modOffsetYMult);
            layerData.layer.transform.RotateAround(layerData.layer.transform.position, Vector3.up, modTwist.Evaluate(modPercentage) * modTwistMult);
            layerData.layer.transform.RotateAround(transform.position, Vector3.left, modBendX.Evaluate(modPercentage) * modBendXMult);
            layerData.layer.transform.RotateAround(transform.position, Vector3.forward, modBendY.Evaluate(modPercentage) * modBendYMult);

            return (1 - scaleMult) + height;
        }

        /// <summary>
        /// Render a single layer of the ProtoVoxel
        /// </summary>
        /// <param name="index">The Layer Index</param>
        /// <param name="layer">The data of the Layer</param>
        /// <param name="offsetY">How much to offset for the Y axis</param>
        /// <returns>New Y Offset</returns>
        protected float RenderLayer(string name, ProtoVoxelLayer layer, float offsetY, Vector3 mirror)
        {
            Vector3 center = new Vector3(5, 0, 5);

            GameObject layerObj = new GameObject(name);
            layerObj.transform.parent = voxelLayerGO.transform;
            layerObj.transform.localPosition = Vector3.zero;
            layerObj.transform.localScale = Vector3.one;
            layerObj.transform.localRotation = Quaternion.identity;

            voxelLayerGOLayers.Add(new RenderLayerDataProtoVoxel(layerObj, offsetY));



            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    ProtoLayerVoxelData layerData = layer.data[(y * 9) + x];



                    Color c = Color.gray;

                    if (data.procgen)
                    {
                        c = layerData.procColor;
                    }

                    if (layerData.procSelType == ProtoVoxelData.ProcTypes.empty)
                        continue;

                    if (!layerData.isNotTransparent && layerData.procSelType != ProtoVoxelData.ProcTypes.border)
                        continue;



                    GameObject go;

                    go = GameObject.CreatePrimitive(PrimitiveType.Cube);

                    go.transform.parent = layerObj.transform;
                    go.name = x + " " + y;
                    go.transform.localRotation = Quaternion.identity;
                    go.transform.localScale = new Vector3(1, layer.scale, 1);
                    go.transform.localPosition = new Vector3(x * mirror.x, (offsetY + layer.height + (layer.scale / 2)) * mirror.y, y * mirror.z) - center;

                    if (!data.generateColliders)
                    {
                        Collider coll = go.GetComponent<Collider>();
                        if (coll != null)
                        {
                            DestroyImmediate(coll);
                        }
                    }

                    if (layer.customMaterial != null)
                    {
                        go.GetComponentInChildren<Renderer>().sharedMaterial = layer.customMaterial;
                    }
                    else if (data.customMaterial != null)
                    {
                        go.GetComponentInChildren<Renderer>().sharedMaterial = data.customMaterial;
                    }


                    if (go.GetComponent<ProtoSetPropertyBlock>() == null)
                    {
                        go.AddComponent<ProtoSetPropertyBlock>();
                    }

                    ProtoSetPropertyBlock block = go.GetComponent<ProtoSetPropertyBlock>();

                    block.color = c;
                    block.multiplier = color;
                    block.UpdateBlock();

                }
            }



            return offsetY + layer.height + (layer.scale);
        }

    }
}
