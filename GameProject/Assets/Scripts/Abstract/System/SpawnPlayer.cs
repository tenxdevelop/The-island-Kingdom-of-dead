using System;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    private static SpawnPlayer instance;
    public static event Action OnSpawnedEvent;

    [SerializeField] private Transform m_pointSpawn;
    [SerializeField] private CharacterController m_playerControl;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        var player = Instantiate(m_playerControl);
        player.transform.position = m_pointSpawn.position;
        player.enabled = true;
        OnSpawnedEvent?.Invoke();
    }

}
