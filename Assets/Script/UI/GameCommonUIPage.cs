using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCommonUIPage : View {

    [SerializeField] private Image m_background = null;
    [SerializeField] private Text m_gameText = null;
    [SerializeField] private Button m_backTitleButton = null;

    public void Show(string content)
    {
        EnableBackground(true);
        EnableBackTitleButton(true, true);
        EnableGameText(true, content);
    }

    public void Hide()
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
        Engine.Instance.LoadScene("Title", null);
    }

}
