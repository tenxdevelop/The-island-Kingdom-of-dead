using TheIslandKOD;
using UnityEngine;

public class PoolBullet : MonoBehaviour
{
    public static PoolBullet instance;

    [SerializeField] private int poolAmount = 10;
    [SerializeField] private bool autoExpand = false;
    [SerializeField] private Bullet prefab;

    private PoolObjects<Bullet> m_poolBullet;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(instance);
        }
        instance = this;
    }


    private void Start()
    {
        m_poolBullet = new PoolObjects<Bullet>(prefab, poolAmount, transform, autoExpand);
    }

    public Bullet CreateBullet()
    {
        return m_poolBullet.GetFreeObject();
    }

    public void DestroyBullet(Bullet bullet)
    {
        bullet.transform.parent = transform;
        bullet.gameObject.SetActive(false);
    }

}
