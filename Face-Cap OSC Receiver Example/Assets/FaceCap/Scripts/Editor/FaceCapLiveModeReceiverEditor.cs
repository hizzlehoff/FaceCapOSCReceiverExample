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

        script.blendshapeMesh = EditorGUILayout.ObjectField("Blendshape Mesh", script.blendshapeMesh, typeof(GameObject), true) as GameObject;

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        script.usePositionData = EditorGUILayout.Toggle("Use Position Data", script.usePositionData);
        script.useRotationData = EditorGUILayout.Toggle("Use Rotation Data", script.useRotationData);

        if (script.usePositionData || script.useRotationData)
        {
            EditorGUILayout.Space();

            script.headTransform = EditorGUILayout.ObjectField("Head Transform", script.headTransform, typeof(Transform), true) as Transform;

            script.neckTransformEnabled = EditorGUILayout.Toggle("Neck", script.neckTransformEnabled);
            if (script.neckTransformEnabled)
            {
                script.neckTransform = EditorGUILayout.ObjectField("Neck Transform", script.neckTransform, typeof(Transform), true) as Transform;
                script.neckTransformBlendFactor = EditorGUILayout.FloatField("Neck Blend", script.neckTransformBlendFactor);
            }

            script.spineTransformEnabled = EditorGUILayout.Toggle("Spine", script.spineTransformEnabled);
            if (script.spineTransformEnabled)
            {
                script.spineTransform = EditorGUILayout.ObjectField("Spine Transform", script.spineTransform, typeof(Transform), true) as Transform;
                script.spineTransformBlendFactor = EditorGUILayout.FloatField("Spine Blend", script.spineTransformBlendFactor);
            }
        }

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        script.useEyeDirectionData = EditorGUILayout.Toggle("Use Eye Direction Data", script.useEyeDirectionData);

        
        if (script.useEyeDirectionData)
        {
            EditorGUILayout.Space();

            script.leftEyeTransform = EditorGUILayout.ObjectField("Left Eye Transform", script.leftEyeTransform, typeof(Transform), true) as Transform;
            script.rightEyeTransform = EditorGUILayout.ObjectField("Right Eye Transform", script.rightEyeTransform, typeof(Transform), true) as Transform;
        }

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        EditorGUILayout.LabelField("Blendshape configuration:");

        if (script.blendshapeMesh != null)
        {
            SkinnedMeshRenderer smr = script.blendshapeMesh.GetComponent<SkinnedMeshRenderer>();
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

            if (script.blendShapeIndexes == null)
            {
                InitializeBlendshapes(script, smr);
                AutoConfigureBlendshapes(script, smr);
            }

            for (int i = 0; i < script.blendShapeIndexes.Length; i++)
            {
                GUILayout.BeginHorizontal(EditorStyles.label);
                string blendShapeName = smr.sharedMesh.GetBlendShapeName(i);
                script.blendShapeIndexes[i] = EditorGUILayout.Popup(blendShapeName, script.blendShapeIndexes[i], faceCapBlendshapeNames);
                GUILayout.EndHorizontal();
            }

            GUILayout.BeginHorizontal(EditorStyles.helpBox);
            if (GUILayout.Button("Initialize"))
            {
                InitializeBlendshapes(script, smr);
            }
            if (GUILayout.Button("Automatic"))
            {
                AutoConfigureBlendshapes(script, smr);
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

        EditorUtility.SetDirty(script);
    }

    void InitializeBlendshapes(FaceCapLiveModeReceiver script, SkinnedMeshRenderer smr)
    {
        script.blendShapeIndexes = new int[smr.sharedMesh.blendShapeCount];

        for (int i = 0; i < script.blendShapeIndexes.Length; i++)
        {
            script.blendShapeIndexes[i] = faceCapBlendshapeNames.Length - 1;
        }
    }

    void AutoConfigureBlendshapes(FaceCapLiveModeReceiver script, SkinnedMeshRenderer smr)
    {
        script.blendShapeIndexes = new int[smr.sharedMesh.blendShapeCount];

        for (int i = 0; i < script.blendShapeIndexes.Length; i++)
        {
            string name = smr.sharedMesh.GetBlendShapeName(i);

            for (int j=0; j < faceCapBlendshapeNames.Length; j++)
            {
                if (name.Contains(faceCapBlendshapeNames[j]) || name.Contains(polywinkBlendshapeNames[j]))
                {
                    script.blendShapeIndexes[i] = j;
                }
            }
        }

    }
}
