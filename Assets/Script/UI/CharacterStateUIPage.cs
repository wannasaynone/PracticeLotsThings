using UnityEngine;
using UnityEngine.UI;

public class CharacterStateUIPage : View {

    [SerializeField] protected Canvas m_characterStateCanvas = null;
    [SerializeField] protected Text m_characterStateText_HP = null;
    [SerializeField] protected Text m_characterStateText_Mat = null;

    public void SetCharacterStateUIActive(bool active, int hp, int mat)
    {
        m_characterStateText_HP.text = string.Format("HP:{0}", hp);
        m_characterStateText_Mat.text = string.Format("Material:{0}", mat);
        m_characterStateCanvas.gameObject.SetActive(active);
    }

}
