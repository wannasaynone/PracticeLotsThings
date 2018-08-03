﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : View {

    [SerializeField] private Actor m_belongsTo = null;
    [SerializeField] private GameObject m_hitEffectTemplete = null;

    private void OnTriggerEnter(Collider other)
    {
        if (EventManager.OnHit != null && m_belongsTo.IsAttacking)
        {
            EventManager.OnHit(new EventManager.HitInfo() { actorType = ActorFilter.ActorType.Zombie, HitCollider = other, HitPosition = transform.position, Damage = 200 });
            CreateHitEffect(transform.position);
        }
    }

    private void CreateHitEffect(Vector3 position)
    {
        GameObject _hitEffect = Instantiate(m_hitEffectTemplete);
        _hitEffect.transform.position = position;
        _hitEffect.transform.localScale = Vector3.one;

        TimerManager.Schedule(1f, delegate
        {
            Destroy(_hitEffect);
        });
    }

}
