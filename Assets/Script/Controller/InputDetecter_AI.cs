using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Controller/AI"))]
public class InputDetecter_AI : InputDetecter
{
    private float m_timer = 5f;
    private float m_setAITime = 5f;

    public override void DetectInput()
    {
        m_timer -= Time.deltaTime;
        if(m_timer <= 0)
        {
            m_leftKey_vertical = Random.Range(-2f, 2f);
            m_leftKey_horizontal = Random.Range(-2f, 2f);
            m_timer = m_setAITime;
        }
    }
}
