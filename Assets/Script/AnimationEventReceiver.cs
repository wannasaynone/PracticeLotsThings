using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventReceiver : MonoBehaviour {

    public static Action VoidAction;
    public static Action<int> IntAction;
    public static Action<string> StringAction;
    public static Action<bool> BoolAction;
    public static Action<object> ObjectAction;

    public void Invoke()
    {
        if(VoidAction != null)
        {
            VoidAction.Invoke();
            VoidAction = null;
        }
    }

    public void Invoke(int value)
    {
        if (IntAction != null)
        {
            IntAction.Invoke(value);
            IntAction = null;
        }
    }

    public void Invoke(string value)
    {
        if (StringAction != null)
        {
            StringAction.Invoke(value);
            StringAction = null;
        }
    }

    public void Invoke(bool value)
    {
        if (BoolAction != null)
        {
            BoolAction.Invoke(value);
            BoolAction = null;
        }
    }

    public void Invoke(object value)
    {
        if (ObjectAction != null)
        {
            ObjectAction.Invoke(value);
            ObjectAction = null;
        }
    }

}
