using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer m_background;

    float m_scrollSpeed = 2.0f;

    //float m_currTime = 0.0f;

    //Bad Hardcode
    float m_width = 19.0f;

    Vector3 m_startPos;
    Vector3 m_endPos;

    private void Start()
    {
        m_startPos = m_background.transform.position;
        m_endPos = m_startPos - new Vector3(m_width, 0.0f, 0.0f);

        m_scrollSpeed = GameManager.Instance.ElementsSpeed;
        GameManager.Instance.OnSpeedModified += HandleSpeedModified;
    }

    private void Update()
    {
        //m_currTime += Time.deltaTime * m_scrollSpeed;
        m_background.transform.position -= new Vector3(m_scrollSpeed * Time.deltaTime, 0.0f, 0.0f);
        if (m_background.transform.position.x <= -m_width)
            m_background.transform.position += new Vector3(m_width, 0.0f, 0.0f);

        //m_background.transform.position = Vector3.Lerp(m_startPos, m_endPos, m_currTime);

        //if (m_currTime >= 1.0f)
        //{
        //    m_background.transform.position += new Vector3(m_width, 0.0f, 0.0f);
        //    m_currTime--;
        //}
    }

    private void OnDestroy()
    {
        if (GameManager.Instance)
            GameManager.Instance.OnSpeedModified -= HandleSpeedModified;
    }

    private void HandleSpeedModified(float newSpeed)
    {
        m_scrollSpeed = newSpeed;
    }
}