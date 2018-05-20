using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventReceiver : MonoBehaviour {

    private static Action VoidAction;
    private static Action<int> IntAction;
    private static Action<float> FloatAction;
    private static Action<string> StringAction;
    private static Action<object> ObjectAction;

    public static void RegistAction(Action action)
    {
        VoidAction += action;
    }

    public static void RegistAction(Action<int> action)
    {
        IntAction += action;
    }

    public static void RegistAction(Action<float> action)
    {
        FloatAction += action;
    }

    public static void RegistAction(Action<string> action)
    {
        StringAction += action;
    }

    public static void RegistAction(Action<object> action)
    {
        ObjectAction += action;
    }

    public static void UnregistAction(Action action)
    {
        VoidAction -= action;
    }

    public static void UnregistAction(Action<int> action)
    {
        IntAction -= action;
    }

    public static void UnregistAction(Action<float> action)
    {
        FloatAction -= action;
    }

    public static void UnregistAction(Action<string> action)
    {
        StringAction -= action;
    }

    public static void UnregistAction(Action<object> action)
    {
        ObjectAction -= action;
    }

    private void Invoke()
    {
        // Debug.Log("Invoke()");
        if(VoidAction != null)
        {
            VoidAction.Invoke();
        }
    }

    private void InvokeInt(int value)
    {
        // Debug.Log("Invoke(" + value + ")");
        if (IntAction != null)
        {
            IntAction.Invoke(value);
        }
    }

    private void InvokeFloat(float value)
    {
        // Debug.Log("Invoke(" + value + ")");
        if (FloatAction != null)
        {
            FloatAction.Invoke(value);
        }
    }

    private void InvokeString(string value)
    {
        // Debug.Log("Invoke(" + value + ")");
        if (StringAction != null)
        {
            StringAction.Invoke(value);
        }
    }

    private void InvokeObject(object value)
    {
        // Debug.Log("Invoke(" + value + ")");
        if (ObjectAction != null)
        {
            ObjectAction.Invoke(value);
        }
    }

}
