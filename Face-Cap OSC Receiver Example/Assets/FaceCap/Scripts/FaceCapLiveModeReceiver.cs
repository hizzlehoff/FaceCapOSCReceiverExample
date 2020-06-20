using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;

public class FaceCapLiveModeReceiver : MonoBehaviour
{
    [SerializeField]
    public int _OSCReceiverPort = 9000;

    private OSCReceiver _OSCReceiver;
    private const string _Position = "/HT";
    private const string _EulerAngles = "/HR";
    private const string _LeftEyeEulerAngles = "/ELR";
    private const string _RightEyeEulerAngles = "/ERR";
    private const string _Blendshapes = "/W";

    [SerializeField]
    public GameObject blendshapeMesh;
    [SerializeField]
    public int[] blendShapeIndexes;
    [SerializeField]
    public SkinnedMeshRenderer smr;

    [SerializeField]
    public bool usePositionData = false;
    Vector3 startPosition = new Vector3(0, 0, 0);

    [SerializeField]
    public bool useRotationData = false;

    [SerializeField]
    public Transform headTransform;
    [SerializeField]
    Quaternion headRotationOffset;

    [SerializeField]
    public bool neckTransformEnabled = false;
    [SerializeField]
    public Transform neckTransform;
    [SerializeField]
    public float neckTransformBlendFactor = 0.66f;
    Quaternion neckRotationInitial;
    Quaternion neckRotationOffset;

    [SerializeField]
    public bool spineTransformEnabled = false;
    [SerializeField]
    public Transform spineTransform;
    [SerializeField]
    public float spineTransformBlendFactor = 0.33f;
    Quaternion spineRotationInitial;
    Quaternion spineRotationOffset;

    [SerializeField]
    public bool useEyeDirectionData = false;

    [SerializeField]
    public Transform leftEyeTransform;
    Quaternion leftEyeRotationOffset;

    [SerializeField]
    public Transform rightEyeTransform;
    Quaternion rightEyeRotationOffset;

    bool isEveryThingConfigured = true;

    void Start()
    {

        if (blendshapeMesh == null)
        {
            isEveryThingConfigured = false;
            Debug.LogWarning("Face Cap Live Mode Receiver Error : Blenshape mesh is not assigned.");
        }
        else
        {
            smr = blendshapeMesh.GetComponent<SkinnedMeshRenderer>();
            if (smr == null)
            {
                isEveryThingConfigured = false;
                Debug.LogWarning("Face Cap Live Mode Receiver Error : Blenshape mesh has no blendshapes.");
            }
            else
            {
                if (smr.sharedMesh.blendShapeCount == 0)
                {
                    isEveryThingConfigured = false;
                    Debug.LogWarning("Face Cap Live Mode Receiver Error : Blenshape mesh has no blendshapes.");
                }
            }
        }

        if (usePositionData || useRotationData)
        {
            if (headTransform == null)
            {
                isEveryThingConfigured = false;
                Debug.LogWarning("Face Cap Live Mode Receiver Error : Head transform is not assigned.");
            }
            else
            {
                headRotationOffset = Quaternion.Inverse(ConvertScneneKitSpaceToUnitySpace(new Vector3(0, 0, 0))) * headTransform.rotation;
                startPosition = headTransform.localPosition;
            }

            if (neckTransformEnabled)
            {
                if (neckTransform == null)
                {
                    isEveryThingConfigured = false;
                    Debug.LogWarning("Face Cap Live Mode Receiver Error : Neck transform is not assigned.");
                }
                else
                {
                    neckRotationInitial = neckTransform.rotation;
                    neckRotationOffset = Quaternion.Inverse(ConvertScneneKitSpaceToUnitySpace(new Vector3(0, 0, 0))) * neckTransform.rotation;
                    startPosition = neckTransform.localPosition;
                }
            }

            if (spineTransformEnabled)
            {
                if (spineTransform == null)
                {
                    isEveryThingConfigured = false;
                    Debug.LogWarning("Face Cap Live Mode Receiver Error : Spine transform is not assigned.");
                }
                else
                {
                    spineRotationInitial = spineTransform.rotation;
                    spineRotationOffset = Quaternion.Inverse(ConvertScneneKitSpaceToUnitySpace(new Vector3(0, 0, 0))) * spineTransform.rotation;
                    startPosition = spineTransform.localPosition;
                }
            }
        }

        if (useEyeDirectionData)
        {
            if (leftEyeTransform == null && rightEyeTransform == null)
            {
                isEveryThingConfigured = false;
                Debug.LogWarning("Face Cap Live Mode Receiver Error : Assign at least 1 eye transform.");
            }

            if (leftEyeTransform != null)
            {
                leftEyeRotationOffset = Quaternion.Inverse(ConvertScneneKitSpaceToUnitySpace(new Vector3(0, 0, 0))) * leftEyeTransform.rotation;
            }

            if (rightEyeTransform != null)
            {
                rightEyeRotationOffset = Quaternion.Inverse(ConvertScneneKitSpaceToUnitySpace(new Vector3(0, 0, 0))) * rightEyeTransform.rotation;
            }
        }

        if (!isEveryThingConfigured)
        {
            return;
        }

        // Setup OSC Receiver

        _OSCReceiver = gameObject.AddComponent<OSCReceiver>();

        _OSCReceiver.LocalPort = _OSCReceiverPort;

        if (usePositionData)
        {
            _OSCReceiver.Bind(_Position, PositionReceived);
        }

        if (useRotationData)
        {
            _OSCReceiver.Bind(_EulerAngles, EulerAnglesReceived);
        }

        if (useEyeDirectionData)
        {
            _OSCReceiver.Bind(_LeftEyeEulerAngles, LeftEyeEulerAnglesReceived);
            _OSCReceiver.Bind(_RightEyeEulerAngles, RightEyeEulerAnglesReceived);
        }

        _OSCReceiver.Bind(_Blendshapes, BlendshapeReceived);

    }

