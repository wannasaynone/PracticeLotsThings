using System;
using UnityEngine;

public class CharacterStatus {

    public event Action<int> OnHpBaseValueChanged;
    public event Action<int> OnAttackBaseValueChanged;
    public event Action<int> OnDefenseBaseValueChanged;
    public event Action<int> OnMaterialBaseValueChanged;

    public int HP
    {
        get
        {
            return m_hp;
        }
        set
        {
            SetBaseValue(ref m_hp, value, OnHpBaseValueChanged, m_characterStatusDisplayer.SetHp);
        }
    }
    public int Attack
    {
        get
        {
            return m_atk;
        }
        set
        {
            SetBaseValue(ref m_atk, value, OnAttackBaseValueChanged, m_characterStatusDisplayer.SetAtk);
        }
    }
    public int Defense
    {
        get
        {
            return m_def;
        }
        set
        {
            SetBaseValue(ref m_def, value, OnDefenseBaseValueChanged, m_characterStatusDisplayer.SetDef);
        }
    }
    public int Material
    {
        get
        {
            return m_mat;
        }
        set
        {
            SetBaseValue(ref m_mat, value, OnMaterialBaseValueChanged, m_characterStatusDisplayer.SetMat);
        }
    }

    private void SetBaseValue(ref int para, int value, Action<int> eventHandler, Action<int> pageSetter)
    {
        para = value;
        if (eventHandler != null)
        {
            eventHandler(value);
        }
        if(pageSetter != null)
        {
            pageSetter(value);
        }
    }

    private int m_hp = 0;
    private int m_atk = 0;
    private int m_def = 0;
    private int m_mat = 0;

    private CharacterStatusDisplayer m_characterStatusDisplayer;

    public CharacterStatus(CharacterStatusDisplayer characterStatusDisplayer)
    {
        m_characterStatusDisplayer = characterStatusDisplayer;

        // TEST
        m_atk = 100;
        m_def = 90;
        m_hp = 100;
        m_mat = 0;
    }

}
