using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputDetecter : ScriptableObject {

    public float Horizontal { get; protected set; }
    public float Vertical { get; protected set; }
    public bool IsRotateingCameraRight { get; protected set; }
    public bool IsRotateingCameraLeft { get; protected set; }
    public bool IsStartingAttack { get; protected set; }
    public bool IsAttacking { get; protected set; }
    public bool IsStartingSpeicalInput { get; protected set; }
    public bool IsProcessingSpecialInput { get; protected set; }
    public bool IsStartingInteract { get; protected set; }
    public bool IsInteracting { get; protected set; }

    public abstract void Update();

}
