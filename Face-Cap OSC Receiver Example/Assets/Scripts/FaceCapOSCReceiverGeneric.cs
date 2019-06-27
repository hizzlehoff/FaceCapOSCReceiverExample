using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace extOSC
{
    public class FaceCapOSCReceiverGeneric : MonoBehaviour
    {
        public Transform headTransform;
        public Transform eyeLTransform;
        public Transform eyeRTransform;

        public SkinnedMeshRenderer blendshapesGeometry;
        int blendshapesCount = 0;

        public bool usePositionData = true;

        public int _OSCReceiverPort = 8080;

        private OSCReceiver _OSCReceiver;

        private const string _Position = "/HT";
        private const string _EulerAngles = "/HR";
        private const string _LeftEyeEulerAngles = "/ELR";
        private const string _RightEyeEulerAngles = "/ERR";
        private const string _Blendshapes = "/W";

        protected virtual void Start()
        {
            // Get the amount of available blendshapes.
            if (blendshapesGeometry != null)
            {
                blendshapesCount = blendshapesGeometry.sharedMesh.blendShapeCount;

                string blendShapeNames = "Blendshape (" + blendshapesCount + ") index and name:\n";

                for (int i = 0; i < blendshapesCount; i++)
                {
                    blendShapeNames += (i + ":" + blendshapesGeometry.sharedMesh.GetBlendShapeName(i).ToString() + "\n");
                }

                Debug.Log(blendShapeNames);
            }
            else
            {
                Debug.Log("Error: please assign the skinnedMeshRenderer with blendshapes.");
            }

            // Creating an OSC receiver.
            _OSCReceiver = gameObject.AddComponent<OSCReceiver>();

            // Set local OSC port.
            _OSCReceiver.LocalPort = _OSCReceiverPort;

            // Bind OSC Addresses.
            if (usePositionData)
            {
                _OSCReceiver.Bind(_Position, PositionReceived);
            }

            _OSCReceiver.Bind(_EulerAngles, EulerAnglesReceived);

            _OSCReceiver.Bind(_LeftEyeEulerAngles, LeftEyeEulerAnglesReceived);
            _OSCReceiver.Bind(_RightEyeEulerAngles, RightEyeEulerAnglesReceived);

            _OSCReceiver.Bind(_Blendshapes, BlendshapeReceived);
        }

        protected void PositionReceived(OSCMessage message)
        {
            Vector3 value;
            if (message.ToVector3(out value))
            {
                headTransform.localPosition = value;
            }
        }

        protected void EulerAnglesReceived(OSCMessage message)
        {
            Vector3 value;
            if (message.ToVector3(out value))
            {
                ConvertEulerAnglesToUnitySpace(value, headTransform);
            }
        }

        protected void LeftEyeEulerAnglesReceived(OSCMessage message)
        {
            Vector2 value;
            if (message.ToVector2(out value))
            {
                ConvertEulerAnglesToUnitySpace(new Vector3(value.x, value.y, 0), eyeLTransform);
            }
        }

        protected void RightEyeEulerAnglesReceived(OSCMessage message)
        {
            Vector2 value;
            if (message.ToVector2(out value))
            {
                ConvertEulerAnglesToUnitySpace(new Vector3(value.x,value.y,0), eyeRTransform);
            }
        }

        protected void BlendshapeReceived(OSCMessage message)
        {
            int index = 0; 
            float value = 0;

            if (message.ToInt(out index) && message.ToFloat(out value))
            {
                if (index >= 0 && index < blendshapesCount)
                {
                    blendshapesGeometry.SetBlendShapeWeight(index, value * 100f);
                }
            }
        }

        protected void ConvertEulerAnglesToUnitySpace(Vector3 eulerAngles, Transform t)
        {
            t.localEulerAngles = eulerAngles;
            t.localRotation = new Quaternion(-t.localRotation.x, t.localRotation.y, t.localRotation.z, -t.localRotation.w);
        }

    }

}
