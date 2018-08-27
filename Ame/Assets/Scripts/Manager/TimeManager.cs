using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{

    public static TimeManager instance = null;

    #region Variables
    public float deltaTime
    {
        get
        {
            return _deltaTime;
        }
    }
    private float _deltaTime;

    public float optionalTime
    {
        get
        {
            return _optionalTime;
        }
    }
    private float _optionalTime;

    public float fixedDeltaTime
    {
        get
        {
            return _fixedDeltaTime;
        }
    }
    private float _fixedDeltaTime;

    private float _modifierMain;
    private float _modifierOptional;

    // Used if paused, in order to restore the good value
    private float _modifierMainBackup;
    private float _modifierOptionalBackup;
    #endregion

    #region Singleton Initialization 
    void Awake()
    {

        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(transform.gameObject);
    }
    #endregion

    #region Native Methods
    void Start()
    {
        GameManager.instance.OnPauseGame += OnPause;
        GameManager.instance.OnResumeGame += OnResume;

        _deltaTime = 0;
        _fixedDeltaTime = 0;
        _optionalTime = 0;
        _modifierOptional = 1;
        _modifierOptionalBackup = 1;
        _modifierMainBackup = 1;
        _modifierMain = 1;
    }

    void Update()
    {
        _deltaTime = Time.deltaTime * _modifierMain;
        _optionalTime = Time.deltaTime * _modifierOptional;
    }

    void FixedUpdate()
    {
        _fixedDeltaTime = Time.fixedDeltaTime * _modifierMain;
    }
    #endregion

    private void OnDestroy()
    {
        GameManager.instance.OnPauseGame -= OnPause;
        GameManager.instance.OnResumeGame -= OnResume;
    }
    #region Private Methods
    private void OnPause()
    {
        _modifierMainBackup = _modifierMain;
        _modifierOptionalBackup = _modifierOptional;

        Time.timeScale = 0;
        _modifierMain = 0;
        _modifierOptional = 0;
    }

    private void OnResume()
    {
        Time.timeScale = _modifierMainBackup;
        _modifierMain = _modifierMainBackup;
        _modifierOptional = _modifierOptionalBackup;
    }
    #endregion

    #region Public Methods
    void TimeScale(float Value)
    {
        _modifierOptional = _modifierOptional / (_modifierMain / Value);
        _modifierMain = Value;
    }

    void TimeScaleAltered(float Value)
    {
        _modifierOptional = Mathf.Lerp(0 , _modifierMain, Value);
    }
    #endregion
}
