using System.Collections;
using UnityEngine;

public class Player : ADamageable
{
    float playerY;

    Animator m_animator;

    [SerializeField]
    Umbrella UmbrellaArm;

    [SerializeField]
    GameObject Arm;

    [SerializeField]
    Vector2 m_armRange = new Vector2(-0.5f, 0.4f);

    [SerializeField]
    AnimationCurve m_jumpCurve;

    [SerializeField]
    AnimationCurve m_crouchCurve;

    bool m_isCrouching = false;

    private float m_gameSpeed = 0.0f;

    private void Start()
    {
        m_animator = GetComponent<Animator>();

        playerY = transform.position.y;
        GameManager.Instance.RegisterPlayer(this);

        m_gameSpeed = GameManager.Instance.ElementsSpeed;
        GameManager.Instance.OnSpeedModified += HandleSpeedModifier;
    }

    private void OnDestroy()
    {
        if (GameManager.Instance)
            GameManager.Instance.OnSpeedModified -= HandleSpeedModifier;
    }

    private void HandleSpeedModifier(float newSpeed)
    {
        m_gameSpeed = newSpeed;
    }

    void Run()
    {
        AnimationClip jumpAnim = m_animator.runtimeAnimatorController.animationClips[1];

        // TODO: Use GameSpeed

        m_animator.speed = 1.0f;

        transform.position = new Vector3(transform.position.x, playerY, transform.position.z);
    }

    #region Jump
    bool IsJumping()
    {
        return !Mathf.Approximately(transform.position.y, playerY);
    }

    public void Jump()
    {
        if (!IsJumping() && !m_isCrouching) // Not in the air.
            StartCoroutine("JumpCoroutine");
    }

    IEnumerator JumpCoroutine()
    {
        m_animator.SetBool("IsJumping", true);

        float currTime = 0.0f;
        float endTime = m_jumpCurve.keys[m_jumpCurve.length - 1].time;

        AnimationClip jumpAnim = m_animator.runtimeAnimatorController.animationClips[1];

        m_animator.speed = jumpAnim.length / endTime;

        //TODO: Use GameSpeed

        while ((currTime += Time.deltaTime) < endTime)
        {
            transform.position = new Vector3(transform.position.x, m_jumpCurve.Evaluate(currTime) + playerY, transform.position.z);
            yield return null;
        }

        m_animator.SetBool("IsJumping", false);
        Run();
    }
    #endregion

    #region Crouch
    public void Crouch()
    {
        if (!m_isCrouching && !IsJumping())
            StartCoroutine("CrouchCoroutine");
    }

    IEnumerator CrouchCoroutine()
    {
        m_animator.SetBool("IsCrouching", true);

        m_isCrouching = true;

        float currTime = 0.0f;
        float endTime = m_crouchCurve.keys[m_jumpCurve.length - 1].time;

        AnimationClip crouchAnim1 = m_animator.runtimeAnimatorController.animationClips[2];
        AnimationClip crouchAnim2 = m_animator.runtimeAnimatorController.animationClips[3];
        m_animator.speed = (2 * crouchAnim1.length + crouchAnim2.length) / endTime;

        //TODO: Use Game Speed

        while ((currTime += Time.deltaTime) < endTime)
        {
            transform.position = new Vector3(transform.position.x, m_crouchCurve.Evaluate(currTime) + playerY, transform.position.z);
            yield return null;
        }

        m_animator.SetBool("IsCrouching", false);
        m_isCrouching = false;

        Run();
    }
    #endregion

    #region Wind
    private float m_windDuration = 0.0f;
    private float m_windForce = 0.0f;
    private float m_windAngle = 0.0f;

    [SerializeField]
    private float m_timeForWindAction = 0.4f;
    private float m_timeLeftForWindAction = 0.0f;
    private float m_windCurrentAngle = 0.0f;
    private bool m_needResetWind = false;
    #endregion

    public void ReceiveWind(WindManager.WindData windData)
    {
        if (m_windDuration > 0.0f)
            return;

        m_windAngle = Vector2.Angle(windData.direction, Vector2.down);
        m_windForce = windData.force;
        m_windDuration = windData.duration;
        m_needResetWind = false;
        m_timeLeftForWindAction = m_timeForWindAction;
        m_windCurrentAngle = 0.0f;
    }

    public void UpdateRotate(float rotationZ)
	{
        m_windDuration -= Time.deltaTime;
        m_timeLeftForWindAction -= Time.deltaTime;
        if (m_windDuration > 0.0f || m_timeLeftForWindAction > 0.0f || m_needResetWind)
        {
            float angleToAdd = 0.0f;
            if (m_timeLeftForWindAction > 0.0f)
            {
                angleToAdd = m_needResetWind
                    ? m_windAngle * m_windForce * Random.Range(0.6f, 1.4f) * Time.deltaTime
                    : -(m_windAngle * m_windForce * Random.Range(0.6f, 1.4f) * Time.deltaTime);
                m_windCurrentAngle += angleToAdd;
            }
            else if (m_windDuration > 0.0f)
            {
                m_needResetWind = !m_needResetWind;
                m_timeLeftForWindAction = m_timeForWindAction;
            }
            else
            {
                m_needResetWind = false;
                angleToAdd = -m_windCurrentAngle;
                m_windCurrentAngle = 0.0f;
            }

            rotationZ += angleToAdd;
        }

        Quaternion temp = Arm.transform.rotation;

        Arm.transform.Rotate(0.0f, 0.0f, rotationZ);

        // fast and ugly clamp
        if (Arm.transform.rotation.z < m_armRange.x || Arm.transform.rotation.z > m_armRange.y)
            Arm.transform.rotation = temp;
    }
}