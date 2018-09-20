using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : View {

    protected enum State
    {
        Normal,
        StartInteracting,
        Interactering,
        Canceling,
        Interacted,
        Ending
    }

    protected Actor m_actor = null;

    public void StartInteract()
    {
        m_state = State.StartInteracting;
    }

    public void StopInteract()
    {
        m_state = State.Canceling;
        m_actor = null;
    }

    public void SetActor(Actor actor)
    {
        if(m_actor != null)
        {
            return;
        }
        m_actor = actor;
    }

    protected abstract void Update_WhileNormal();
    protected abstract void Update_OnInteractingStarted();
    protected abstract void Update_WhileInteracting();
    protected abstract void Update_WhileCanceling();
    protected abstract void Update_OnInteracted();
    protected abstract void Update_InteractionEnding();

    protected State m_state = State.Normal;

    private void Update()
    {
        switch (m_state)
        {
            case State.Normal:
                {
                    Update_WhileNormal();
                    break;
                }
            case State.StartInteracting:
                {
                    Update_OnInteractingStarted();
                    break;
                }
            case State.Interactering:
                {
                    Update_WhileInteracting();
                    break;
                }
            case State.Canceling:
                {
                    Update_WhileCanceling();
                    break;
                }
            case State.Interacted:
                {
                    Update_OnInteracted();
                    break;
                }
            case State.Ending:
                {
                    Update_InteractionEnding();
                    break;
                }
        }
    }

}
