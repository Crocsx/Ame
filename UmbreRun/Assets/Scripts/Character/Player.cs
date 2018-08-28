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

        m_animator.speed = jumpAnim.length / endTime ;

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
        if(!m_isCrouching && !IsJumping())
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

    public void UpdateRotate(float rotationZ)
	{
        Quaternion temp = Arm.transform.rotation;

        Arm.transform.Rotate(0.0f, 0.0f, rotationZ);

        // fast and ugly clamp
        if (Arm.transform.rotation.z < m_armRange.x || Arm.transform.rotation.z > m_armRange.y)
            Arm.transform.rotation = temp;
    }
}