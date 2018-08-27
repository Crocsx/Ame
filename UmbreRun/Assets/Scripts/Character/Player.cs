using System.Collections;
using UnityEngine;

public class Player : ADamageable
{
    [SerializeField]
    Umbrella UmbrellaArm;

    [SerializeField]
    GameObject Arm;

    [SerializeField]
    float m_rotationSpeed = 2.0f;

    Quaternion m_targetRot;
    public Quaternion TargetRotation
    {
        get { return m_targetRot; }
        set
        {
            m_targetRot = value;
            m_currTimeRot = 0.0f;
        }
    } // Set TargetRotation as degrees

    float m_currTimeRot = 0.0f;

    [SerializeField]
    AnimationCurve m_jumpCurve;

    [SerializeField]
    float m_crouchTime = 1.0f;
    bool m_isCrouching = false;

    bool IsJumping()
    {
        return !Mathf.Approximately(transform.position.y, 0.0f);
    }

    public void Jump()
    {
        if (!IsJumping() && !m_isCrouching) // Not in the air.
            StartCoroutine("JumpCoroutine");
    }

    IEnumerator JumpCoroutine()
    {
        float currTime = 0.0f;
        float endTime = m_jumpCurve.keys[m_jumpCurve.length - 1].time;

        while ((currTime += Time.deltaTime) < endTime)
        {
            transform.position = new Vector3(transform.position.x, m_jumpCurve.Evaluate(currTime), transform.position.z);
            yield return null;
        }

        transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
    }

    public void Crouch()
    {
        if(!m_isCrouching && !IsJumping())
            StartCoroutine("CrouchCoroutine");
    }

    IEnumerator CrouchCoroutine()
    {
        m_isCrouching = true;
        float currTime = 0.0f;

        while ((currTime += Time.deltaTime) < m_crouchTime)
        {
            //Animation + HitBox
            yield return null;
        }

        m_isCrouching = false;
    }

    void UpdateRotate()
	{
        m_currTimeRot += Time.deltaTime * m_rotationSpeed;

        Arm.transform.rotation = Quaternion.Slerp(Arm.transform.rotation,　m_targetRot, m_currTimeRot);
	}

    private void Update()
    {
        UpdateRotate();
    }

    private void Start()
    {
        GameManager.Instance.RegisterPlayer(this);
    }
}
