using System;
using UnityEngine;

[Serializable]
public class Tuple
{
    public Sprite sprite;

    public Vector3 pos;
    public Vector3 scale = Vector3.one;
}

public class Background : MonoBehaviour
{
    SpriteRenderer m_spriteRend;

    [SerializeField]
    float m_scrollSpeed = 1.0f;

    [SerializeField]
    float m_spawnRate = 100.0f;

    [SerializeField]
    Tuple[] m_sprites;

    //Very Bad Hardcode
    float maxX = 19;
    Vector3 offset;

    void Start ()
    {
        offset = new Vector3(2 * maxX, 0.0f, 0.0f);

        m_spriteRend = GetComponent<SpriteRenderer>();
        m_spriteRend.enabled = false;

        if(m_spawnRate == 100.0f)
        m_spriteRend.enabled = true;
    }

    private void Update()
    {
        if(!m_spriteRend.enabled && UnityEngine.Random.Range(0.0f, 100.0f) < m_spawnRate)
        {
            int index = (int)UnityEngine.Random.Range(0.0f, m_sprites.Length);
            m_spriteRend.sprite = m_sprites[index].sprite;
            
            transform.position = m_sprites[index].pos + offset;
            transform.localScale = m_sprites[index].scale;

            m_spriteRend.enabled = true;
        }

        transform.position -= new Vector3(m_scrollSpeed * Time.deltaTime * GameManager.Instance.ElementsSpeed, 0.0f, 0.0f);

        if (transform.position.x <= -maxX)
        {
            if(m_spawnRate == 100.0f)
                transform.position += new Vector3(maxX, 0.0f, 0.0f);
            else
                m_spriteRend.enabled = false;
        }
    }
}
