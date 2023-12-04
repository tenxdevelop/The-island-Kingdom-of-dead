using UnityEngine;

public class Arrow : PoolObject
{

    [SerializeField] private float m_arrowForce = 1;
    [SerializeField] private float m_timeToLive = 1;
    [SerializeField] private AudioClip m_clipArrow;

    private Rigidbody m_rigidbody;
    private BoxCollider m_collider;
    private PoolArrow m_poolArrow;

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
        m_poolArrow = PoolArrow.instance;
        m_audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (m_rigidbody.velocity.magnitude != 0 && !m_isCollide)
        {
            transform.rotation = Quaternion.LookRotation(m_rigidbody.velocity) * Quaternion.Euler(90, 0, 0);   
        }
        if (m_isCollide)
        {
            m_currentTimeLive += Time.deltaTime;
            if (m_currentTimeLive >= m_timeToLive)
            {
                m_poolArrow.RemoveArrow(this);
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
