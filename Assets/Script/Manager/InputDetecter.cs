using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputDetecter : ScriptableObject {

    public float Horizontal { get; protected set; }
    public float Vertical { get; protected set; }
    public bool StartAttack { get; protected set; }
    public bool IsAttacking { get; protected set; }

    public abstract void Update();

}
