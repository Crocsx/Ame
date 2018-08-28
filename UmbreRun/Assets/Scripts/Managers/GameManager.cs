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

    public enum GameState : uint
    {
        None = 0,
        Init,
        Menu,
        StartGame,
        InGame,
        Pause,
        GameOver,

        COUNT
    }
    public GameState m_gameState = GameState.Init;

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
    private float m_pausedSpeed = 0.0f;

    [SerializeField]
    private float m_timeBetweenSpeedIncrease = 1.0f;
    [SerializeField]
    private float m_increaseStepValue = 0.1f;

    private float m_timeBeforeNextSpeedIncrease = 0.0f;

    [SerializeField]
    private bool m_isGuilleminot = false;

    private Player m_player = null;
    public Player Player
    {
        get { return m_player; }
    }

    private void Awake()
    {
        if (m_instance == null)
            m_instance = this;
        else if (m_instance != this)
        {
            Debug.LogWarning("GameManager.Awake() - instance already exists!");
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        ElementsSpeed = 0.0f;
        m_timeBeforeNextSpeedIncrease = m_timeBetweenSpeedIncrease;
    }

    private void Start()
    {
        SetNewState(GameState.Menu);
    }
	
	private void Update()
    {
        OnUpdate();
    }

    private void OnDestroy()
    {
        m_instance = null;
    }

    public void RegisterPlayer(Player player)
    {
        if (m_player)
        {
            Debug.LogWarning("GameManager.RegisterPlayer() - m_player was already set, replacing with new player");
        }

        m_player = player;
    }

    public void SetNewState(GameState newState)
    {
        switch (newState)
        {
            case GameState.Menu:
                OnUpdate = UpdateMenu;
                break;
            case GameState.StartGame:
                ElementsSpeed = 5.0f;
                OnUpdate = UpdateStartGame;
                break;
            case GameState.InGame:
                if (m_gameState == GameState.Pause)
                    ElementsSpeed = m_pausedSpeed;
                OnUpdate = UpdateRunning;
                break;
            case GameState.Pause:
                m_pausedSpeed = ElementsSpeed;
                ElementsSpeed = 0.0f;
                OnUpdate = UpdatePause;
                break;
            case GameState.GameOver:
                ElementsSpeed = 0.0f;
                IGMenu.Instance.ShowPanel("End");
                IGMenu.Instance.SetupEnd();
                OnUpdate = UpdateGameOver;
                break;
            case GameState.None:
            case GameState.Init:
            case GameState.COUNT:
            default:
                OnUpdate = () => { };
                break;
        }

        m_gameState = newState;
    }

    public void NotifyLose()
    {
        if (m_isGuilleminot)
            return;

        SetNewState(GameState.GameOver);
    }

    #region Update functions
    private void UpdateMenu()
    {
        // Game Update while menu
    }

    private void UpdateStartGame()
    {
        SetNewState(GameState.InGame);
    }

    private void UpdatePause()
    {
        // Pause Update while menu
    }

    private void UpdateRunning()
    {
        m_timeBeforeNextSpeedIncrease -= Time.fixedDeltaTime;

        if (m_timeBeforeNextSpeedIncrease <= 0.0f)
        {
            ElementsSpeed += m_increaseStepValue;
            m_timeBeforeNextSpeedIncrease = m_timeBetweenSpeedIncrease;
        }

        CheckForLoss();
    }

    private void UpdateGameOver()
    {
        // TODO: score, restart etc.
    }
    #endregion

#if UNITY_EDITOR
    // DEBUG
    Vector2 DEBUG_tangent1;
    Vector2 DEBUG_tangent2;
    Vector2 DEBUG_windDirection;
    //
#endif
    private void CheckForLoss()
    {
        Vector2 windDirection = EnvironmentManager.Instance.GetRainDirection().normalized;
#if UNITY_EDITOR
        DEBUG_windDirection = new Vector2(windDirection.x, windDirection.y);
#endif
        CircleCollider2D playerCollider = m_player.Collider as CircleCollider2D;
        if (playerCollider == null)
        {
            Debug.LogError("GameManager.CheckForLoss() - wrong type for player collider, should be CircleCollider2D");
        }
        Vector2 colliderPos = playerCollider.transform.position;
        Vector2 orthoVector = new Vector2(-windDirection.y, windDirection.x);

        Vector2 tangentPoint = colliderPos + playerCollider.offset + orthoVector * playerCollider.radius;
#if UNITY_EDITOR
        DEBUG_tangent1 = new Vector2(tangentPoint.x, tangentPoint.y);
#endif
        RaycastHit2D hit = Physics2D.Raycast(tangentPoint, -windDirection, 100f, ~LayerMask.NameToLayer("Umbrella"));
        if (hit.collider == null)
            NotifyLose();

        tangentPoint = colliderPos + playerCollider.offset - orthoVector * playerCollider.radius;
#if UNITY_EDITOR
        DEBUG_tangent2 = new Vector2(tangentPoint.x, tangentPoint.y);
#endif
        hit = Physics2D.Raycast(tangentPoint, -windDirection, 100f, ~LayerMask.NameToLayer("Umbrella"));
        if (hit.collider == null)
            NotifyLose();
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(DEBUG_tangent1, DEBUG_tangent1 - DEBUG_windDirection * 5);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(DEBUG_tangent2, DEBUG_tangent2 - DEBUG_windDirection * 5);
    }
#endif
}
