using System;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    private static SpawnPlayer instance;
    public static event Action OnSpawnedEvent;

    [SerializeField] private Transform m_pointSpawn;
    [SerializeField] private Player m_player;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        var player = Instantiate(m_player);
        player.transform.position = m_pointSpawn.position;
        OnSpawnedEvent?.Invoke();
    }

}
