using System;
using UnityEngine;
using UnityEngine.UI;

public class ReferenceSystem : MonoBehaviour
{
    public static event Action OnFindedObjecs;
    public const string TAG_PLAYER = "Player";
    public static ReferenceSystem instance;


    [SerializeField] public Text TextPromtMessage;
    [SerializeField] public UIBar healthBar;
    [SerializeField] public Image damageOverlay;
    [SerializeField] public Camera MainCamera;
    public GameObject player { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(instance);
        }
        instance = this;

        SpawnPlayer.OnSpawnedEvent += FindObjects;
        
    }

    private void FindObjects()
    {
        SpawnPlayer.OnSpawnedEvent -= FindObjects;
        player = GameObject.FindGameObjectWithTag(TAG_PLAYER);
        OnFindedObjecs?.Invoke();
    }
}
