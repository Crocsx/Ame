using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    HighScore save;

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
    private float m_scoreMultiplierBase = 0.4f;

    private float m_gameSpeed = 0.0f;

    private float m_score = 0;
    public float Score
    {
        get { return m_score; }
        protected set
        {
            m_score = value;
            if (OnScoreChanged != null)
                OnScoreChanged((int)value);
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
        Score += Time.deltaTime * m_gameSpeed * m_scoreMultiplierBase;
        Debug.Log(Score);
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

    public void SaveScore()
    {
        for(int i = 0; i < save.scores.Length; i++)
        {
            if(m_score > save.scores[i])
            {
                for(int j = save.scores.Length - 1; j > i; j--)
                {
                    save.scores[j] = save.scores[j - 1];
                }

                save.scores[i] = (int)m_score;
                return;
            }
        }
    }
}
