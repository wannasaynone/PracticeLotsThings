using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatPage : Page {

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            var _boxes = HitBox.HitBoxes;
            for(int i = 0; i < _boxes.Count; i++)
            {
                Debug.Log(_boxes[i].name);
            }
        }
    }

}
