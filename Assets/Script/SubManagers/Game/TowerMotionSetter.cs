using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerMotionSetter : SubManager {

    private CharacterListPage m_characterListPage;

    public TowerMotionSetter(CharacterListPage page) : base(page)
    {
        m_characterListPage = page;
    }

    private List<Character> m_allCharacters = null;
    private List<Tower> m_towers = null;
    public override void Update()
    {
        m_allCharacters = m_characterListPage.Characters;

        for (int _characterIndex = 0; _characterIndex < m_allCharacters.Count; _characterIndex++)
        {
            m_towers = m_allCharacters[_characterIndex].Actor.OwnTowers;

            for (int _towerIndex = 0; _towerIndex < m_towers.Count; _towerIndex++)
            {
                Character _target = m_characterListPage.GetCloestCharacter(m_towers[_towerIndex], false);

                if (_target != null)
                {
                    Vector3 _targetPos = _target.Actor.transform.position;
                    m_towers[_towerIndex].SetTargerPosition(_targetPos);
                }
            }
        }
    }

}
