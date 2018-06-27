using UnityEngine;

public abstract class InputDetecter : ScriptableObject {

    public float LeftKey_Vertical { get { return m_leftKey_vertical; } }
    public float LeftKey_Horizontal { get { return m_leftKey_horizontal; } }
    public float RightKey_Vertical { get { return m_rightKey_vertical; } }
    public float RightKey_Horizontal { get { return m_rightKey_horizontal; } }

    public bool KeyAPressed { get; protected set; }
    public bool KeyBPressed { get; protected set; }
    public bool KeyCPressed { get; protected set; }
    public bool KeyDPressed { get; protected set; }
    public bool KeyEPressed { get; protected set; }
    public bool KeyFPressed { get; protected set; }
    public bool KeyAPressing { get; protected set; }
    public bool KeyBPressing { get; protected set; }
    public bool KeyCPressing { get; protected set; }
    public bool KeyDPressing { get; protected set; }
    public bool KeyEPressing { get; protected set; }
    public bool KeyFPressing { get; protected set; }
    public bool KeyAUp { get; protected set; }
    public bool KeyBUp { get; protected set; }
    public bool KeyCUp { get; protected set; }
    public bool KeyDUp { get; protected set; }
    public bool KeyEUp { get; protected set; }
    public bool KeyFUp { get; protected set; }

    protected float m_leftKey_vertical = 0f;
    protected float m_leftKey_horizontal = 0f;
    protected float m_rightKey_vertical = 0f;
    protected float m_rightKey_horizontal = 0f;

    public abstract void DetectInput();

}
