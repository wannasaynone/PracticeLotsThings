using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using DG.Tweening;

public class CameraController : MonoBehaviour {

    [SerializeField] private Camera m_camera;
    [SerializeField] private InputModule m_input;
    [SerializeField] private GameObject m_playerHandle;
    [SerializeField] private GameObject m_cameraHandle;
    [SerializeField] private ActorController m_actorController;
    [SerializeField] private float m_rotateSpeed_horizontal = 100f;
    [SerializeField] private float m_rotateSpeed_vertical = 80f;
    [SerializeField] private float m_vertical_minDegree = -5f;
    [SerializeField] private float m_vertical_maxDegree = 30f;
    [SerializeField] private float m_syncCameraSpeed = 0.2f;

    private float m_cameraHandleTempEulerAngles_x = 12f;
    private Vector3 m_cameraDampVelocity = Vector3.zero;

    private void Update()
    {
        DoHorizontalRotate();
        DoVerticalRotate();
    }

    private void FixedUpdate()
    {
        DoSyncCamera();
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
            m_cameraHandleTempEulerAngles_x += m_input.JoyStick_Vertical * -m_rotateSpeed_vertical * Time.deltaTime;
            m_cameraHandleTempEulerAngles_x = Mathf.Clamp(m_cameraHandleTempEulerAngles_x, m_vertical_minDegree, m_vertical_maxDegree);
            m_cameraHandle.transform.localEulerAngles = new Vector3(m_cameraHandleTempEulerAngles_x, 0f, 0f);
        }
    }

    private void DoSyncCamera()
    {
        m_camera.transform.position = Vector3.SmoothDamp(m_camera.transform.position, transform.position, ref m_cameraDampVelocity, m_syncCameraSpeed);
        m_camera.transform.rotation = transform.rotation;
    }

}
