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

        GyroscopeManager.Instance.OnGyroUpdate += AskRotationUpdate;
    }

    protected void OnDestroy()
    {
        TouchManager.Instance.OnMoveDown -= AskCrouch;
        TouchManager.Instance.OnMoveUp -= AskJump;

        GyroscopeManager.Instance.OnGyroUpdate -= AskRotationUpdate;
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

    private void AskRotationUpdate(Quaternion rot)
    {
        m_player.TargetRotation = rot;
    }
}
