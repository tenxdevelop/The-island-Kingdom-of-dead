using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    [SerializeField] private Image m_LoadScreen;
    [SerializeField] private float m_duration;
    [SerializeField] private float m_fadeSpeed;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(instance);
        }

        instance = this;

        GenerateMap.OnInitMapedEvent += InitMaped;

    }

    public void SetCursorVisible(bool state)
    {
        Cursor.visible = state;
        if (state)
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;         
        }
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    private void InitMaped()
    {
        GenerateMap.OnInitMapedEvent -= InitMaped;

        StartCoroutine(UpdateLoadScreen());
    }

    private IEnumerator UpdateLoadScreen()
    {
        yield return new WaitForSeconds(m_duration);
        while (m_LoadScreen.color.a > 0)
        {
            float tempAlpha = m_LoadScreen.color.a;
            tempAlpha -= Time.deltaTime * m_fadeSpeed;
            m_LoadScreen.color = new Color(m_LoadScreen.color.r, m_LoadScreen.color.g, m_LoadScreen.color.b, tempAlpha);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

}
