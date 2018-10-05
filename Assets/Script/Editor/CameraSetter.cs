using UnityEngine;
using UnityEditor;
using PracticeLotsThings.MainGameMonoBehaviour;

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