using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FaceCapLiveModeReceiver))]
public class FaceCapLiveModeReceiverEditor : Editor
{
    FaceCapLiveModeReceiver script;

    string[] faceCapBlendshapeNames = { "browInnerUp", "browDown_L", "browDown_R", "browOuterUp_L","browOuterUp_R","eyeLookUp_L","eyeLookUp_R","eyeLookDown_L","eyeLookDown_R","eyeLookIn_L","eyeLookIn_R","eyeLookOut_L","eyeLookOut_R","eyeBlink_L","eyeBlink_R","eyeSquint_L","eyeSquint_R","eyeWide_L","eyeWide_R","cheekPuff","cheekSquint_L","cheekSquint_R","noseSneer_L","noseSneer_R","jawOpen","jawForward","jawLeft","jawRight","mouthFunnel","mouthPucker","mouthLeft","mouthRight","mouthRollUpper","mouthRollLower","mouthShrugUpper","mouthShrugLower","mouthClose","mouthSmile_L","mouthSmile_R","mouthFrown_L","mouthFrown_R","mouthDimple_L","mouthDimple_R","mouthUpperUp_L","mouthUpperUp_R","mouthLowerDown_L","mouthLowerDown_R","mouthPress_L","mouthPress_R","mouthStretch_L","mouthStretch_R","tongueOut","none"};
    string[] polywinkBlendshapeNames = { "browInnerUp", "browDownLeft", "browDownRight", "browOuterUpLeft", "browOuterUpRight", "eyeLookUpLeft", "eyeLookUpRight", "eyeLookDownLeft", "eyeLookDownRight", "eyeLookInLeft", "eyeLookInRight", "eyeLookOutLeft", "eyeLookOutRight", "eyeBlinkLeft", "eyeBlinkRight", "eyeSquintLeft", "eyeSquintRight", "eyeWideLeft", "eyeWideRight", "cheekPuff", "cheekSquintLeft", "cheekSquintRight", "noseSneerLeft", "noseSneerRight", "jawOpen", "jawForward", "jawLeft", "jawRight", "mouthFunnel", "mouthPucker", "mouthLeft", "mouthRight", "mouthRollUpper", "mouthRollLower", "mouthShrugUpper", "mouthShrugLower", "mouthClose", "mouthSmileLeft", "mouthSmileRight", "mouthFrownLeft", "mouthFrownRight", "mouthDimpleLeft", "mouthDimpleRight", "mouthUpperUpLeft", "mouthUpperUpRight", "mouthLowerDownLeft", "mouthLowerDownRight", "mouthPressLeft", "mouthPressRight", "mouthStretchLeft", "mouthStretchRight", "tongueOut", "none" };

    void OnEnable()
    {
        script = (FaceCapLiveModeReceiver)target;
    }

