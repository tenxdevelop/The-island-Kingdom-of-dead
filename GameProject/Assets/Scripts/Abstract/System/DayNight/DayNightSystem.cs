using System.Collections.Generic;
using TheIslandKOD;
using UnityEngine;

public class DayNightSystem : MonoBehaviour
{
    private const string SHADER_SKYBOX = "Custom/SkyBoxBlender";
    private const string TAG_SKYBOX_SHADER_TRANSITION = "_TransitionFactor";

    [SerializeField] private Light m_directinalLight;
  
    [SerializeField] private List<SkyBoxTimeMapping> m_timeMappings;

    private float m_blendedValue = 0f;
    private float m_changeLight = 1.54f;
    
    private void Update()
    {
        
        var angle = (-TimeManager.instance.currentTimeOfDay * 360) - 90;
        m_directinalLight.transform.rotation = Quaternion.Euler(new Vector3(angle, 105, 15));
        if (TimeManager.instance.currentHour == 19)
        {
            m_changeLight = Mathf.Lerp(m_changeLight, 0, Time.deltaTime * 2);
            m_directinalLight.intensity = m_changeLight;
        }
        else if (TimeManager.instance.currentHour == 5)
        {
            m_changeLight = Mathf.Lerp(m_changeLight, 1.54f, Time.deltaTime * 2);
            m_directinalLight.intensity = m_changeLight;
        }

        UpdateSkyBox();
    }

    private void UpdateSkyBox()
    {
        Material currentSkyBox = null;
        foreach (var map in m_timeMappings)
        {
            if (TimeManager.instance.currentHour == map.hour)
            {
                currentSkyBox = map.skyBox;

                if (currentSkyBox?.shader.name == SHADER_SKYBOX)
                {
                    m_blendedValue += Time.deltaTime / TimeManager.instance.timeBlendedSkyBox;
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
