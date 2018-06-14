using System;
using UnityEngine;

public class CharacterStatus : Page {

    public int HP { get { return m_hp; } }
    public int Attack { get { return m_atk; } }
    public int Defense { get { return m_def; } }
    public int Material { get { return m_mat; } }

    private int m_hp = 100;
    private int m_atk = 100;
    private int m_def = 90;
    private int m_mat = 0;

    public event Action<int> OnHpBaseValueChanged;
    public event Action<int> OnAttackBaseValueChanged;
    public event Action<int> OnDefenseBaseValueChanged;
    public event Action<int> OnMaterialBaseValueChanged;

    public void SetHp(int value)
    {
        Debug.Log("Display HP=" + value);
        m_hp = value;
        if (OnHpBaseValueChanged != null)
        {
            OnHpBaseValueChanged(value);
        }
    }

    public void SetAtk(int value)
    {
        Debug.Log("Display Atk=" + value);
        m_atk = value;
        if (OnAttackBaseValueChanged != null)
        {
            OnAttackBaseValueChanged(value);
        }
    }

    public void SetDef(int value)
    {
        Debug.Log("Display Def=" + value);
        m_def = value;
        if (OnDefenseBaseValueChanged != null)
        {
            OnDefenseBaseValueChanged(value);
        }
    }

    public void SetMat(int value)
    {
        Debug.Log("Display Mat=" + value);
        m_mat = value;
        if (OnMaterialBaseValueChanged != null)
        {
            OnMaterialBaseValueChanged(value);
        }
    }

    public void AddHp(int value)
    {
        Debug.Log("Add HP=" + value);
        SetHp(m_hp + value);
    }

    public void AddAtk(int value)
    {
        Debug.Log("Add Atk=" + value);
        SetAtk(m_atk + value);
    }

    public void AddDef(int value)
    {
        Debug.Log("Add Def=" + value);
        SetDef(m_mat + value);
    }

    public void AddMat(int value)
    {
        Debug.Log("Add Mat=" + value);
        SetMat(m_mat + value);
    }

}
