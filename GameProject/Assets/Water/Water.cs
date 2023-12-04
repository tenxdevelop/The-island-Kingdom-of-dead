using System.Collections;
using TheIslandKOD;
using UnityEngine;

public class Water : MonoBehaviour
{
    private Coroutine m_coroutine;

    private Player m_player;
    private void Start()
    {
        ReferenceSystem.OnFindedObjecs += OnInitPlayer;
    }

    private void OnInitPlayer()
    {
        ReferenceSystem.OnFindedObjecs -= OnInitPlayer;
        m_player = ReferenceSystem.instance.player.GetComponent<Player>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == (int)LayerType.Player)
        {
            m_coroutine = StartCoroutine(PlayerInWaterUpdate());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == (int)LayerType.Player)
        {
            if (m_coroutine != null)
            {
                StopCoroutine(m_coroutine);
                m_coroutine = null;
            }
        }
    }


    private IEnumerator PlayerInWaterUpdate()
    {
        while (true)
        {
            m_player.TakeDamage(10);

            yield return new WaitForSeconds(2);
        }
    }

}