    public override void OnInspectorGUI()
    {
        script._OSCReceiverPort = EditorGUILayout.IntField("OSC Port", script._OSCReceiverPort);

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        GameObject blendshapeMesh = script.blendshapeMesh;

        blendshapeMesh = EditorGUILayout.ObjectField("Blendshape Mesh", blendshapeMesh, typeof(GameObject), true) as GameObject;

        if (blendshapeMesh != script.blendshapeMesh)
        {
            script.blendshapeMesh = blendshapeMesh;
            EditorUtility.SetDirty(script);
        }

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        bool usePositionData = script.usePositionData;
        bool useRotationData = script.useRotationData;

        usePositionData = EditorGUILayout.Toggle("Use Position Data", usePositionData);
        useRotationData = EditorGUILayout.Toggle("Use Rotation Data", useRotationData);

        if (usePositionData != script.usePositionData || useRotationData != script.useRotationData)
        {
            script.usePositionData = usePositionData;
            script.useRotationData = useRotationData;
            EditorUtility.SetDirty(script);
        }

        if (usePositionData || useRotationData)
        {
            EditorGUILayout.Space();

            Transform headTransform = script.headTransform;

            headTransform = EditorGUILayout.ObjectField("Head Transform", headTransform, typeof(Transform), true) as Transform;
            if (headTransform != script.headTransform)
            {
                script.headTransform = headTransform;
                EditorUtility.SetDirty(script);
            }

            bool neckTransformEnabled = script.neckTransformEnabled;
            neckTransformEnabled = EditorGUILayout.Toggle("Neck", neckTransformEnabled);
            if (neckTransformEnabled != script.neckTransformEnabled)
            {
                script.neckTransformEnabled = neckTransformEnabled;
                EditorUtility.SetDirty(script);
            }

            if (neckTransformEnabled)
            {
                Transform neckTransform = script.neckTransform;
                float neckTransformBlendFactor = script.neckTransformBlendFactor;

                neckTransform = EditorGUILayout.ObjectField("Neck Transform", neckTransform, typeof(Transform), true) as Transform;
                neckTransformBlendFactor = EditorGUILayout.FloatField("Neck Blend", neckTransformBlendFactor);

                if (neckTransform != script.neckTransform || neckTransformBlendFactor != script.neckTransformBlendFactor)
                {
                    script.neckTransform = neckTransform;
                    script.neckTransformBlendFactor = neckTransformBlendFactor;
                    EditorUtility.SetDirty(script);
                }
            }

            bool spineTransformEnabled = script.spineTransformEnabled;
            spineTransformEnabled = EditorGUILayout.Toggle("Spine", spineTransformEnabled);
            if (spineTransformEnabled != script.spineTransformEnabled)
            {
                script.spineTransformEnabled = spineTransformEnabled;
                EditorUtility.SetDirty(script);
            }

            if (spineTransformEnabled)
            {
                Transform spineTransform = script.spineTransform;
                float spineTransformBlendFactor = script.spineTransformBlendFactor;

                spineTransform = EditorGUILayout.ObjectField("Spine Transform", spineTransform, typeof(Transform), true) as Transform;
                spineTransformBlendFactor = EditorGUILayout.FloatField("Spine Blend", spineTransformBlendFactor);

                if (spineTransform != script.spineTransform || spineTransformBlendFactor != script.spineTransformBlendFactor)
                {
                    script.spineTransform = spineTransform;
                    script.spineTransformBlendFactor = spineTransformBlendFactor;
                    EditorUtility.SetDirty(script);
                }
            }
        }

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        bool useEyeDirectionData = script.useEyeDirectionData;
        useEyeDirectionData = EditorGUILayout.Toggle("Use Eye Direction Data", useEyeDirectionData);
        if (useEyeDirectionData != script.useEyeDirectionData)
        {
            script.useEyeDirectionData = useEyeDirectionData;
            EditorUtility.SetDirty(script);
        }

        if (useEyeDirectionData)
        {
            EditorGUILayout.Space();

            Transform leftEyeTransform = script.leftEyeTransform;
            Transform rightEyeTransform = script.rightEyeTransform;

            leftEyeTransform = EditorGUILayout.ObjectField("Left Eye Transform", leftEyeTransform, typeof(Transform), true) as Transform;
            rightEyeTransform = EditorGUILayout.ObjectField("Right Eye Transform", rightEyeTransform, typeof(Transform), true) as Transform;

            if (leftEyeTransform != script.leftEyeTransform || rightEyeTransform != script.rightEyeTransform)
            {
                script.leftEyeTransform = leftEyeTransform;
                script.rightEyeTransform = rightEyeTransform;
                EditorUtility.SetDirty(script);
            }
        }

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        EditorGUILayout.LabelField("Blendshape configuration:");

        if (blendshapeMesh != null)
        {
            SkinnedMeshRenderer smr = blendshapeMesh.GetComponent<SkinnedMeshRenderer>();
            if (!smr)
            {
                GUILayout.BeginHorizontal(EditorStyles.helpBox);
                EditorGUILayout.LabelField("Warning: Assigned blendshape mesh has no skinned mesh renderer.");
                GUILayout.EndHorizontal();
                return;
            }

            if (smr.sharedMesh.blendShapeCount == 0)
            {
                GUILayout.BeginHorizontal(EditorStyles.helpBox);
                EditorGUILayout.LabelField("Warning: Assigned blendshape mesh has no blendshapes.");
                GUILayout.EndHorizontal();
                return;
            }

            int[] blendShapeIndexes = script.blendShapeIndexes;

            if (blendShapeIndexes == null)
            {
                blendShapeIndexes = InitializeBlendshapes(smr);
                blendShapeIndexes = AutoConfigureBlendshapes(smr);

                if (blendShapeIndexes != script.blendShapeIndexes)
                {
                    script.blendShapeIndexes = blendShapeIndexes;
                    EditorUtility.SetDirty(script);
                }
            }

            for (int i = 0; i < blendShapeIndexes.Length; i++)
            {
                GUILayout.BeginHorizontal(EditorStyles.label);
                string blendShapeName = smr.sharedMesh.GetBlendShapeName(i);
                blendShapeIndexes[i] = EditorGUILayout.Popup(blendShapeName, blendShapeIndexes[i], faceCapBlendshapeNames);

                if (blendShapeIndexes[i] != script.blendShapeIndexes[i])
                {
                    script.blendShapeIndexes[i] = blendShapeIndexes[i];
                    EditorUtility.SetDirty(script);
                }

                GUILayout.EndHorizontal();
            }

            GUILayout.BeginHorizontal(EditorStyles.helpBox);
            if (GUILayout.Button("Initialize"))
            {
                blendShapeIndexes = InitializeBlendshapes(smr);
                if (blendShapeIndexes != script.blendShapeIndexes)
                {
                    script.blendShapeIndexes = blendShapeIndexes;
                    EditorUtility.SetDirty(script);
                }
            }
            if (GUILayout.Button("Automatic"))
            {
                blendShapeIndexes = AutoConfigureBlendshapes(smr);
                if (blendShapeIndexes != script.blendShapeIndexes)
                {
                    script.blendShapeIndexes = blendShapeIndexes;
                    EditorUtility.SetDirty(script);
                }
            }
            GUILayout.EndHorizontal();
        }
        else
        {
            GUILayout.BeginHorizontal(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Warning: Blendshape mesh not assigned.");
            GUILayout.EndHorizontal();
            return;
        }

        EditorGUILayout.Space();
    }

    int[] InitializeBlendshapes(SkinnedMeshRenderer smr)
    {
        int[] blendShapeIndexes = new int[smr.sharedMesh.blendShapeCount];

        for (int i = 0; i < blendShapeIndexes.Length; i++)
        {
            blendShapeIndexes[i] = faceCapBlendshapeNames.Length - 1;
        }

        return blendShapeIndexes;
    }

    int[] AutoConfigureBlendshapes(SkinnedMeshRenderer smr)
    {
        int[] blendShapeIndexes = new int[smr.sharedMesh.blendShapeCount];

        for (int i = 0; i < blendShapeIndexes.Length; i++)
        {
            string name = smr.sharedMesh.GetBlendShapeName(i);

            for (int j=0; j < faceCapBlendshapeNames.Length; j++)
            {
                if (name.Contains(faceCapBlendshapeNames[j]) || name.Contains(polywinkBlendshapeNames[j]))
                {
                    blendShapeIndexes[i] = j;
                }
            }
        }
        return blendShapeIndexes;
    }

}
