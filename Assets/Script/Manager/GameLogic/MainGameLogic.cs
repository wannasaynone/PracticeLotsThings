using UnityEngine;

public class MainGameLogic : IGameLogic {

    private float m_materialSpawnRate = 0f;
    private float m_materialSpawnTime = 0f;
    private float m_materialSpawnTimer = -1f;
    private MaterialCube m_materialCubePrefab = null;

    public MainGameLogic(float materialSpawnRate, float materialSpawnTime)
    {
        m_materialSpawnRate = materialSpawnRate;
        m_materialSpawnTime = materialSpawnTime;
        m_materialSpawnTimer = m_materialSpawnTime;
        m_materialCubePrefab = Resources.Load<MaterialCube>("MaterialCube");
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
                    InteractableObject.InstantiateInteractableObject(m_materialCubePrefab, Engine.GetRamdomPosition() + new Vector3(0, m_materialCubePrefab.transform.position.y, 0), Vector3.zero);
                }
            }
        }
    }

}
