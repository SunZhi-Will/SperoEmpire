

using UnityEditor;
using UnityEngine;

namespace ProtoVoxelLite
{
   
    [CustomEditor(typeof(ProtoVoxelData))]
    public class ProtoVoxelDataEditor:Editor
    {

        ProtoVoxelData myTarget;
        Material mat;
        Vector2 mousePos;
        Texture2D tex ;


        void Awake()
        {
            tex = new Texture2D(1, 1);
            myTarget = target as ProtoVoxelData;
            var shader = Shader.Find("Hidden/Internal-Colored");
            mat = new Material(shader);

            myTarget.SetLayers();
            myTarget.SetResolutionAll();
        }

      

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Voxel data", EditorStyles.boldLabel);
            EditorGUI.BeginChangeCheck();

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.DelayedIntField(new GUIContent("Res X (FullVer Only)", "Resolution in the X axis of the paint canvas"), 9);
            EditorGUILayout.DelayedIntField(new GUIContent("Res Y (FullVer Only)", "Resolution in the Y axis of the paint canvas"), 9);
            GUILayout.ExpandWidth(true);
            EditorGUI.EndDisabledGroup();


            EditorGUI.BeginChangeCheck();

            myTarget.mirrorX = EditorGUILayout.Toggle(new GUIContent("Mirror X", "Mirror de voxel in the X axis"), myTarget.mirrorX);
            myTarget.mirrorY = EditorGUILayout.Toggle(new GUIContent("Mirror Y", "Mirror de voxel in the X axis"), myTarget.mirrorY);
            myTarget.mirrorZ = EditorGUILayout.Toggle(new GUIContent("Mirror Z", "Mirror de voxel in the X axis"), myTarget.mirrorZ);



            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(myTarget, "Mirror changed");
                if (myTarget.updateSceneObjects)
                {
                    myTarget.UpdateObjectsInScene();
                }
                EditorUtility.SetDirty(target);
            }

            EditorGUI.BeginDisabledGroup(true);
            EditorGUI.BeginChangeCheck();
            
            EditorGUILayout.DelayedIntField(new GUIContent("Total Layers (FullVer Only)", "How much layers does this ProtoVoxel have in total."), 1);
           

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.ObjectField(new GUIContent("Custom material", "A custom material to be applied to all elements in the generated model. If left null the model will keep its original material."), null, typeof(Material), false);
            EditorGUI.EndDisabledGroup();
            myTarget.generateColliders = EditorGUILayout.Toggle(new GUIContent("Keep Collider", "If on, the model will keep the colliders if there is any, otherwise will remove all colliders."), myTarget.generateColliders);
            myTarget.updateSceneObjects = EditorGUILayout.Toggle(new GUIContent("Update Objects In Real Time", "Make any change to this script recalculate all objects in scene."), myTarget.updateSceneObjects);

            if (!myTarget.updateSceneObjects)
            {
                if (GUILayout.Button(new GUIContent("Update Meshes in scene", "Update manually all the objects in the scene."))){
                    myTarget.UpdateObjectsInScene();
                }
            }
            

            EditorGUILayout.LabelField("Layer data", EditorStyles.boldLabel);

            myTarget.layers.scale = EditorGUILayout.Slider(new GUIContent("Layer Y Scale", "The size of this layer in the Y axis."), myTarget.layers.scale, 0.05f, 10f);
            myTarget.layers.height = EditorGUILayout.Slider(new GUIContent("Layer Y Offset", "How high or low this layer is from the offset."), myTarget.layers.height, -10f,  10f);
            myTarget.layers.repeat = EditorGUILayout.IntSlider(new GUIContent("Layer Repeat", "Repeat this layer X times in the render."), myTarget.layers.repeat, 1, 10);
            myTarget.layers.stackHeight = EditorGUILayout.Toggle(new GUIContent("Layer Local Y", "Set the Y value relative to the last layer position."), myTarget.layers.stackHeight);

