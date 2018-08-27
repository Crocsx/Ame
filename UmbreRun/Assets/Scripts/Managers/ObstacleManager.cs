using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    private static ObstacleManager m_instance = null;
    public static ObstacleManager Instance
    {
        get
        {
            if (m_instance)
                return m_instance;
            return null;
        }
    }

    [SerializeField]
    private float m_minDistanceBetweenObstacles = 10.0f;
    [SerializeField]
    private float m_maxDistanceBetweenObstacles = 20.0f;
    [SerializeField]
    private List<AObstacle> m_listObstacles = null;

    private float m_timeBeforeNextObstacle = 0.0f;
    private float m_gameSpeed = 0.0f;

    private void Awake()
    {
        if (m_instance == null)
            m_instance = this;
        else if (m_instance != this)
        {
            Debug.LogWarning("ObstacleManager.Awake() - instance already exists!");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (m_listObstacles.Count <= 0)
            Debug.LogError("ObstacleManager.Start() - no obstacles set in list");

        m_gameSpeed = GameManager.Instance.ElementsSpeed;
        GameManager.Instance.OnSpeedModified += HandleSpeedModified;
    }

    private void Update()
    {
        m_timeBeforeNextObstacle -= Time.deltaTime * m_gameSpeed;
        if (m_timeBeforeNextObstacle <= 0.0f)
        {
            SendObstacle();
            m_timeBeforeNextObstacle = Random.Range(m_minDistanceBetweenObstacles, m_maxDistanceBetweenObstacles);
        }
    }

    private void OnDestroy()
    {
        if (GameManager.Instance)
            GameManager.Instance.OnSpeedModified -= HandleSpeedModified;
    }

    private void SendObstacle()
    {
        Instantiate( m_listObstacles[Random.Range(0, m_listObstacles.Count)] );
    }

    private void HandleSpeedModified(float newSpeed)
    {
        m_gameSpeed = newSpeed;
    }
}
