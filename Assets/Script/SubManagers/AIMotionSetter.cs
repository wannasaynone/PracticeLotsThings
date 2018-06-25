using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMotionSetter : SubManager {

    private CharacterListPage m_characterListPage;

    private float m_attackTime = 0.5f; // TEST
    private float m_attackTimer = 0.5f; // TEST

    public AIMotionSetter(CharacterListPage page) : base(page)
    {
        m_characterListPage = page;
    }

    public override void Update()
    {
        SetAiMotion();
    }

    private void SetAiMotion()
    {
        List<Character> _ai = m_characterListPage.Ai;
        Character _player = m_characterListPage.Player;
        InputDetecter_AI _inputDetecter_AI = null;
        for (int i = 0; i < _ai.Count; i++)
        {
            _inputDetecter_AI = _ai[i].Info.InputDetecter as InputDetecter_AI;

            if (Vector3.Distance(_ai[i].Info.Transform.position, _player.Info.Transform.position) > 1.8f) // TODO: avoid magic number
            {
                m_attackTimer = 0.5f;
                _inputDetecter_AI.SetMoveTo(_ai[i].Info.Transform.position, _player.Info.Transform.position);
            }
            else
            {
                _inputDetecter_AI.SetIdle();
                _ai[i].Info.Transform.LookAt(_player.Info.Transform);

                m_attackTimer -= Time.deltaTime;
                if(m_attackTimer < 0)
                {
                    _inputDetecter_AI.SetAttack();
                }
            }
        }
    }

}
