using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIActor : Actor {

    private Vector3 m_goal;

    public void SetMoveTo(Vector3 goal)
    {
        m_goal = goal;
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, m_goal) > 0.5f)
        {
            Vector3 _dir = new Vector3((m_goal.x - transform.position.x), 0f, (m_goal.z - transform.position.z));
            transform.position += _dir.normalized * m_speed;
        }
    }

}