            EditorGUILayout.LabelField("Procedural Generation", EditorStyles.boldLabel);

 
            myTarget.procUseBorder = EditorGUILayout.Toggle(new GUIContent("Generate Exterior Border", "Render a border outside the mesh."), myTarget.procUseBorder);
            myTarget.procBorderColor = EditorGUILayout.ColorField(new GUIContent("Border Color", "Color of the borders of the generated voxel."), myTarget.procBorderColor);
            myTarget.procRandomColorBase = (ProtoVoxelData.ProcColorBase)EditorGUILayout.EnumPopup(new GUIContent("Color Mode", "How the voxel will be colored."), myTarget.procRandomColorBase);
            myTarget.procColorRange = EditorGUILayout.Slider(new GUIContent("Color Range", "Assigned colors can vary by this amount."), myTarget.procColorRange, 0f, 1f);
            myTarget.procColorMode = (ProtoVoxelData.ProcColorMode)EditorGUILayout.EnumPopup(new GUIContent("Color Mode", "How the shape will be colored."), myTarget.procColorMode);

            
            EditorGUILayout.LabelField("Drawing Data", EditorStyles.boldLabel);

            bool isBrushVoxel = myTarget.currBrushType == ProtoVoxelData.BrushType.voxel;


            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginDisabledGroup(true);
            GUILayout.Toggle(false, "Voxel Brush (FullVer Only)", "Button");
            EditorGUI.EndDisabledGroup();
            GUILayout.Toggle(true, "ProcGen Brush", "Button");
            EditorGUILayout.EndHorizontal();

            myTarget.currBrushType = ProtoVoxelData.BrushType.procgen;



                myTarget.selectedProcType = (ProtoVoxelData.ProcTypes)EditorGUILayout.EnumPopup(new GUIContent("Pixel Type", "Show drawing in the background of the canvas."), myTarget.selectedProcType);
                EditorGUILayout.LabelField(new GUIContent("Pixel Seed Group:", "Groups will always share the same seed values."), EditorStyles.label);
                EditorGUILayout.BeginHorizontal();

                bool group0 = GUILayout.Toggle(myTarget.currProcSeedGroup == 0, "No Seed", "Button");
                if (group0)
                    myTarget.currProcSeedGroup = 0;

                bool group1 = GUILayout.Toggle(myTarget.currProcSeedGroup == 1, "Seed +", "Button");
                if (group1)
                    myTarget.currProcSeedGroup = 1;

                bool group2 = GUILayout.Toggle(myTarget.currProcSeedGroup == 2, "Seed ▲", "Button");
                if (group2)
                    myTarget.currProcSeedGroup = 2;
                bool group3 = GUILayout.Toggle(myTarget.currProcSeedGroup == 3, "Seed ●", "Button");
                if(group3)
                    myTarget.currProcSeedGroup = 3;

