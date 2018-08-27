﻿using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    GameObject Umbrella;

    [SerializeField]
    float m_rotationSpeed = 2.0f;

    float m_targetRot = 0.0f; // radiant rotation
    public float TargetRotation
    {
        get { return m_targetRot * Mathf.Rad2Deg; }
        set
        {
            m_targetRot = Mathf.Deg2Rad * value;
            m_currTimeRot = 0.0f;
        }
    } // Set TargetRotation as degrees

    float m_currTimeRot = 0.0f;

    [SerializeField]
    AnimationCurve m_jumpCurve;

    [SerializeField]
    float m_crouchTime = 1.0f;
    bool m_isCrouching = false;

    float temp = 3.0f;

    bool IsJumping()
    {
        return !Mathf.Approximately(transform.position.y, 0.0f);
    }

    void Jump()
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

    void Crouch()
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
        float angle = m_targetRot / 2.0f;
        m_currTimeRot += Time.deltaTime * m_rotationSpeed;

        Umbrella.transform.rotation = Quaternion.Slerp(Umbrella.transform.rotation,
            new Quaternion(0.0f, 0.0f, Mathf.Sin(angle), Mathf.Cos(angle)), m_currTimeRot);
	}

    private void Update()
    {
        if ((temp -= Time.deltaTime) <= 0.0f)
        {
            Jump();
            TargetRotation += 90.0f;
            temp = 3.0f;
        }

        UpdateRotate();
    }
}
