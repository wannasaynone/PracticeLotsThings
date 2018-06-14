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

    private static Dictionary<string, Dictionary<ActorController, Action<AnimatorEventArgs>>> OnStateEntered = new Dictionary<string, Dictionary<ActorController, Action<AnimatorEventArgs>>>();
    private static Dictionary<string, Dictionary<ActorController, Action<AnimatorEventArgs>>> OnStateUpdated = new Dictionary<string, Dictionary<ActorController, Action<AnimatorEventArgs>>>();
    private static Dictionary<string, Dictionary<ActorController, Action<AnimatorEventArgs>>> OnStateExited = new Dictionary<string, Dictionary<ActorController, Action<AnimatorEventArgs>>>();

    [SerializeField] private List<string> m_triggerTag;
    [SerializeField] private List<string> m_clearAnimatorTrigger;

    public static void RegisterOnStateEntered(string tag, ActorController actor, Action<AnimatorEventArgs> method)
    {
        Register(OnStateEntered, tag, actor, method);
    }

    public static void RegisterOnStateExited(string tag, ActorController actor, Action<AnimatorEventArgs> method)
    {
        Register(OnStateExited, tag, actor, method);
    }

    public static void RegisterOnStateUpdated(string tag, ActorController actor, Action<AnimatorEventArgs> method)
    {
        Register(OnStateUpdated, tag, actor, method);
    }

    public static void UnregisterOnStateEntered(string tag, ActorController actor, Action<AnimatorEventArgs> method)
    {
        Unregister(OnStateEntered, tag, actor, method);
    }

    public static void UnregisterOnStateUpdated(string tag, ActorController actor, Action<AnimatorEventArgs> method)
    {
        Unregister(OnStateUpdated, tag, actor, method);
    }

    public static void UnregisterOnStateExited(string tag, ActorController actor, Action<AnimatorEventArgs> method)
    {
        Unregister(OnStateExited, tag, actor, method);
    }

    private static void Register(Dictionary<string, Dictionary<ActorController, Action<AnimatorEventArgs>>> keyValues, string tag, ActorController actor, Action<AnimatorEventArgs> method)
    {
        if(keyValues.ContainsKey(tag))
        {
            if(keyValues[tag].ContainsKey(actor))
            {
                keyValues[tag][actor] += method;
            }
            else
            {
                keyValues[tag].Add(actor, method);
            }
        }
        else
        {
            keyValues.Add(tag, new Dictionary<ActorController, Action<AnimatorEventArgs>>() { { actor, method } });
        }
    }

    private static void Unregister(Dictionary<string, Dictionary<ActorController, Action<AnimatorEventArgs>>> keyValues, string tag, ActorController actor, Action<AnimatorEventArgs> method)
    {
        if (keyValues.ContainsKey(tag))
        {
            if (keyValues[tag].ContainsKey(actor))
            {
                keyValues[tag][actor] -= method;
            }
        }
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        for (int i = 0; i < m_clearAnimatorTrigger.Count; i++)
        {
            animator.ResetTrigger(m_clearAnimatorTrigger[i]);
        }

        foreach (KeyValuePair<string, Dictionary<ActorController, Action<AnimatorEventArgs>>> kvp in OnStateEntered)
        {
            if(m_triggerTag.Contains(kvp.Key))
            {
                foreach (ActorController actor in kvp.Value.Keys)
                {
                    if (actor.ModelAnimator == animator)
                    {
                        kvp.Value[actor](new AnimatorEventArgs(animator, stateInfo, layerIndex));
                        return;
                    }
                }
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (KeyValuePair<string, Dictionary<ActorController, Action<AnimatorEventArgs>>> kvp in OnStateExited)
        {
            if (m_triggerTag.Contains(kvp.Key))
            {
                foreach (ActorController actor in kvp.Value.Keys)
                {
                    if (actor.ModelAnimator == animator)
                    {
                        kvp.Value[actor](new AnimatorEventArgs(animator, stateInfo, layerIndex));
                        return;
                    }
                }
            }
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (KeyValuePair<string, Dictionary<ActorController, Action<AnimatorEventArgs>>> kvp in OnStateUpdated)
        {
            if (m_triggerTag.Contains(kvp.Key))
            {
                foreach (ActorController actor in kvp.Value.Keys)
                {
                    if (actor.ModelAnimator == animator)
                    {
                        kvp.Value[actor](new AnimatorEventArgs(animator, stateInfo, layerIndex));
                        return;
                    }
                }
            }
        }
    }

}
