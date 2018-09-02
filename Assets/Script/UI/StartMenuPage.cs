using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuPage : View {

    [SerializeField] private SelectStartGameSettingPage m_selectStartGameSetttingPage = null;
    [SerializeField] private UnityEngine.UI.Text m_versionText = null;

    private void Start()
    {
        m_versionText.text = GameManager.GAME_VERSION.ToString();
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