    protected void PositionReceived(OSCMessage message)
    {
        Vector3 value;
        if (message.ToVector3(out value) && usePositionData)
        {
            value.x *= -1;

            if (spineTransformEnabled)
            {
                spineTransform.localPosition = startPosition + value; 
            }
            else if (neckTransformEnabled)
            {
                neckTransform.localPosition = startPosition + value;
            }
            else
            {
                headTransform.localPosition = startPosition + value;
            }
        }
    }

    protected void EulerAnglesReceived(OSCMessage message)
    {
        Vector3 value;
        if (message.ToVector3(out value) && useRotationData)
        {
            Quaternion sceneKitRotation = ConvertScneneKitSpaceToUnitySpace(value);

            if (spineTransformEnabled)
            {
                Quaternion newRotation = sceneKitRotation * spineRotationOffset;
                spineTransform.rotation = Quaternion.Lerp(spineRotationInitial, newRotation, spineTransformBlendFactor);
            }

            if (neckTransformEnabled)
            {
                Quaternion newRotation = sceneKitRotation * neckRotationOffset;
                neckTransform.rotation = Quaternion.Lerp(neckRotationInitial, newRotation, neckTransformBlendFactor);
            }

            headTransform.rotation =  ConvertScneneKitSpaceToUnitySpace(value) * headRotationOffset;
        }
    }

    protected void LeftEyeEulerAnglesReceived(OSCMessage message)
    {
        Vector2 value;
        if (message.ToVector2(out value) && leftEyeTransform != null)
        {
            Matrix4x4 inMatrix = Matrix4x4.Rotate(ConvertScneneKitSpaceToUnitySpace(new Vector3(value.x, value.y, 0)));
            leftEyeTransform.rotation =  ConvertScneneKitSpaceToUnitySpace( new Vector3(value.x,value.y,0)) * leftEyeRotationOffset;
        }
    }

    protected void RightEyeEulerAnglesReceived(OSCMessage message)
    {
        Vector2 value;
        if (message.ToVector2(out value) && rightEyeTransform != null)
        {
            Matrix4x4 inMatrix = Matrix4x4.Rotate(ConvertScneneKitSpaceToUnitySpace(new Vector3(value.x, value.y, 0)));
            rightEyeTransform.rotation =  ConvertScneneKitSpaceToUnitySpace(new Vector3(value.x, value.y, 0)) * rightEyeRotationOffset;
        }
    }

    protected void BlendshapeReceived(OSCMessage message)
    {
        int index = 0;
        float value = 0;

        if (message.ToInt(out index) && message.ToFloat(out value))
        {
            for (int i = 0; i < blendShapeIndexes.Length; i++)
            {
                if (blendShapeIndexes[i] == index)
                {
                    smr.SetBlendShapeWeight(i, value * 100f);
                }
            }
        }
    }

    protected Quaternion ConvertScneneKitSpaceToUnitySpace(Vector3 eulerAngles)
    {
        Quaternion q = Quaternion.Euler(eulerAngles);
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }

}