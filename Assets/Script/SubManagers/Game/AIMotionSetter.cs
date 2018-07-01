using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMotionSetter : SubManager {

    private CharacterListPage m_characterListPage;

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
        List<Character> _ai = m_characterListPage.AI;
        Character _player = m_characterListPage.Player;
        for (int i = 0; i < _ai.Count; i++)
        {
            SetAI(_ai[i], _player);
        }
    }

    private void SetAI(Character ai, Character player)
    {
        InputDetecter_AI _inputDetecter_AI = null;

        _inputDetecter_AI = ai.Actor.InputDetecter as InputDetecter_AI;

        if (Vector3.Distance(ai.Actor.transform.position, player.Actor.transform.position) > 1.8f) // TODO: avoid magic number 
        {
            _inputDetecter_AI.SetMoveTo(ai.Actor.transform.position, player.Actor.transform.position);
        }
        else
        {
            _inputDetecter_AI.SetIdle();
        }
    }

}
