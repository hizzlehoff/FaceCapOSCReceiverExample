using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "FaceCapRemappingObject", menuName = "Create FaceCap Remapping Object")]
public class FaceCapObject : ScriptableObject
{
    [HideInInspector] [SerializeField] public List<FaceCapData> data = new List<FaceCapData>();
    [SerializeField] public SkinnedMeshRenderer sMR;

    public void AddData(int inputIndex, float multiplier)
    {
        FaceCapData f = new FaceCapData();
        f.inputIndex = inputIndex;
        f.multiplier = multiplier;
        data.Add(f);
    }

}
