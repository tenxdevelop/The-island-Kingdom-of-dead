using TheIslandKOD;
using UnityEngine;
using UnityEngine.UI;

public class UIInfoCraftResources : MonoBehaviour
{
    [SerializeField] private Text m_textAmount;
    [SerializeField] private Text m_textItemType;
    [SerializeField] private Text m_textTotal;
    [SerializeField] private Text m_textHave;

    
    public void UpdateLoadInfo(string textAmount, string textItemType, string textTotal, string textHave)
    {
       
        m_textAmount.text = textAmount;
        m_textItemType.text = textItemType;
        m_textTotal.text = textTotal;
        m_textHave.text = textHave;
    }
}
