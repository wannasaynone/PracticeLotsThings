using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Page {

    [SerializeField] private Rigidbody m_bullet;
    [SerializeField] private Transform m_firePosition;
    [SerializeField] private float m_fireForce;
    [SerializeField] private float m_fireCdTimeSet;

    public int Camp { get { return m_camp; } }

    private ActorController m_creator = null;
    private GameObject m_currentTarget = null;
    private int m_camp = -1;

    private float m_fireCdTimer = 0f;

    private void Start()
    {
        m_fireCdTimer = m_fireCdTimeSet;
    }

    public void SetCreator(ActorController actor)
    {
        m_creator = actor;
    }

    public void SetCamp(int value)
    {
        m_camp = value;
    }

    public void Fire()
    {
        Rigidbody _bullet = Instantiate(m_bullet);
        // TODO: refactor here: try not to use GetComponentInChildren
        _bullet.GetComponentInChildren<HitBox>().SetBelongsTo(m_creator);
        _bullet.transform.position = m_firePosition.position;
        _bullet.AddForce(transform.forward * m_fireForce);
    }

    public void SetTarget(GameObject target)
    {
        m_currentTarget = target;
    }

    private void Update()
    {
        if (m_currentTarget == null)
        {
            return;
        }

        Vector3 _targetDir = new Vector3(m_currentTarget.transform.position.x, 0, m_currentTarget.transform.position.z) - new Vector3(transform.position.x, 0, transform.position.z);

        Debug.DrawLine(new Vector3(m_currentTarget.transform.position.x, 0, m_currentTarget.transform.position.z), new Vector3(transform.position.x, 0, transform.position.z), Color.red);

        Vector3 _cross = Vector3.Cross(_targetDir, new Vector3(transform.forward.x, 0, transform.forward.z));
        float _angle = Vector3.Angle(_targetDir, new Vector3(transform.forward.x, 0, transform.forward.z));
        if (_angle > 5.0f)
        {
            // Debug.Log(string.Format("_targetDir={0}, transform.forward={1}", _targetDir, transform.forward));
            if(_cross.y < 0)
            {
                transform.Rotate(new Vector3(0, 4f, 0));
            }
            else
            {
                transform.Rotate(new Vector3(0, -4f, 0));
            }
        }

        if(m_fireCdTimer > 0)
        {
            m_fireCdTimer -= Time.deltaTime;
            if(m_fireCdTimer <= 0)
            {
                Fire();
                m_fireCdTimer = m_fireCdTimeSet;
            }
        }

    }

}
