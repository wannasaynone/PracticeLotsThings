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
        SetAiDirection();
    }

    private void SetAiDirection()
    {
        List<Character> _ai = m_characterListPage.Ai;
        Character _player = m_characterListPage.Player;
        for(int i = 0; i < _ai.Count; i++)
        {
            InputDetecter_AI _inputDetecter_AI = _ai[i].Info.InputDetecter as InputDetecter_AI;
            _inputDetecter_AI.SetMoveTo(_ai[i].Info.Transform.position, _player.Info.Transform.position);
        }
    }

}
