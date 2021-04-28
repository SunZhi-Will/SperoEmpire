using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProtoVoxelLite
{
    /// <summary>
    /// Animate a ProtoVoxel element
    /// </summary>
    [RequireComponent(typeof(RenderProtoVoxel))]
    public class ProtoVoxelAnimate : MonoBehaviour
    {
        [Range(-1f, 1f)]
        public float scaleSpeed = 0;
        [Range(-1f, 1f)]
        public float offsetXSpeed = 0;
        [Range(-1f, 1f)]
        public float offsetYSpeed = 0;
        [Range(-1f, 1f)]
        public float bendXSpeed = 0;
        [Range(-1f, 1f)]
        public float bendYSpeed = 0;
        [Range(-1f, 1f)]
        public float twistSpeed = 0;

        private RenderProtoVoxel render;

        private void Awake()
        {
            render = GetComponent<RenderProtoVoxel>();
            render.modScale.preWrapMode     = render.modScale.postWrapMode      = WrapMode.Loop;
            render.modOffsetX.preWrapMode   = render.modOffsetX.postWrapMode    = WrapMode.Loop;
            render.modOffsetY.preWrapMode   = render.modOffsetY.postWrapMode    = WrapMode.Loop;
            render.modBendX.preWrapMode     = render.modBendX.postWrapMode      = WrapMode.Loop;
            render.modBendY.preWrapMode     = render.modBendY.postWrapMode      = WrapMode.Loop;
            render.modTwist.preWrapMode     = render.modTwist.postWrapMode      = WrapMode.Loop;
        } 
        private void Update()
        {
            AnimationCurve curve;

            curve = render.modScale;
            for (int i = 0; i < curve.keys.Length; i++)
            {
                Keyframe k = new Keyframe(curve.keys[i].time + (scaleSpeed * Time.deltaTime), curve.keys[i].value, curve.keys[i].inTangent, curve.keys[i].outTangent);
                curve.MoveKey(i, k);
            }

            curve = render.modOffsetX;
            for (int i = 0; i < curve.keys.Length; i++)
            {
                Keyframe k = new Keyframe(curve.keys[i].time + (offsetXSpeed * Time.deltaTime), curve.keys[i].value, curve.keys[i].inTangent, curve.keys[i].outTangent);
                curve.MoveKey(i, k);
            }

            curve = render.modOffsetY;
            for (int i = 0; i < curve.keys.Length; i++)
            {
                Keyframe k = new Keyframe(curve.keys[i].time + (offsetYSpeed * Time.deltaTime), curve.keys[i].value, curve.keys[i].inTangent, curve.keys[i].outTangent);
                curve.MoveKey(i, k);
            }

            curve = render.modBendX;
            for (int i = 0; i < curve.keys.Length; i++)
            {
                Keyframe k = new Keyframe(curve.keys[i].time + (bendXSpeed * Time.deltaTime), curve.keys[i].value, curve.keys[i].inTangent, curve.keys[i].outTangent);
                curve.MoveKey(i, k);
            }

            curve = render.modBendY;
            for (int i = 0; i < curve.keys.Length; i++)
            {
                Keyframe k = new Keyframe(curve.keys[i].time + (bendYSpeed * Time.deltaTime), curve.keys[i].value, curve.keys[i].inTangent, curve.keys[i].outTangent);
                curve.MoveKey(i, k);
            }

            curve = render.modTwist;
            for (int i = 0; i < curve.keys.Length; i++)
            {
                Keyframe k = new Keyframe(curve.keys[i].time + (twistSpeed * Time.deltaTime), curve.keys[i].value, curve.keys[i].inTangent, curve.keys[i].outTangent);
                curve.MoveKey(i, k);
            }



            render.ApplyMods();
        }
    }
}
