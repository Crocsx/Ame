using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Player m_player = null;

    #region Unity functions
    protected void Start()
    {
        if (m_player == null)
        {
            m_player = FindObjectOfType<Player>();
            if (m_player == null)
            {
                Debug.LogError("PlayerController.Awake() - can't find player!");
            }
        }

        TouchManager.Instance.OnMoveDown += AskCrouch;
        TouchManager.Instance.OnMoveUp += AskJump;

        GyroscopeManager.Instance.OnGyroUpdateZAngle += AskRotationUpdate;
    }

    protected void OnDestroy()
    {
        if (TouchManager.Instance)
        {
            TouchManager.Instance.OnMoveDown -= AskCrouch;
            TouchManager.Instance.OnMoveUp -= AskJump;
        }

        if (GyroscopeManager.Instance)
        {
            GyroscopeManager.Instance.OnGyroUpdateZAngle -= AskRotationUpdate;
        }
    }
    #endregion

    private void AskCrouch()
    {
        m_player.Crouch();
    }

    private void AskJump()
    {
        m_player.Jump();
    }

    private void AskRotationUpdate(float angleDegreeZ)
    {
        m_player.TargetRotation = angleDegreeZ;
    }
}
