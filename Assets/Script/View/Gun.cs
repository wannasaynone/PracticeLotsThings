using System.Collections.Generic;
using UnityEngine;
using PracticeLotsThings.Manager;

namespace PracticeLotsThings.View
{
    public class Gun : View
    {
        public float FireCdTime { get { return m_fireCd; } }

        [SerializeField] private float m_fireCd = 0.1f;
        [SerializeField] private float m_fireExpandRange = 0.5f;
        [SerializeField] private float m_fireLength = 10f;
        [SerializeField] private int m_damage = 0;
        [SerializeField] private Vector3 m_endPointAdjustion = default(Vector3);
        [SerializeField] private Transform m_firePoint = null;
        [SerializeField] private LineRenderer m_gunLineTemplete = null;
        [SerializeField] private GameObject m_hitEffectTemplete = null;

        private List<LineRenderer> m_gunLinePool = new List<LineRenderer>();
        // TODO: fire particle

        public Vector3 Fire()
        {
            Vector3 _endPoint = m_firePoint.transform.position + m_firePoint.transform.forward * m_fireLength + m_endPointAdjustion + new Vector3(UnityEngine.Random.Range(-m_fireExpandRange, m_fireExpandRange), UnityEngine.Random.Range(-m_fireExpandRange, m_fireExpandRange), 0f);
            return FireToSpecificPoint(_endPoint);
        }

        public Vector3 FireToSpecificPoint(Vector3 point)
        {
            Vector3 _point = Vector3.zero;
            _point = point;

            RaycastHit _hit = default(RaycastHit);

            if (Physics.Raycast(m_firePoint.position, _point - m_firePoint.position, out _hit, 10f, LayerMask.GetMask("Character")))
            {
                _point = _hit.point;

                CreateHitEffect(_point);
                if (EventManager.OnHit != null)
                {
                    EventManager.OnHit(new EventManager.HitInfo()
                    {
                        actorType = ActorFilter.ActorType.Shooter,
                        HitCollider = _hit.collider,
                        HitPosition = _point,
                        Damage = m_damage
                    });
                }
            }

            CreateGunLine(m_firePoint.transform.position, _point);

            return _point;
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
            for (int i = 0; i < m_gunLinePool.Count; i++)
            {
                if (!m_gunLinePool[i].gameObject.activeSelf)
                {
                    SetGunLine(m_gunLinePool[i], startPoint, endPoint);
                    return;
                }
            }

            LineRenderer _lineRendererClone = Instantiate(m_gunLineTemplete);
            m_gunLinePool.Add(_lineRendererClone);
            SetGunLine(_lineRendererClone, startPoint, endPoint);
        }

        private void SetGunLine(LineRenderer lineRenderer, Vector3 startPoint, Vector3 endPoint)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.startWidth = 0.025f;
            lineRenderer.endWidth = 0.025f;

            lineRenderer.SetPosition(0, startPoint);
            lineRenderer.SetPosition(1, endPoint);

            lineRenderer.gameObject.SetActive(true);

            TimerManager.Schedule(0.1f, delegate
            {
                if (this != null && lineRenderer.gameObject != null)
                {
                    lineRenderer.gameObject.SetActive(false);
                }
            });
        }
    }
}
