using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIDeadScreen : MonoBehaviour
{
    [SerializeField] private float m_duration;
    [SerializeField] private float m_fadeSpeed;

    private Image m_deadImage;

    private void Start()
    {
        Player.OnPlayerDeadEvent += PlayerDeadEvent;
        m_deadImage = GetComponent<Image>();
        gameObject.SetActive(false);
    }

    private void PlayerDeadEvent()
    {
        gameObject.SetActive(true);
        GameManager.instance.SetCursorVisible(true);
        StartCoroutine(UpdateDeadScreen());
    }


    private IEnumerator UpdateDeadScreen()
    {
        yield return new WaitForSeconds(m_duration);
        while (m_deadImage.color.a < 0.98)
        {
            float tempAlpha = m_deadImage.color.a;
            tempAlpha += Time.deltaTime * m_fadeSpeed;
            m_deadImage.color = new Color(m_deadImage.color.r, m_deadImage.color.g, m_deadImage.color.b, tempAlpha);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

}
