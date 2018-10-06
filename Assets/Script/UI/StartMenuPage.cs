using UnityEngine;
using PracticeLotsThings.Manager;
using PracticeLotsThings.MainGameMonoBehaviour;

namespace PracticeLotsThings.View.UI
{
    public class StartMenuPage : View
    {
        [SerializeField] private SelectStartGameSettingPage m_selectStartGameSetttingPage = null;
        [SerializeField] private UnityEngine.UI.Text m_versionText = null;

        private void Start()
        {
            m_versionText.text = Engine.GameVersion;
            if(Engine.Instance.Inited)
            {
                ShowStartScreen();
            }
            else
            {
                Engine.OnEngineInited += ShowStartScreen;
            }
        }

        private void ShowStartScreen()
        {
            Engine.OnEngineInited -= ShowStartScreen;
            if (!NetworkManager.IsOffline)
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
}
