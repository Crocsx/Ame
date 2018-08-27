using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private float m_minDistanceToTriggerMove = 30.0f;

    public delegate void TriggerMove();
    public event TriggerMove OnMoveUp;
    public event TriggerMove OnMoveDown;

    private bool m_isHandling = false;
    private bool m_hasTriggerHandle = false;
    private Touch m_currentTouchStart;

    protected void Start()
    {
        Input.multiTouchEnabled = false;
    }

    protected void Update()
    {
        if (Input.touchCount <= 0)
            return;

        Touch activeTouch = Input.touches[0];

        switch (activeTouch.phase)
        {
            case TouchPhase.Began:
                {
                    if (m_isHandling)
                        break;
                    m_currentTouchStart = activeTouch;
                    m_isHandling = true;
                    m_hasTriggerHandle = false;
                    break;
                }
            case TouchPhase.Moved:
                {
                    HandleMove(activeTouch);
                    break;
                }
            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                {
                    HandleMove(activeTouch);
                    m_isHandling = false;
                    break;
                }
            case TouchPhase.Stationary:
            default:
                break;
        }
	}

    private bool HandleMove(Touch activeTouch)
    {
        if (m_hasTriggerHandle)
            return true;

        bool directionUp = m_currentTouchStart.position.y < activeTouch.position.y;
        float distanceFromStartToCurrentX = Mathf.Abs(activeTouch.position.y - m_currentTouchStart.position.y);

        if (distanceFromStartToCurrentX < m_minDistanceToTriggerMove)
            return false;

        if (directionUp)
        {
        Debug.Log("UPDATE OnMoveUp");
            if (OnMoveUp != null)
                OnMoveUp();
        }
        else
        {
        Debug.Log("UPDATE OnMoveDown");
            if (OnMoveDown != null)
                OnMoveDown();
        }

        m_hasTriggerHandle = true;
        return true;
    }
}
