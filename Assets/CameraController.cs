using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField] private InputModule m_input;
    [SerializeField] private GameObject m_playerHandle;
    [SerializeField] private GameObject m_cameraHandle;
    [SerializeField] private ActorController m_actorController;
    [SerializeField] private float m_rotateSpeed_horizontal = 100f;
    [SerializeField] private float m_rotateSpeed_vertical = 80f;
    [SerializeField] private float m_vertical_minDegree = -5f;
    [SerializeField] private float m_vertical_maxDegree = 30f;

    float tempEulerAngles_x = 12f;
    private void Update()
    {
        DoHorizontalRotate();
        DoVerticalRotate();
    }

    private void DoHorizontalRotate()
    {
        if(Mathf.Abs(m_input.JoyStick_Horizontal) > 0)
        {
            Vector3 tempModelEulerAngle = m_actorController.Model.transform.eulerAngles;
            m_playerHandle.transform.Rotate(Vector3.up, m_input.JoyStick_Horizontal * m_rotateSpeed_horizontal * Time.deltaTime);
            m_actorController.Model.transform.eulerAngles = tempModelEulerAngle;
        }
    }

    private void DoVerticalRotate()
    {
        if(Mathf.Abs(m_input.JoyStick_Vertical) > 0)
        {
            tempEulerAngles_x += m_input.JoyStick_Vertical * -m_rotateSpeed_vertical * Time.deltaTime;
            tempEulerAngles_x = Mathf.Clamp(tempEulerAngles_x, m_vertical_minDegree, m_vertical_maxDegree);
            m_cameraHandle.transform.localEulerAngles = new Vector3(tempEulerAngles_x, 0f, 0f);
        }
    }

}
