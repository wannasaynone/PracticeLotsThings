using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Gun : View {

    public static event Action<HitInfo> OnHit;

    public struct HitInfo
    {
        public Collider HitCollider;
        public Vector3 HitPosition;
    }

    public float FireCdTime { get { return m_fireCd; } }

    [SerializeField] private float m_fireCd = 0.1f;
    [SerializeField] private float m_fireExpandRange = 0.5f;
    [SerializeField] private Transform m_firePoint = null;
    [SerializeField] private LineRenderer m_gunLineTemplete = null;
    [SerializeField] private GameObject m_hitEffectTemplete = null;

    // TODO: fire particle

    public void Fire()
    {
        Vector3 _endPoint = m_firePoint.transform.position + m_firePoint.transform.forward * 8f + new Vector3(UnityEngine.Random.Range(-m_fireExpandRange, m_fireExpandRange), UnityEngine.Random.Range(-m_fireExpandRange, m_fireExpandRange), 0f);

        RaycastHit _hit = default(RaycastHit);

        if (Physics.Raycast(m_firePoint.position, _endPoint - m_firePoint.position, out _hit))
        {
            _endPoint = _hit.point;

            CreateHitEffect(_endPoint);
            if (OnHit != null)
            {
                OnHit(new HitInfo() { HitCollider = _hit.collider, HitPosition = _endPoint });
            }
        }

        CreateGunLine(m_firePoint.transform.position, _endPoint);
    }

    private void CreateHitEffect(Vector3 position)
    {
        GameObject _hitEffect = Instantiate(m_hitEffectTemplete);
        _hitEffect.transform.position = position;
        _hitEffect.transform.localScale = Vector3.one;

        TimerManager.Schedule(0.1f, delegate
        {
            Destroy(_hitEffect);
        });
    }

    private void CreateGunLine(Vector3 startPoint, Vector3 endPoint)
    {
        LineRenderer lineRenderer = Instantiate(m_gunLineTemplete);

        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.025f;
        lineRenderer.endWidth = 0.025f;

        lineRenderer.SetPosition(0, startPoint);
        lineRenderer.SetPosition(1, endPoint);

        TimerManager.Schedule(0.1f, delegate
        {
            Destroy(lineRenderer.gameObject);
        });
    }

}
