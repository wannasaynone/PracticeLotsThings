using System;
using UnityEngine;
using UnityEngine.UI;

public class GameCommonUIPage : View {

    [SerializeField] private Image m_background = null;
    [SerializeField] private Text m_gameText = null;
    [SerializeField] private Button m_backTitleButton = null;
    [SerializeField] private Text m_hintText = null;

    private Action m_onBackToTittle = null;

    public void ShowGameOverMenu(string content, Action onBackToTittle)
    {
        if(m_onBackToTittle != null)
        {
            Debug.LogError("Unexcepted Error: GameCommonUIPage.OnClickBack != null");
            return;
        }

        m_hintText.gameObject.SetActive(false);
        EnableBackground(true);
        EnableBackTitleButton(true, true);
        EnableGameText(true, content);
        m_onBackToTittle = onBackToTittle;
    }

    public void HideAll()
    {
        EnableBackground(false);
        EnableBackTitleButton(false, false);
        EnableGameText(false, "");
    }

    public void EnableGameText(bool enable, string content)
    {
        m_gameText.text = content;
        m_gameText.gameObject.SetActive(enable);
    }

    public void EnableBackground(bool enable)
    {
        m_background.gameObject.SetActive(enable);
    }

    public void EnableBackTitleButton(bool enable, bool interactable)
    {
        m_backTitleButton.gameObject.SetActive(enable);
        m_backTitleButton.interactable = interactable;
    }

    public void UI_Button_BackToTitle()
    {
        if (!NetworkManager.IsOffline)
        {
            PhotonEventReceiver.OnRoomLeft += OnLeftRoom;
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            OnLeftRoom();
        }
    }

    private void OnLeftRoom()
    {
        if (!NetworkManager.IsOffline)
        {
            PhotonEventReceiver.OnRoomLeft -= OnLeftRoom;
        }

        Engine.Instance.LoadScene("Title", OnBackToTittle);
    }

    private void OnBackToTittle()
    {
        m_onBackToTittle();
        m_onBackToTittle = null;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            m_hintText.gameObject.SetActive(false);
        }
    }

}
