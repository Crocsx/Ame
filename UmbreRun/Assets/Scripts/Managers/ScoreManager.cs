using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager m_instance = null;
    public static ScoreManager Instance
    {
        get
        {
            if (m_instance)
                return m_instance;
            return null;
        }
    }

    public delegate void TriggerScoreModified(int newScore);
    public event TriggerScoreModified OnScoreChanged;

    [SerializeField]
    private float m_scoreMultiplierBase = 100.0f;

    private float m_gameSpeed = 0.0f;

    private int m_score = 0;
    public int Score
    {
        get { return m_score; }
        protected set
        {
            m_score = value;
            if (OnScoreChanged != null)
                OnScoreChanged(value);
        }
    }

    private void Awake()
    {
        if (m_instance == null)
            m_instance = this;
        else if (m_instance != this)
        {
            Debug.LogWarning("ScoreManager.Awake() - instance already exists!");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        m_gameSpeed = GameManager.Instance.ElementsSpeed;
        GameManager.Instance.OnSpeedModified += HandleSpeedModified;
    }
	
	private void Update()
    {
        Score += Mathf.FloorToInt(Time.deltaTime * m_gameSpeed * m_scoreMultiplierBase);
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
}
