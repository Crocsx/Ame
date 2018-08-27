using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer m_background1;

    [SerializeField]
    SpriteRenderer m_background2;

    [SerializeField]
    float m_scrollSpeed = 2.0f;

    float m_currTime = 0.0f;

    void Update ()
    {
        float width = 17;
        m_currTime += Time.deltaTime * m_scrollSpeed;

        if (m_currTime > width)
            m_currTime -= width;

        Vector3 offset = new Vector3(m_currTime, 0.0f, 0.0f);

        m_background1.transform.position -= offset;
        m_background2.transform.position -= offset;

        if (m_background1.transform.position.x <= -width)
            m_background1.transform.position = m_background2.transform.position + new Vector3(width + 2, 0.0f, 0.0f);

        else if (m_background2.transform.position.x <= -width)
            m_background2.transform.position = m_background1.transform.position + new Vector3(width + 2, 0.0f, 0.0f);
    }
}
