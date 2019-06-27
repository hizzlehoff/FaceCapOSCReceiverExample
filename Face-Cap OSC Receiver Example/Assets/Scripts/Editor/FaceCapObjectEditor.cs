using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(FaceCapObject))]
public class FaceCapObjectEditor : Editor
{
    FaceCapObject faceCapObject;

    string[] inputNames = {
                        "browInnerUp",
                        "browDown_L",
                        "browDown_R",
                        "browOuterUp_L",
                        "browOuterUp_R",
                        "eyeLookUp_L",
                        "eyeLookUp_R",
                        "eyeLookDown_L",
                        "eyeLookDown_R",
                        "eyeLookIn_L",
                        "eyeLookIn_R",
                        "eyeLookOut_L",
                        "eyeLookOut_R",
                        "eyeBlink_L",
                        "eyeBlink_R",
                        "eyeSquint_L",
                        "eyeSquint_R",
                        "eyeWide_L",
                        "eyeWide_R",
                        "cheekPuff",
                        "cheekSquint_L",
                        "cheekSquint_R",
                        "noseSneer_L",
                        "noseSneer_R",
                        "jawOpen",
                        "jawForward",
                        "jawLeft",
                        "jawRight",
                        "mouthFunnel",
                        "mouthPucker",
                        "mouthLeft",
                        "mouthRight",
                        "mouthRollUpper",
                        "mouthRollLower",
                        "mouthShrugUpper",
                        "mouthShrugLower",
                        "mouthClose",
                        "mouthSmile_L",
                        "mouthSmile_R",
                        "mouthFrown_L",
                        "mouthFrown_R",
                        "mouthDimple_L",
                        "mouthDimple_R",
                        "mouthUpperUp_L",
                        "mouthUpperUp_R",
                        "mouthLowerDown_L",
                        "mouthLowerDown_R",
                        "mouthPress_L",
                        "mouthPress_R",
                        "mouthStretch_L",
                        "mouthStretch_R",
                        "tongueOut",
                        "No input selected:"
                        };

    public override void OnInspectorGUI()
    {
        faceCapObject = (FaceCapObject)target;

        DrawDefaultInspector();

        if (faceCapObject.sMR == null)
        {
            EditorGUILayout.HelpBox(new GUIContent("Please assign a SkinnedMeshRenderer from your assets/."));
            return;
        }

        // Get output blendshape names:

        string[] outputNames = new string[faceCapObject.sMR.sharedMesh.blendShapeCount];

        for (int i = 0; i < outputNames.Length; i++)
        {
            outputNames[i] = faceCapObject.sMR.sharedMesh.GetBlendShapeName(i);
        }

        // Create remapping options:

        if (faceCapObject.data.Count < 1)
        {
            // Todo: If the amount of blendshapes is updated outside of Unity how to deal with it?

            for (int i = 0; i < outputNames.Length; i++)
            {
                faceCapObject.AddData(inputNames.Length - 1, 1);
            }
        }

        EditorGUILayout.Space();

        // Draw header:

        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField(new GUIContent("Face Cap Input = Custom Output * multiplier."));

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

        // Draw options:

        for (int i = 0; i < faceCapObject.data.Count; i++)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent(i.ToString()), GUILayout.MaxWidth(22));

            string sourceBlendShapeName = inputNames[faceCapObject.data[i].inputIndex];

            if (EditorGUILayout.DropdownButton(new GUIContent(sourceBlendShapeName), FocusType.Keyboard, GUILayout.MaxWidth(148)))
            {
                ShowInputOptions(i);
            }

            EditorGUILayout.LabelField(new GUIContent("= " + outputNames[i] + " *"));

            faceCapObject.data[i].multiplier = EditorGUILayout.FloatField(faceCapObject.data[i].multiplier, GUILayout.MaxWidth(40));

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField(new GUIContent("Use 'save project' to save this configuration."));

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

        // Mark scriptable object as dirty.
        EditorUtility.SetDirty(faceCapObject);
    }

    private void ShowInputOptions(int dataIndex)
    {
        GenericMenu menu = new GenericMenu();

        for (int i = 0; i < inputNames.Length; i++)
        {
            string option = inputNames[i];
            bool isActive = false;
            
            if (i == faceCapObject.data[dataIndex].inputIndex)
            {
                isActive = true;
            }
            menu.AddItem(new GUIContent(option), isActive, StoreSelectedOption, (dataIndex+"."+i));
        }

        menu.ShowAsContext();
    }

    private void StoreSelectedOption(object _object)
    {
        string inpuString = (string) _object;

        int dataIndex = int.Parse(inpuString.Split('.')[0]);
        int inputIndex = int.Parse(inpuString.Split('.')[1]);

        faceCapObject.data[dataIndex].inputIndex = inputIndex;
    }

}
