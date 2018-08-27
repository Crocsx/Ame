using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager m_instance = null;
    public static GameManager Instance
    {
        get
        {
            if (m_instance)
                return m_instance;
            return null;
        }
    }

    private delegate void TriggerUpdate();
    private event TriggerUpdate OnUpdate = () => { };

    public delegate void TriggerSpeedModified(float newSpeed);
    public event TriggerSpeedModified OnSpeedModified;

    [SerializeField]
    private float m_elementsSpeed = 5.0f;
    public float ElementsSpeed
    {
        get { return m_elementsSpeed; }
        private set
        {
            m_elementsSpeed = value;
            if (OnSpeedModified != null)
                OnSpeedModified(m_elementsSpeed);
        }
    }

    [SerializeField]
    private float m_timeBetweenSpeedIncrease = 1.0f;
    [SerializeField]
    private float m_increaseStepValue = 0.1f;

    private float m_timeBeforeNextSpeedIncrease = 0.0f;

    [SerializeField]
    private bool m_isGuilleminot = false;

    private void Awake()
    {
        if (m_instance == null)
            m_instance = this;
        else if (m_instance != this)
        {
            Debug.LogWarning("GameManager.Awake() - instance already exists!");
            Destroy(gameObject);
        }

        ElementsSpeed = 5.0f;
        m_timeBeforeNextSpeedIncrease = m_timeBetweenSpeedIncrease;
    }

    private void Start()
    {
        // TODO: change
        OnUpdate = UpdateRunning;
	}
	
	private void Update()
    {
        OnUpdate();
    }

    private void OnDestroy()
    {
        m_instance = null;
    }

    public void NotifyLose()
    {
        if (m_isGuilleminot)
            return;

        OnUpdate = UpdateGameOver;
    }

    #region Update functions
    private void UpdateRunning()
    {
        m_timeBeforeNextSpeedIncrease -= Time.fixedDeltaTime;

        if (m_timeBeforeNextSpeedIncrease <= 0)
        {
            ElementsSpeed += m_increaseStepValue;
            m_timeBeforeNextSpeedIncrease = m_timeBetweenSpeedIncrease;
        }
    }

    private void UpdateGameOver()
    {
        ElementsSpeed = 0.0f;
        // TODO: score, restart etc.
    }
    #endregion
}
