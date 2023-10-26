using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{

    [SerializeField] private Text m_textPrompt;

    public void UpdateText(string textMassage)
    {
        m_textPrompt.text = textMassage;
    }

}
