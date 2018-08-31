using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuPage : View {

    [SerializeField] private SelectStartGameSettingPage m_selectStartGameSetttingPage = null;

    private void Start()
    {
        if(!NetworkManager.IsOffline)
        {
            gameObject.SetActive(false);
            m_selectStartGameSetttingPage.ShowSetCharacterMenu();
        }
    }

    public void StartGame()
    {
        gameObject.SetActive(false);
        m_selectStartGameSetttingPage.ShowMainGameMenu();
    }

}
