using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour {

	public static Camera MainCamera { get { return m_mainCamera; } }
    private static Camera m_mainCamera = null;

    public static CameraController MainCameraController { get { return m_mainCameraController; } }
    private static CameraController m_mainCameraController = null;

    public static GameObject MainCameraActor { get { return m_mainCameraActor; } }
    private static GameObject m_mainCameraActor = null;

    public int TrackingGameObjectInstanceID
    {
        get
        {
            if (m_trackingTarget != null)
            {
                return m_trackingTarget.GetInstanceID();
            }
            else
            {
                return -1;
            }
        }
    }

    [SerializeField] private float m_rotateSpeed = 0f;

    private GameObject m_trackingTarget = null;

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

        if (m_mainCameraActor != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            if(transform.parent != null)
            {
                m_mainCameraActor = transform.parent.gameObject;
            }
            else
            {
                GameObject _parent = new GameObject("CameraActor");
                _parent.transform.position = Vector3.zero;
                _parent.transform.localScale = Vector3.zero;
                transform.SetParent(_parent.transform);
                m_mainCameraActor = _parent;
            }
        }
    }

    private void FixedUpdate()
    {
        if (m_trackingTarget != null && Vector3.Distance(m_mainCameraActor.transform.position, m_trackingTarget.transform.position) > 0.1f)
        {
            m_mainCameraActor.transform.DOMove(m_trackingTarget.transform.position, 0.5f);
        }
    }

    public void SetPosition(Vector3 position)
    {
        m_trackingTarget = null;
        m_mainCameraActor.transform.DOMove(position, 0.5f);
    }

    public void AddPosition(Vector3 additive)
    {
        m_trackingTarget = null;
        m_mainCameraActor.transform.DOMove(m_mainCameraActor.transform.position + additive, 0.5f);
    }

    public void Rotate(bool toRight)
    {
        if(toRight)
        {
            m_mainCameraActor.transform.Rotate(new Vector3(0f, m_rotateSpeed, 0f));
        }
        else
        {
            m_mainCameraActor.transform.Rotate(new Vector3(0f, -m_rotateSpeed, 0f));
        }
    }

    public void Track(GameObject target)
    {
        SetPosition(target.transform.position);
        m_mainCameraActor.transform.position = target.transform.position;
        m_trackingTarget = target;
    }

    public void StopTrack()
    {
        m_trackingTarget = null;
    }

}
