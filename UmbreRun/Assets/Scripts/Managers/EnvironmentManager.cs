using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    private static EnvironmentManager m_instance = null;
    public static EnvironmentManager Instance
    {
        get
        {
            if (m_instance)
                return m_instance;
            return null;
        }
    }

    private void Awake()
    {
        if (m_instance == null)
            m_instance = this;
        else if (m_instance != this)
        {
            Debug.LogWarning("EnvironmentManager.Awake() - instance already exists!");
            Destroy(gameObject);
        }
    }

    #region Serialized Fields
    [SerializeField]
    private float m_rotationSpeed = 1.0f;
    [SerializeField]
    private float m_minTimeBeforeNextWeatherChange = 10.0f;
    [SerializeField]
    private float m_maxTimeBeforeNextWeatherChange = 20.0f;
    [SerializeField]
    private Vector2 m_minMaxAnglesRain = new Vector2(-65.0f, 65.0f);
    #endregion

    private List<Rain> m_listRains = new List<Rain>();

    private float m_timeBeforeNextWeatherChange = 5.0f;
    private float m_currRotationRatio = 0.0f;
    private float m_gameSpeed = 0.0f;

    private float m_targetAngle;
    public float TargetAngle
    {
        get { return m_targetAngle; }
        set
        {
            m_targetAngle = value;
            m_currRotationRatio = 0.0f;
        }
    }

    public void RegisterRain(Rain oneRain)
    {
        m_listRains.Add(oneRain);
    }

    private void Start()
    {
        m_gameSpeed = GameManager.Instance.ElementsSpeed;
        GameManager.Instance.OnSpeedModified += HandleSpeedModified;
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeRainDirection(Random.Range(m_minMaxAnglesRain.x, m_minMaxAnglesRain.y));
        }
        else
#endif
        UpdateWeather();

        if (m_currRotationRatio < 1.0f)
            UpdateRotation();
    }

    private void OnDestroy()
    {
        if (GameManager.Instance)
            GameManager.Instance.OnSpeedModified -= HandleSpeedModified;
    }

    private void UpdateWeather()
    {
        m_timeBeforeNextWeatherChange -= Time.deltaTime * m_gameSpeed;
        if (m_timeBeforeNextWeatherChange <= 0.0f)
        {
            ChangeRainDirection(Random.Range(m_minMaxAnglesRain.x, m_minMaxAnglesRain.y));
            m_timeBeforeNextWeatherChange = Random.Range(m_minTimeBeforeNextWeatherChange, m_maxTimeBeforeNextWeatherChange);
        }
    }

    private void UpdateRotation()
    {
        m_currRotationRatio += Time.deltaTime * m_rotationSpeed * m_gameSpeed;

        transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.Euler(
                    transform.rotation.eulerAngles.x,
                    transform.rotation.eulerAngles.y,
                    TargetAngle),
                m_currRotationRatio
            );

        foreach (Rain oneRain in m_listRains)
        {
            oneRain.transform.rotation = Quaternion.Slerp(oneRain.transform.rotation,
                Quaternion.Euler(
                    oneRain.transform.rotation.eulerAngles.x,
                    oneRain.transform.rotation.eulerAngles.y,
                    TargetAngle),
                m_currRotationRatio
            );
        }
    }

    public void ChangeRainDirection(float angleDegree)
    {
        TargetAngle = angleDegree;
    }

    private void HandleSpeedModified(float newSpeed)
    {
        m_gameSpeed = newSpeed;
    }

    public Vector3 GetRainDirection()
    {
        Debug.DrawRay(new Vector3(0.0f, 0.0f, 0.0f), transform.rotation * Vector3.down, Color.blue, 2.0f);
        return transform.rotation * Vector3.down;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector3(0.0f, 0.0f, 0.0f), GetRainDirection() * 3.0f);
    }
}
