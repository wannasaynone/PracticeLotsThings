using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI Behaviour/State")]
public class State : ScriptableObject {

    public List<Transition> transitions = new List<Transition>();

	public void Tick()
    {

    }

    public Transition AddTransition()
    {
        Transition _newTrans = new Transition();
        transitions.Add(_newTrans);
        return _newTrans;
    }

}
