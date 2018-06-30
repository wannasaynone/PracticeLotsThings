using System;
using UnityEngine;

public class CharacterStatusPage : Page {

    public int HP { get { return m_hp; } }
    public int Attack { get { return m_atk; } }
    public int Defense { get { return m_def; } }
    public int Material { get { return m_mat; } }
    public int Camp { get { return m_camp; } }

    private int m_hp = 100;
    private int m_atk = 100;
    private int m_def = 90;
    private int m_mat = 0;
    private int m_camp = -1;

    public event Action<int> OnHpValueChanged;
    public event Action<int> OnAttackValueChanged;
    public event Action<int> OnDefenseValueChanged;
    public event Action<int> OnMaterialValueChanged;
    public event Action<int> OnCampChanged;

    public void SetHp(int value)
    {
        Debug.Log("Display HP=" + value);
        m_hp = value;
        if (OnHpValueChanged != null)
        {
            OnHpValueChanged(value);
        }
    }

    public void SetAtk(int value)
    {
        Debug.Log("Display Atk=" + value);
        m_atk = value;
        if (OnAttackValueChanged != null)
        {
            OnAttackValueChanged(value);
        }
    }

    public void SetDef(int value)
    {
        Debug.Log("Display Def=" + value);
        m_def = value;
        if (OnDefenseValueChanged != null)
        {
            OnDefenseValueChanged(value);
        }
    }

    public void SetMat(int value)
    {
        Debug.Log("Display Mat=" + value);
        m_mat = value;
        if (OnMaterialValueChanged != null)
        {
            OnMaterialValueChanged(value);
        }
    }

    public void SetCamp(int value)
    {
        m_camp = value;
        if(OnCampChanged != null)
        {
            OnCampChanged(value);
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
