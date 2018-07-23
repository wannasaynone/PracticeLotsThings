using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Camera))]
public class CameraSetter : Editor {

    [ExecuteInEditMode]
    private void Reset()
    {
        if (((Camera)target).GetComponent<CameraController>() == null)
        {
            ((Camera)target).gameObject.AddComponent<CameraController>();
        }
    }

}