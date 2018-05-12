using System;
using System.Collections;
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

    public static Dictionary<string, Action<AnimatorEventArgs>> OnStateEntered = new Dictionary<string, Action<AnimatorEventArgs>>();
    public static Dictionary<string, Action<AnimatorEventArgs>> OnStateExited = new Dictionary<string, Action<AnimatorEventArgs>>();

    [SerializeField] private string[] m_triggerTag;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (KeyValuePair<string, Action<AnimatorEventArgs>> kvp in OnStateEntered)
        {
            kvp.Value(new AnimatorEventArgs(animator, stateInfo, layerIndex));
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (KeyValuePair<string, Action<AnimatorEventArgs>> kvp in OnStateExited)
        {
            kvp.Value(new AnimatorEventArgs(animator, stateInfo, layerIndex));
        }
    }
}
