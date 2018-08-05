using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectStartGameSettingPage : View {

    [SerializeField] private GameObject m_defaultRoot = null;
    [Header("Select Play As")]
    [SerializeField] private Text m_selectPlayAsTipText = null;
    [Header("Select Game Type")]
    [SerializeField] private Text m_selectGameTypeTipText = null;

    private GameObject m_previous = null;
    private GameObject m_current = null;

    private ActorFilter.ActorType m_startAs = ActorFilter.ActorType.Shooter;
    private NewGameSetting.GameType m_gameType = NewGameSetting.GameType.OneVsOne;
    private int m_normalActorNumber = 50;

    private void Start()
    {
        m_current = m_defaultRoot;
        m_defaultRoot.SetActive(true);
    }

    public void SelectShooter()
    {
        // TODO: localiztion
        m_selectPlayAsTipText.text = "Play As Shooter";
        m_startAs = ActorFilter.ActorType.Shooter;
    }

    public void SelectZombie()
    {
        // TODO: localiztion
        m_selectPlayAsTipText.text = "Play As Zombie";
        m_startAs = ActorFilter.ActorType.Zombie;
    }

    public void Select1V1()
    {
        // TODO: localiztion
        m_selectGameTypeTipText.text = "Game Type: 1 VS 1";
        m_gameType = NewGameSetting.GameType.OneVsOne;
    }

    public void Select2V2()
    {
        // TODO: localiztion
        m_selectGameTypeTipText.text = "Game Type: 2 VS 2";
        m_gameType = NewGameSetting.GameType.TwoVsTwo;
    }

    public void GoNext(GameObject next)
    {
        m_previous = m_current;
        m_current = next;
        m_previous.SetActive(false);
        m_current.SetActive(true);
    }

    public void GoBack()
    {
        GoNext(m_previous);
    }

    public void StartGame()
    {
        Engine.NewGameSetting = ScriptableObject.CreateInstance<NewGameSetting>();
        Engine.NewGameSetting.startAs = m_startAs;
        Engine.NewGameSetting.gameType = m_gameType;
        Engine.NewGameSetting.normalActorNumber = m_normalActorNumber;
        Engine.Instance.StartGame();
    }

}
