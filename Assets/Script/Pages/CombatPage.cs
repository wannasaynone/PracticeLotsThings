using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatPage : Page {

    public event Action<int> OnDanageSet;

    public void SetDamage(int value)
    {
        Debug.Log("Display Dmg=" + value);
        if(OnDanageSet != null)
        {
            OnDanageSet(value);
        }
    }

}
