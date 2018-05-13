using System;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorEventArgs
{
    public Animator Animator { get; private set; }
    public AnimatorStateInfo AnimatorStateInfo { get; private set; }
    public int LayerIndex { get; private set; }

    public AnimatorEventArgs(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Animator = animator;
        AnimatorStateInfo = stateInfo;
        LayerIndex = layerIndex;
    }
}

public class AnimatorEventSender : StateMachineBehaviour {

    private static Dictionary<string, Action<AnimatorEventArgs>> OnStateEntered = new Dictionary<string, Action<AnimatorEventArgs>>();
    private static Dictionary<string, Action<AnimatorEventArgs>> OnStateUpdated = new Dictionary<string, Action<AnimatorEventArgs>>();
    private static Dictionary<string, Action<AnimatorEventArgs>> OnStateExited = new Dictionary<string, Action<AnimatorEventArgs>>();

    [SerializeField] private List<string> m_triggerTag;

    public static void RegistOnStateEntered(string tag, Action<AnimatorEventArgs> method)
    {
        Regist(OnStateEntered, tag, method);
    }

    public static void RegistOnStateExited(string tag, Action<AnimatorEventArgs> method)
    {
        Regist(OnStateExited, tag, method);
    }

    public static void RegistOnStateUpdated(string tag, Action<AnimatorEventArgs> method)
    {
        Regist(OnStateUpdated, tag, method);
    }

    public static void UnregistOnStateEntered(string tag, Action<AnimatorEventArgs> method)
    {
        Unregist(OnStateEntered, tag, method);
    }

    public static void UnregistOnStateUpdated(string tag, Action<AnimatorEventArgs> method)
    {
        Unregist(OnStateUpdated, tag, method);
    }

    public static void UnregistOnStateExited(string tag, Action<AnimatorEventArgs> method)
    {
        Unregist(OnStateExited, tag, method);
    }

    private static void Regist(Dictionary<string, Action<AnimatorEventArgs>> keyValues, string tag, Action<AnimatorEventArgs> method)
    {
        if(keyValues.ContainsKey(tag))
        {
            keyValues[tag] += method;
        }
        else
        {
            keyValues.Add(tag, method);
        }
    }

    private static void Unregist(Dictionary<string, Action<AnimatorEventArgs>> keyValues, string tag, Action<AnimatorEventArgs> method)
    {
        if (keyValues.ContainsKey(tag))
        {
            keyValues[tag] -= method;
        }
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (KeyValuePair<string, Action<AnimatorEventArgs>> kvp in OnStateEntered)
        {
            if(m_triggerTag.Contains(kvp.Key))
            {
                kvp.Value(new AnimatorEventArgs(animator, stateInfo, layerIndex));
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (KeyValuePair<string, Action<AnimatorEventArgs>> kvp in OnStateExited)
        {
            if (m_triggerTag.Contains(kvp.Key))
            {
                kvp.Value(new AnimatorEventArgs(animator, stateInfo, layerIndex));
            }
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (KeyValuePair<string, Action<AnimatorEventArgs>> kvp in OnStateUpdated)
        {
            if (m_triggerTag.Contains(kvp.Key))
            {
                kvp.Value(new AnimatorEventArgs(animator, stateInfo, layerIndex));
            }
        }
    }

}
