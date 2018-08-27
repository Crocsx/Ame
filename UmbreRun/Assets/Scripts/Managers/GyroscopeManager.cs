using UnityEngine;

public class GyroscopeManager : MonoBehaviour
{
    private static GyroscopeManager m_instance = null;
    public static GyroscopeManager Instance
    {
        get
        {
            if (m_instance)
                return m_instance;

            Debug.LogError("GyroscopeManager.Instance.get - instance is null!");
            return null;
        }
    }

    public delegate void UpdateGyroDataAngle(float angleDegree);
    public event UpdateGyroDataAngle OnGyroUpdateZAngle;

    protected void Awake()
    {
        if (m_instance == null)
            m_instance = this;
        else if (m_instance != this)
        {
            Debug.LogWarning("GyroscopeManager.Awake() - instance already exists!");
            Destroy(gameObject);
        }
    }

    protected void Start()
    {
        Input.gyro.enabled = true;
	}

    Quaternion gyroQuat; // TEMP

    protected void Update()
    {
        if (OnGyroUpdateZAngle != null)
            OnGyroUpdateZAngle(Input.gyro.rotationRateUnbiased.z);
    }
}
