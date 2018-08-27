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

    private void OnDestroy()
    {
        m_instance = null;
    }

    protected void OnGUI()
    {
        GUI.skin.label.fontSize = Screen.width / 40;

        GUILayout.Label("Orientation: " + Screen.orientation);
        GUILayout.Label("input.gyro.attitude: " + Input.gyro.attitude);

        bool isLandScapeLeft = Screen.orientation == ScreenOrientation.LandscapeLeft;
        Quaternion gyroQuat = new Quaternion(
            Input.gyro.attitude.x * (isLandScapeLeft ? 1 : -1),
            Input.gyro.attitude.y * (isLandScapeLeft ? 1 : -1),
            Input.gyro.attitude.z * (isLandScapeLeft ? 1 : -1),
            Input.gyro.attitude.w * (isLandScapeLeft ? 1 : -1)
        );
        GUILayout.Label("gyroQuat: " + gyroQuat);

        GUILayout.Label("input.gyro.attitude.eulerAngles.z: " + Input.gyro.attitude.eulerAngles.z);
    }
}
