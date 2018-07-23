using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour {

	public static Camera MainCamera { get { return m_mainCamera; } }
    private static Camera m_mainCamera = null;

    public static CameraController MainCameraController { get { return m_mainCameraController; } }
    private static CameraController m_mainCameraController = null;

    private Vector3 m_cameraTargetPosition = default(Vector3);

    private void Awake()
    {
        if(m_mainCamera != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            m_mainCamera = GetComponent<Camera>();
        }

        if(m_mainCameraController != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            m_mainCameraController = this;
        }

        m_cameraTargetPosition = transform.position;
    }

    private void FixedUpdate()
    {
        transform.DOMove(m_cameraTargetPosition, 0.5f);
    }

    public void SetPosition(Vector3 position)
    {
        m_cameraTargetPosition = position;
    }

    public void AddPosition(Vector3 additive)
    {
        m_cameraTargetPosition += additive;
    }

}
