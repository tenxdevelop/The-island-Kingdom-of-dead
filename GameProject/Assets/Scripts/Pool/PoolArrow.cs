using TheIslandKOD;
using UnityEngine;

public class PoolArrow : MonoBehaviour
{
    public static PoolArrow instance;

    [SerializeField] private int poolAmount = 10;
    [SerializeField] private bool autoExpand = false;
    [SerializeField] private Arrow prefab;

    private PoolObject<Arrow> m_poolArrow;

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

        m_poolArrow = new PoolObject<Arrow>(prefab, poolAmount, transform, autoExpand);

    }

    public Arrow CreateArrow()
    {
        return m_poolArrow.GetFreeObject();
    }

    public void RemoveArrow(Arrow arrow)
    {
        arrow.transform.parent = transform;
        arrow.gameObject.SetActive(false);
    }
}
