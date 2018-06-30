using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Page {

    [SerializeField] Rigidbody m_bullet;
    [SerializeField] Transform m_firePosition;
    [SerializeField] float m_fireForce;

    public int Camp { get { return m_camp; } }
    private Vector3 m_currentTargetPosition = default(Vector3);
    private int m_camp = -1;

    public void SetCamp(int value)
    {
        m_camp = value;
    }

    public void Fire()
    {
        Rigidbody _bullet = Instantiate(m_bullet);
        _bullet.transform.position = m_firePosition.position;
        _bullet.AddForce(transform.forward * m_fireForce);
    }

    public void SetTargerPosition(Vector3 position)
    {
        m_currentTargetPosition = position;
    }

    private void Update()
    {
        Vector3 _targetDir = new Vector3(m_currentTargetPosition.x, 0, m_currentTargetPosition.z) - new Vector3(transform.position.x, 0, transform.position.z);
        float _angle = Vector3.Angle(_targetDir, new Vector3(transform.forward.x, 0, transform.forward.z));
        if (_angle > 5.0f)
        {
            if(_targetDir.x > transform.forward.x)
            {
                transform.localEulerAngles += new Vector3(0, 1f, 0);
            }
            else
            {
                transform.localEulerAngles -= new Vector3(0, 1f, 0);
            }
        }
    }

}
