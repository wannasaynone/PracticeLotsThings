using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : View {

    [SerializeField] protected Actor m_actor = null;
    [SerializeField] protected InputDetecter m_inputDetecter = null;

    protected Vector3 m_mousePositionOnStage = default(Vector3);

    protected virtual void Update()
    {
        m_inputDetecter.Update();
        m_actor.SetMotion(m_inputDetecter.Horizontal, m_inputDetecter.Vertical, m_inputDetecter.Horizontal != 0f || m_inputDetecter.Vertical != 0f ? 1f : 0f);
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
            m_mousePositionOnStage.y = 0f;
        }

        Debug.DrawLine(transform.position, m_mousePositionOnStage);
    }

}
