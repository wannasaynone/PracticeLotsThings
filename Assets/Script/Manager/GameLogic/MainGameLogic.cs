using UnityEngine;

public class MainGameLogic : IGameLogic {

    private float m_materialSpawnRate = 0f;
    private float m_materialSpawnTime = 0f;
    private float m_materialSpawnTimer = -1f;
    private GameObject m_materialCubePrefab = null;

    public MainGameLogic(float materialSpawnRate, float materialSpawnTime)
    {
        m_materialSpawnRate = materialSpawnRate;
        m_materialSpawnTime = materialSpawnTime;
        m_materialSpawnTimer = m_materialSpawnTime;
        m_materialCubePrefab = Resources.Load<GameObject>("MaterialCube");
    }

    public void Tick()
    {
        if(m_materialSpawnTimer > 0)
        {
            m_materialSpawnTimer -= Time.deltaTime;
            if(m_materialSpawnTimer <= 0)
            {
                m_materialSpawnTimer = m_materialSpawnTime;
                if(Random.Range(0f, 100f) < m_materialSpawnRate)
                {
                    Object.Instantiate(m_materialCubePrefab, Engine.GetRamdomPosition(), Quaternion.identity);
                }
            }
        }
    }

}
