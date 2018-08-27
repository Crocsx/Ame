using UnityEngine;

public abstract class AObstacle : MonoBehaviour
{
    [SerializeField]
    private Collider2D m_collider = null;
    protected Collider2D Collider
    {
        get { return m_collider; }
    }

    [SerializeField]
    private Vector3 m_spawnPoint = new Vector3(0.0f, 0.0f, 0.0f);
    protected Vector3 SpawnPoint
    {
        get { return m_spawnPoint; }
    }

    private float m_speed = 0.0f;
    public float Speed
    {
        get { return m_speed; }
        protected set { m_speed = value; }
    }

    private float m_deathLimitX = 0;

    protected void Awake()
    {
        if (m_collider == null)
        {
            Debug.LogWarning("AObstacle.Awake() - collider should be set in Editor!");
            m_collider = GetComponent<Collider2D>();
            if (m_collider == null)
                Debug.LogError("AObstacle.Awake() - collider does not exists");
        }
    }

    protected void Start()
    {
        transform.position = SpawnPoint;
        m_deathLimitX = -SpawnPoint.x;
        Speed = GameManager.Instance.ElementsSpeed;

        GameManager.Instance.OnSpeedModified += HandleSpeedModified;
    }

    protected void Update()
    {
        transform.position = transform.position - new Vector3(Speed * Time.deltaTime, 0.0f, 0.0f);

        if (m_deathLimitX > transform.position.x)
            Destroy(gameObject);
    }

    protected void OnDestroy()
    {
        if (GameManager.Instance)
            GameManager.Instance.OnSpeedModified -= HandleSpeedModified;
    }

    protected void HandleSpeedModified(float newSpeed)
    {
        Speed = newSpeed;
    }

    #region Collision
    private void OnTriggerEnter2D(Collider2D otherCol)
    {
        if (otherCol.CompareTag("Player"))
        {
            ADamageable damageable = otherCol.GetComponent<ADamageable>();
            if (damageable)
                damageable.OnHitObstacle();
        }
    }
    #endregion
}
