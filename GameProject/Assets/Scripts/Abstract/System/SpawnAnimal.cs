using UnityEngine;

public class SpawnAnimal : MonoBehaviour
{

    [SerializeField] private float m_timeSpawn;
    [SerializeField] private int m_countSpawn;
    [SerializeField] private BasicAI m_prefab;
    [SerializeField] private Transform m_positionSpawn;

    private float m_currentTimeSpawn;

    private void Start()
    {
        Spawn();
    }

    private void Update()
    {
        m_currentTimeSpawn += Time.deltaTime;

        if (m_currentTimeSpawn > m_timeSpawn)
        {
            Spawn();
            m_currentTimeSpawn = 0;
        }
    }


    private void Spawn()
    {
        for (int i = 0; i < m_countSpawn; i++)
        {
            var animal = Instantiate(m_prefab);
            animal.GetComponent<CharacterController>().enabled = false;
            animal.transform.position = m_positionSpawn.position;
            animal.GetComponent<CharacterController>().enabled = true;
        }
    }
}
