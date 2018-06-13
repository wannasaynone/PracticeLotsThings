using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour {

    public static List<HitBox> HitBoxes
    {
        get
        {
            if(m_hitBoxes == null)
            {
                return new List<HitBox>();
            }
            else
            {
                return new List<HitBox>(m_hitBoxes);
            }
        }
    }
    private static List<HitBox> m_hitBoxes;

    private void OnEnable()
    {
        if(m_hitBoxes == null)
        {
            m_hitBoxes = new List<HitBox>();
        }

        if(!m_hitBoxes.Contains(this))
        {
            m_hitBoxes.Add(this);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Character"))
        {
            Debug.Log("Hit " + collision.gameObject.name);
        }
    }

    private void OnDisable()
    {
        if (m_hitBoxes.Contains(this))
        {
            m_hitBoxes.Remove(this);
        }
    }

    private void OnDestroy()
    {
        if (m_hitBoxes.Contains(this))
        {
            m_hitBoxes.Remove(this);
        }
    }

}
