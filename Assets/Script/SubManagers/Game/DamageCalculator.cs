using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCalculator : SubManager {

    private CombatPage m_combatPage;

	public DamageCalculator(CombatPage combatPage) : base(combatPage)
    {
        m_combatPage = combatPage;
        HitBox.OnHitOthers += OnHitBoxHit;
    }

    public override void Update()
    {
        return;
    }

    private void OnHitBoxHit(object sender, HitBox.HitEventArgs e)
    {
        int _dmg = e.Attacker.CharacterStatus.Attack - e.Defender.CharacterStatus.Defense;
        m_combatPage.SetDamage(_dmg);
        e.Defender.CharacterStatus.AddHp(-_dmg);
    }

}
