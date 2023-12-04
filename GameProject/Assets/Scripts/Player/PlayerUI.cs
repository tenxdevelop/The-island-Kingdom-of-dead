using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{

    private Text m_textPrompt;
    private void Start()
    {
        m_textPrompt = ReferenceSystem.instance.TextPromtMessage;
    }

    public void UpdateText(string textMassage)
    {
        m_textPrompt.text = textMassage;
    }

}
