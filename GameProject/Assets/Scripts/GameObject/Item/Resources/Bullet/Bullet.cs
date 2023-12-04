using UnityEngine;

public class Bullet : PoolObject
{
    [SerializeField] private float m_arrowForce = 1;
    [SerializeField] private float m_timeToLive = 1;
    [SerializeField] private AudioClip m_clipArrow;

    private Rigidbody m_rigidbody;
    private BoxCollider m_collider;
    private PoolBullet m_poolBullet;

    private AudioSource m_audioSource;

    private bool m_isCollide = false;
    private float m_currentTimeLive = 0;

    public void Fire(Vector3 direction)
    {

        m_rigidbody.AddForce(direction * m_arrowForce, ForceMode.VelocityChange);

    }

    protected override void Initialization()
    {
        m_rigidbody.isKinematic = false;
        m_collider.isTrigger = false;
        m_isCollide = false;
        m_currentTimeLive = 0;
    }

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_collider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        m_poolBullet = PoolBullet.instance;
        m_audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (m_isCollide)
        {
            m_currentTimeLive += Time.deltaTime;
            if (m_currentTimeLive >= m_timeToLive)
            {
                m_poolBullet.DestroyBullet(this);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        m_audioSource.pitch = Random.Range(0.8f, 1.2f);
        m_audioSource.PlayOneShot(m_clipArrow);
        m_isCollide = true;
        m_rigidbody.isKinematic = true;
        m_collider.isTrigger = true;
    }

}
