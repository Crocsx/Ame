using System.Collections;
using UnityEngine;

public class Player : ADamageable
{
    [SerializeField]
    Umbrella UmbrellaArm;

    [SerializeField]
    GameObject Arm;

    [SerializeField]
    Vector2 m_armRange = new Vector2(-0.5f, 0.4f);

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

    public void UpdateRotate(float rotationZ)
	{
        Quaternion temp = Arm.transform.rotation;

        Arm.transform.Rotate(0.0f, 0.0f, rotationZ);

        // fast and ugly clamp
        if (Arm.transform.rotation.z < m_armRange.x || Arm.transform.rotation.z > m_armRange.y)
            Arm.transform.rotation = temp;
    }
}
