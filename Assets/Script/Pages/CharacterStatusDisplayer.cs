using System;
using UnityEngine;

public class CharacterStatusDisplayer : Page {

    public void SetHp(int value)
    {
        Debug.Log("Display HP=" + value);
    }

    public void SetAtk(int value)
    {
        Debug.Log("Display Atk=" + value);
    }

    public void SetDef(int value)
    {
        Debug.Log("Display Def=" + value);
    }

    public void SetMat(int value)
    {
        Debug.Log("Display Mat=" + value);
    }

}
