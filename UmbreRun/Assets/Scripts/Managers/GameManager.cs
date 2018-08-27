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
    }

    private void Start()
    {

	}
	
	private void Update()
    {
        OnUpdate();
    }

    private void OnDestroy()
    {
        m_instance = null;
    }
}
