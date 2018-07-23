using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "InputDeater_KeyBoard")]
public class InputDetecter_Keybaord : InputDetecter {

    public override void Update()
    {
        Horizontal = Input.GetAxisRaw("Horizontal");
        Vertical = Input.GetAxisRaw("Vertical");
        StartShoot = Input.GetMouseButtonDown(0);
        IsShooting = Input.GetMouseButton(0);
    }

}
