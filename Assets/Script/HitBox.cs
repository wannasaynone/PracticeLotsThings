using System;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour {

    public class HitEventArgs : EventArgs
    {
        public ActorController Attacker { get; private set; }
        public ActorController Defender { get; private set; }
        
        public HitEventArgs(ActorController attacker, ActorController defender)
        {
            Attacker = attacker;
            Defender = defender;
        }
    }

    public static Dictionary<ActorController, List<HitBox>> HitBoxes
    {
        get
        {
            if(m_hitBoxes == null)
            {
                return new Dictionary<ActorController, List<HitBox>>();
            }
            else
            {
                return new Dictionary<ActorController, List<HitBox>>(m_hitBoxes);
            }
        }
    }
    private static Dictionary<ActorController, List<HitBox>> m_hitBoxes;

    public static event EventHandler<HitEventArgs> OnHitOthers;

    [SerializeField] private ActorController m_belongsActorController;

    private void OnEnable()
    {
        if(m_hitBoxes == null)
        {
            m_hitBoxes = new Dictionary<ActorController, List<HitBox>>();
        }

        if(m_belongsActorController == null)
        {
            Debug.LogError(gameObject.name + "'s ActorController is null");
            return;
        }

        if(m_hitBoxes.ContainsKey(m_belongsActorController))
        {
            if(m_hitBoxes[m_belongsActorController] == null)
            {
                m_hitBoxes[m_belongsActorController] = new List<HitBox>();
            }

            m_hitBoxes[m_belongsActorController].Add(this);
        }
        else
        {
            m_hitBoxes.Add(m_belongsActorController, new List<HitBox>() { this });
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Character"))
        {
            if(OnHitOthers != null)
            {
                ActorController _defender = new List<ActorController>(m_hitBoxes.Keys).Find(x => x.ModelCollider == collision);
                if(_defender == null)
                {
                    Debug.LogError("_defender is null when getting ActorController from m_hitBoxes");
                }
                OnHitOthers.Invoke(this, new HitEventArgs(m_belongsActorController, _defender));
            }
        }
    }

    private void OnDisable()
    {
        if (m_hitBoxes.ContainsKey(m_belongsActorController))
        {
            if (m_hitBoxes[m_belongsActorController] == null)
            {
                return;
            }
            else
            {
                m_hitBoxes[m_belongsActorController].Remove(this);
            }
        }
    }

    private void OnDestroy()
    {
        OnDisable();
    }

}
