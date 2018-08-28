using UnityEngine;

public class WindManager : MonoBehaviour
{
    public class WindData
    {
        public float force;
        public float duration;
        public Vector2 direction;
    }
    private WindData m_currentWind = null;

    private static WindManager m_instance = null;
    public static WindManager Instance
    {
        get
        {
            if (m_instance)
                return m_instance;
            return null;
        }
    }

    #region Serialized Fields
    [SerializeField]
    private Vector2 m_forceOfWind = new Vector2(0.5f, 1.5f);
    [SerializeField]
    private Vector2 m_durationOfWind = new Vector2(1.0f, 5.0f);
    [SerializeField]
    private Vector2 m_timeLimitsBeforeNextWind = new Vector2(10.0f, 20.0f);
    #endregion

    private Player m_player = null;
    private float m_gameSpeed = 0.0f;
    private float m_timeBeforeNextWind = 10.0f;

    private void Start()
    {
        m_player = GameManager.Instance.Player;
        if (m_player == null)
        {
            m_player = FindObjectOfType<Player>();
            if (m_player == null)
            {
                Debug.LogError("WindManager.Start() - could not find Player!");
            }
        }

        m_gameSpeed = GameManager.Instance.ElementsSpeed;
        GameManager.Instance.OnSpeedModified += HandleSpeedModified;
    }
	
	private void Update()
    {
        if (m_currentWind != null)
        {
            UpdateCurrentWind();
            if (m_currentWind != null)
                return;
        }

        m_timeBeforeNextWind -= Time.deltaTime * m_gameSpeed;
        if (m_timeBeforeNextWind <= 0.0f)
        {
            CreateWind( Random.Range(m_forceOfWind.x, m_forceOfWind.y), Random.Range(m_durationOfWind.x, m_durationOfWind.y) );
            m_timeBeforeNextWind = Random.Range(m_timeLimitsBeforeNextWind.x, m_timeLimitsBeforeNextWind.y);
        }
    }

    private void OnDestroy()
    {
        if (GameManager.Instance)
            GameManager.Instance.OnSpeedModified -= HandleSpeedModified;

        m_instance = null;
    }

    private void HandleSpeedModified(float newSpeed)
    {
        m_gameSpeed = newSpeed;
    }

    private void CreateWind(float force, float duration)
    {
        m_currentWind = new WindData
        {
            force = force,
            duration = duration,
            direction = EnvironmentManager.Instance.GetRainDirection()
        };

        m_player.ReceiveWind(m_currentWind);
    }

    private void UpdateCurrentWind()
    {
        m_currentWind.duration -= Time.deltaTime * m_gameSpeed;

        if (m_currentWind.duration <= 0)
            m_currentWind = null;
    }
}
