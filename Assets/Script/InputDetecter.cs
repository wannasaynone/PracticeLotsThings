using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputDetecter : MonoBehaviour {

    public float LeftKey_Vertical { get { return m_leftKey_vertical; } }
    public float LeftKey_Horizontal { get { return m_leftKey_horizontal; } }
    public float RightKey_Vertical { get { return m_rightKey_vertical; } }
    public float RightKey_Horizontal { get { return m_rightKey_horizontal; } }

    public bool KeyAPressed { get { return m_keyAState; } }
    public bool KeyBPressed { get { return m_keyBState; } }
    public bool KeyCPressed { get { return m_keyCState; } }
    public bool KeyAPressing { get; protected set; }
    public bool KeyBPressing { get; protected set; }
    public bool KeyCPressing { get; protected set; }

    protected float m_leftKey_vertical = 0f;
    protected float m_leftKey_horizontal = 0f;
    protected float m_rightKey_vertical = 0f;
    protected float m_rightKey_horizontal = 0f;

    protected bool m_keyAState = false;
    protected bool m_lastKeyAState = false;
    protected bool m_keyBState = false;
    protected bool m_lastKeyBState = false;
    protected bool m_keyCState = false;
    protected bool m_lastkeyCState = false;

    public abstract void DetectInput();

}
