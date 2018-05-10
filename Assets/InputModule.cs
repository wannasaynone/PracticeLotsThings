using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputModule : MonoBehaviour {

    private static bool INPUT_ENABLE = true;

	[SerializeField] private string m_keyUp = "w";
	[SerializeField] private string m_keyDown = "s";
	[SerializeField] private string m_keyRight = "d";
	[SerializeField] private string m_keyLeft = "a";
    [SerializeField] private float m_moveSmoothTime = 0.1f;

	private float m_direction_Vertical = 0f;
	private float m_direction_Horizontal = 0f;

    private float m_target_direction_Vertical = 0f;
    private float m_target_direction_Horizontal = 0f;
    private float m_velocity_direction_Vertical = 0f;
    private float m_velocity_direction_Horizontal = 0f;

    private void Update()
	{
        DetectInput();
    }

    private void DetectInput()
    {
        // 未來有可能會根據情況，讓玩家不能輸入，但還是要做角色移動(EX 過場)
        if (INPUT_ENABLE)
        {
            float Vertical = (Input.GetKey(m_keyUp) ? 1f : 0f) - (Input.GetKey(m_keyDown) ? 1f : 0f);
            float Horizontal = (Input.GetKey(m_keyRight) ? 1f : 0f) - (Input.GetKey(m_keyLeft) ? 1f : 0f);
            SetDirection(Vertical, Horizontal);
        }
    }

    private void SetDirection(float vertical, float horizontal)
    {
        // "前後左右"會根據狀況改變，所以不做"位移"(Vector3.forward * speed...之類的)，先做玩家輸入的方向判斷
        vertical = Mathf.Clamp(vertical, -1f, 1f);
        horizontal = Mathf.Clamp(horizontal, -1f, 1f);

        m_target_direction_Vertical = vertical;
        m_target_direction_Horizontal = horizontal;

        // 避免瞬間變值導致詭異的角色移動表現，用SmoothDamp製造類似遞增遞減的效果
        m_direction_Vertical = Mathf.SmoothDamp(m_direction_Vertical, m_target_direction_Vertical, ref m_velocity_direction_Vertical, m_moveSmoothTime);
        m_direction_Horizontal = Mathf.SmoothDamp(m_direction_Horizontal, m_target_direction_Horizontal, ref m_velocity_direction_Horizontal, m_moveSmoothTime);
    }

}