                EditorGUILayout.EndHorizontal();
            

            
            myTarget.currBrushSize = EditorGUILayout.IntSlider(new GUIContent("Brush Size", "Size of the brush to be used in the canvas."), myTarget.currBrushSize, 1, 15);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(myTarget, "Data changed");
                if (myTarget.updateSceneObjects)
                {
                    myTarget.UpdateObjectsInScene();
                }
                EditorUtility.SetDirty(target);
            }


            

            float width = Screen.width * 0.90f;
            float height =9 / 9 * width;
            float scale = width / 9;


            bool canPaint = true;

         
            GUILayout.Box(tex, GUILayout.Width(width), GUILayout.Height(height));
            Rect boxRect = GUILayoutUtility.GetLastRect();


            float pixelWidth =9 / width;
            float pixelHeight =9 / height;

            float pwidth = 1 / pixelWidth;
            float pheight = 1 / pixelHeight;

            Color cursorSelection = new Color(0, 0, 0, 0.25f);

           
            
            if (canPaint && boxRect.Contains(Event.current.mousePosition))
            {
               
                mousePos = Event.current.mousePosition;

                //If mouse is down or dragging, paint the colors in the canvas
                if (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseDrag)
                {
                    Repaint();
                    Vector2 snapped = SnapToGrid(mousePos - boxRect.position, width, pixelWidth, height, pixelHeight, scale);
                    
                    snapped /= scale;
                    Undo.RecordObject(myTarget, "Painted");

                    Color selColor;
                    ProtoVoxelData.ProcTypes procType;

                    ProtoVoxelData.BrushShapes shape;
                    GameObject prefabInstance = null;

                    //Paint with mouse 0, remove paint with other buttons
                    if(Event.current.button == 0)
                    {
                        selColor = myTarget.currBrushColor;
                        procType = myTarget.selectedProcType;
                        shape = myTarget.currBrushShape;
                    }
                    else
                    {
                        selColor = new Color(0, 0, 0, 0);
                        procType = ProtoVoxelData.ProcTypes.empty;
                        shape = ProtoVoxelData.BrushShapes.cube;
                    }

                    
                    //Set the colors to the canvas
                    for (int mX = 0; mX < myTarget.currBrushSize; mX++)
                    {
                        for (int mY = 0; mY < myTarget.currBrushSize; mY++)
                        {
                            int dataIndex = ((Mathf.RoundToInt(snapped.y) + mY) *9) + Mathf.RoundToInt(snapped.x) + mX;

                            if (myTarget.currBrushType == ProtoVoxelData.BrushType.voxel)
                            {
                                myTarget.layers.data[dataIndex].instance = prefabInstance;
                               
                                if (myTarget.layers.data[dataIndex].procType == ProtoVoxelData.ProcTypes.empty)
                                    myTarget.layers.data[dataIndex].procType = ProtoVoxelData.ProcTypes.colored;
                                myTarget.layers.data[dataIndex].isNotTransparent = selColor.a != 0;
                                if(!myTarget.layers.data[dataIndex].isNotTransparent)
                                    myTarget.layers.data[dataIndex].procType = ProtoVoxelData.ProcTypes.empty;
                            }
                            else if(myTarget.currBrushType == ProtoVoxelData.BrushType.procgen)
                            {
                                myTarget.layers.data[dataIndex].procType = procType;
                                myTarget.layers.data[dataIndex].procSeedGroup = myTarget.currProcSeedGroup;

                            }


                        }
                    }

                   
                    Undo.RecordObject(myTarget, "Data changed");
                    if (myTarget.updateSceneObjects)
                    {
                        myTarget.UpdateObjectsInScene();
                    }
                    EditorUtility.SetDirty(target);
                    
                }

            }
            else
            {
                mousePos = Vector2.zero;
            }

            Repaint();


            if (Event.current.type == EventType.Repaint)
            {
                GUI.BeginClip(boxRect);
                GL.PushMatrix();
                mat.SetPass(0);
                GL.Clear(true, false, Color.black);
                GL.Begin(GL.QUADS);

                //Paint the colors in the canvas
                for (int x = 0; x < 9; x++)
                {
                    for (int y = 0; y < 9; y++)
                    {
                        float px = x / pixelWidth;
                        float py = y / pixelHeight;
                        

                        int colorIndex = (y *9) + x;
                        RenderPaint(myTarget.currLayer, colorIndex, 1f, px, py, pwidth, pheight);
                    }
                }

                //Paint the mouse selection area in the canvas
                if (mousePos != Vector2.zero)
                {
                    Vector3 pos = mousePos - boxRect.position;
                    Vector2 snapped = SnapToGrid(pos, width, pixelWidth, height, pixelHeight, scale);

                    for(int mX = 0; mX < myTarget.currBrushSize; mX++)
                    {
                        for (int mY = 0; mY < myTarget.currBrushSize; mY++)
                        {
                            if(mX <9 && mY < 9)
                                DrawPixel(snapped.x + (mX * pwidth), snapped.y + (mY * pheight), pwidth, pheight, cursorSelection);
                        }
                    } 

                    
                }

                GL.End();
                GL.PopMatrix();

                GUI.EndClip();
            }
        }


        public void RenderPaint(int layerIndex, int dataIndex, float alpha, float px, float py, float pwidth, float pheight)
        {
            Color c = Color.gray;

            if (myTarget.currBrushType == ProtoVoxelData.BrushType.voxel)
            {
                if (myTarget.layers.data[dataIndex].isNotTransparent)
                {
                    DrawPixel(px, py, pwidth, pheight, c * new Color(1, 1, 1, alpha));
                }
            }
            if (myTarget.currBrushType == ProtoVoxelData.BrushType.procgen)
            {
                switch (myTarget.layers.data[dataIndex].procType)
                {
                    case ProtoVoxelData.ProcTypes.empty:
                        c = Color.white;
                        break;
                    case ProtoVoxelData.ProcTypes.border:
                        c = Color.black;
                        break;
                    case ProtoVoxelData.ProcTypes.colored:
                        c = Color.green;
                        break;
                    case ProtoVoxelData.ProcTypes.colored_border:
                        c = new Color(0, 0.5f, 0f, 1);
                        break;
                    case ProtoVoxelData.ProcTypes.empty_border:
                        c = Color.grey;
                        break;
                    case ProtoVoxelData.ProcTypes.empty_colored:
                        c = new Color(0.5f, 1f, 0.5f, 1); ;
                        break;
                }

                switch (myTarget.layers.data[dataIndex].procSeedGroup) {
                    case 0:
                        DrawPixel(px, py, pwidth, pheight, c * new Color(1, 1, 1, alpha));
                        break;
                    case 1:
                        DrawPrefab(px, py, pwidth, pheight, c * new Color(1, 1, 1, alpha));
                        break;
                    case 2:
                        DrawTriangle(px, py, pwidth, pheight, c * new Color(1, 1, 1, alpha));
                        break;
                    case 3:
                        DrawCircle(px, py, pwidth, pheight, c * new Color(1, 1, 1, alpha));
                        break;
                }
            }
        }




        public Vector2 SnapToGrid(Vector2 pos, float width, float pixelWidth, float height, float pixelHeight, float scale)
        {
            return new Vector2(
                scale * Mathf.Round(((pos.x / width * 9 / (pixelWidth)) - (0.5f / pixelWidth)) / scale),
                scale * Mathf.Round(((pos.y / height * 9 / pixelHeight) - 0.5f / pixelHeight) / scale)
            );
        }
      

        public void DrawLine(Vector2 from, Vector2 to)
        {
            GL.Begin(GL.LINES);
            GL.Color(Color.black);
            GL.Vertex3(from.x, from.y, 0);
            GL.Vertex3(to.x, to.y, 0);
            GL.End();
        }

        public void DrawPixel(float x, float y, float w, float h, Color color)
        {
           
            GL.Color(color);
            GL.Vertex3(x, y, 0);
            GL.Vertex3(x + w, y, 0);
            GL.Vertex3(x + w, y + h, 0);
            GL.Vertex3(x, y + h, 0);
            
        }

        public void DrawTriangle(float x, float y, float w, float h, Color color)
        {

            GL.Color(color);
            GL.Vertex3(x + (w * 0.5f), y, 0);
            GL.Vertex3(x + w, y + h, 0);
            GL.Vertex3(x, y + h, 0);
            GL.Vertex3(x + (w * 0.5f), y, 0);
        }

        public void DrawCircle(float x, float y, float w, float h, Color color)
        {
              
            GL.Color(color);

            GL.Vertex3(x + (w * 0.5f), y, 0);
            GL.Vertex3(x + w, y + (h * 0.5f), 0);
            GL.Vertex3(x + (w * 0.5f), y + h, 0);
            GL.Vertex3(x, y+(h * 0.5f), 0);

        }

        public void DrawPrefab(float x, float y, float w, float h, Color color)
        {

            GL.Color(color);


            GL.Vertex3(x + (w * 0.4f), y, 0);
            GL.Vertex3(x + (w * 0.6f), y, 0);
            GL.Vertex3(x + (w * 0.6f), y + h, 0);
            GL.Vertex3(x + (w * 0.4f), y + h, 0);

            GL.Vertex3(x , y + (h * 0.4f), 0);
            GL.Vertex3(x + w, y + (h * 0.4f), 0);
            GL.Vertex3(x + w , y +(h * 0.6f), 0);
            GL.Vertex3(x, y + (h * 0.6f), 0);

        }

    }
}
