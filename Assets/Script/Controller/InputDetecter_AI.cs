using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Controller/AI"))]
public class InputDetecter_AI : InputDetecter
{
    private Vector3 m_start = default(Vector3);
    private Vector3 m_end = default(Vector3);

    public override void DetectInput()
    {
        if(m_start.x > m_end.x)
        {
            m_leftKey_horizontal = -2f;
        }
        else
        {
            m_leftKey_horizontal = 2f;
        }

        if (m_start.z > m_end.z)
        {
            m_leftKey_vertical = -2f;
        }
        else
        {
            m_leftKey_vertical = 2f;
        }
    }

    public void SetMoveTo(Vector3 start, Vector3 end)
    {
        m_start = start;
        m_end = end;
    }

}
