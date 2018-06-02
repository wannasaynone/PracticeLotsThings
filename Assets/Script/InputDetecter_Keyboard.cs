﻿using UnityEngine;

[CreateAssetMenu(menuName = ("Controller/Key Board"))]
public class InputDetecter_Keyboard : InputDetecter {

    [SerializeField] private string m_leftKey_Up = "w";
    [SerializeField] private string m_leftKey_Down = "s";
    [SerializeField] private string m_leftKey_Right = "d";
    [SerializeField] private string m_leftKey_Left = "a";
    [SerializeField] private string m_rightKey_Up = "up";
    [SerializeField] private string m_rightKey_Down = "down";
    [SerializeField] private string m_rightKey_Right = "right";
    [SerializeField] private string m_rightKey_Left = "left";
    [SerializeField] private string m_keyA = "left shift";
    [SerializeField] private string m_keyB = "space";
    [SerializeField] private string m_keyC = "j";
    [SerializeField] private string m_keyD = "l";

    public override void DetectInput()
    {
        DetectInput_RightKey(m_rightKey_Up, m_rightKey_Down, m_rightKey_Right, m_rightKey_Left);
        DetectInput_LeftKey(m_leftKey_Up, m_leftKey_Down, m_leftKey_Right, m_leftKey_Left, m_keyA, m_keyB);
        m_keyAState = Input.GetKeyDown(m_keyA);
        m_keyBState = Input.GetKeyDown(m_keyB);
        m_keyCState = Input.GetKeyDown(m_keyC);
        m_keyDState = Input.GetKeyDown(m_keyD);
        KeyAPressing = Input.GetKey(m_keyA);
        KeyBPressing = Input.GetKey(m_keyB);
        KeyCPressing = Input.GetKey(m_keyC);
        KeyDPressing = Input.GetKey(m_keyD);
    }

    private void DetectInput_RightKey(string up, string down, string right, string left)
    {
        Vector2 _tempInput = TransferSquareViewToCircleView(new Vector2((Input.GetKey(right) ? 1f : 0f) - (Input.GetKey(left) ? 1f : 0f), (Input.GetKey(up) ? 1f : 0f) - (Input.GetKey(down) ? 1f : 0f)));

        m_rightKey_vertical = _tempInput.y;
        m_rightKey_horizontal = _tempInput.x;
    }

    private void DetectInput_LeftKey(string up, string down, string right, string left, string a, string b)
    {
        Vector2 _tempInput = TransferSquareViewToCircleView(new Vector2((Input.GetKey(right) ? 1f : 0f) - (Input.GetKey(left) ? 1f : 0f), (Input.GetKey(up) ? 1f : 0f) - (Input.GetKey(down) ? 1f : 0f)));

        m_leftKey_vertical = _tempInput.y;
        m_leftKey_horizontal = _tempInput.x;
    }

    // https://arxiv.org/ftp/arxiv/papers/1509/1509.06344.pdf
    private Vector2 TransferSquareViewToCircleView(Vector2 input)
    {
        Vector2 output = Vector2.zero;

        input.x = Mathf.Round(input.x);
        input.y = Mathf.Round(input.y);

        output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2f);
        output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2f);

        return output;
    }


}
