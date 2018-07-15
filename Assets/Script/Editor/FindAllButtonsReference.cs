using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class FindAllButtonsReference {

	[MenuItem("Tools/Find All Buttons Reference Function")]
	public static void FindAllButtonsReferenceFunction()
	{
		Button[] _allButtons = Resources.FindObjectsOfTypeAll<Button>();
		for (int i = 0; i < _allButtons.Length; i++)
		{
			string _prefabPath = _allButtons[i].name;
            GameObject _tempGameObject = _allButtons[i].gameObject;

            for (int j = 0; j < 10; j++)
            {
                if (_tempGameObject.transform.parent != null)
                {
					_tempGameObject = _tempGameObject.transform.parent.gameObject;
					_prefabPath = _tempGameObject.name + "/" + _prefabPath;
                }
                else
                {
                    break;
                }
            }

			for (int m = 0; m < _allButtons[i].onClick.GetPersistentEventCount(); m++)
			{
				Debug.LogError(_prefabPath + " : " + _allButtons[i].onClick.GetPersistentMethodName(m));
			}
		}
	}

}