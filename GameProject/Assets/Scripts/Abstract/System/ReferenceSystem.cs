using UnityEngine;


public class ReferenceSystem : MonoBehaviour
{
    public const string TAG_PLAYER = "Player";
    public static ReferenceSystem instance;

    public GameObject player;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(instance);
        }
        instance = this;
        FindObjects();
    }

    private void FindObjects()
    {
        player = GameObject.FindGameObjectWithTag(TAG_PLAYER);
    }
}
