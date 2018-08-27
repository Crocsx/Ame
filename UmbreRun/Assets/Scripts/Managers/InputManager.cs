using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private float m_minDistanceToTriggerMove = 50.0f;

    public delegate void TriggerMove();
    public event TriggerMove OnMoveUp;
    public event TriggerMove OnMoveDown;

    private bool m_needHandling = false;
    private Touch m_currentTouchStart;

    protected void Start()
    {
        Input.multiTouchEnabled = false;
    }

    protected void Update()
    {
        if (Input.touchCount <= 0)
            return;
        
        Debug.Log("UPDATE ENTERED");

        Touch activeTouch = Input.touches[0];

        switch (activeTouch.phase)
        {
            case TouchPhase.Began:
                {
                    m_currentTouchStart = activeTouch;
                    m_needHandling = true;
                    break;
                }
            case TouchPhase.Moved:
                {
                    if (HandleMove(activeTouch) == true)
                        m_needHandling = false;
                    break;
                }
            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                {
                    HandleMove(activeTouch);
                    m_needHandling = false;
                    break;
                }
            case TouchPhase.Stationary:
            default:
                break;
        }
	}

    private bool HandleMove(Touch activeTouch)
    {
        bool directionUp = m_currentTouchStart.position.y > activeTouch.position.y;
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

        return true;
    }
}
