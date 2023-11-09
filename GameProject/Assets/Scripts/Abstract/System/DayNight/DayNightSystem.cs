using System.Collections.Generic;
using TheIslandKOD;
using UnityEngine;

public class DayNightSystem : MonoBehaviour
{
    private const string SHADER_SKYBOX = "Custom/SkyBoxBlender";
    private const string TAG_SKYBOX_SHADER_TRANSITION = "_TransitionFactor";

    [SerializeField] private Light m_directinalLight;

    [SerializeField] private float m_dayDurationInSeconds = 24f;
    
    [SerializeField] private List<SkyBoxTimeMapping> m_timeMappings;

    [SerializeField] private float m_timeBlendedSkyBox = 1f;

    private int m_currentHour = 0;
    private float m_currentTimeOfDay = 0.5f;

    private float m_blendedValue = 0f;
    public int currentHour => m_currentHour;

    private void Update()
    {
        m_currentTimeOfDay += Time.deltaTime / m_dayDurationInSeconds;
        m_currentTimeOfDay %= 1;
        m_currentHour = Mathf.FloorToInt(m_currentTimeOfDay * 24);

        m_directinalLight.transform.rotation = Quaternion.Euler(new Vector3((-m_currentTimeOfDay * 360) - 90, 105, 15));
        UpdateSkyBox();
    }

    private void UpdateSkyBox()
    {
        Material currentSkyBox = null;
        foreach (var map in m_timeMappings)
        {
            if (m_currentHour == map.hour)
            {
                currentSkyBox = map.skyBox;

                if (currentSkyBox?.shader.name == SHADER_SKYBOX)
                {
                    m_blendedValue += Time.deltaTime / m_timeBlendedSkyBox;
                    m_blendedValue = Mathf.Clamp01(m_blendedValue);
                    currentSkyBox.SetFloat(TAG_SKYBOX_SHADER_TRANSITION, m_blendedValue);
                }
                else
                {
                    m_blendedValue = 0;
                }

                break;
            }
        }

        if (currentSkyBox != null)
        {
            RenderSettings.skybox = currentSkyBox;
        }
    }
}
