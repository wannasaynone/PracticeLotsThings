using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : View {

    [SerializeField] protected Actor m_actor = null;
    [SerializeField] protected InputDetecter m_inputDetecter = null;
    [SerializeField] protected float m_fixedMousePositionZ = 0.1f;

    protected Vector3 m_mousePositionOnStage = default(Vector3);

    protected virtual void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            CameraController.MainCameraController.Track(gameObject);
        }

        m_inputDetecter.Update();

        if(CameraController.MainCameraActor != null)
        {
            m_actor.SetMotion
                (
                m_inputDetecter.Horizontal * CameraController.MainCameraActor.transform.right + m_inputDetecter.Vertical * CameraController.MainCameraActor.transform.forward, 
                m_inputDetecter.Horizontal != 0f || m_inputDetecter.Vertical != 0f ? 1f : 0f
                );
        }
        else
        {
            m_actor.SetMotion
                (
                m_inputDetecter.Horizontal, 
                m_inputDetecter.Vertical, 
                m_inputDetecter.Horizontal != 0f || m_inputDetecter.Vertical != 0f ? 1f : 0f
                );
        }

        if(CameraController.MainCameraController.TrackingGameObjectInstanceID == gameObject.GetInstanceID())
        {
            if(m_inputDetecter.IsRotateingCameraRight)
            {
                CameraController.MainCameraController.Rotate(true);
            }

            if(m_inputDetecter.IsRotateingCameraLeft)
            {
                CameraController.MainCameraController.Rotate(false);
            }
        }

        ParseMousePositionToStage();
        m_actor.FaceTo(m_mousePositionOnStage);
    }

    private void ParseMousePositionToStage()
    {
        Ray _camRay = CameraController.MainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit _hit;

        if (Physics.Raycast(_camRay, out _hit, 100f, LayerMask.GetMask("Ground")))
        {
            m_mousePositionOnStage = _hit.point;
            m_mousePositionOnStage += CameraController.MainCameraActor.transform.forward * m_fixedMousePositionZ;
        }

        Debug.DrawLine(transform.position, m_mousePositionOnStage);
    }

}
