using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Rigidbody2D cRigidbody;
    float animUpTimer = 0;
    float animDownTIMER = 0;
    float animTIMER = 0.5f;
    Vector3[] verticesDefaultPosition;


    public float width = 4;
    public float height = 4;
    public Material material;

    Mesh m_meshTriangle;

    void Awake()
    {
        cRigidbody = transform.parent.GetComponent<Rigidbody2D>();
    }
    /// <summary>
    /// Create a square formed by 4 triangle
    /// Check this http://prntscr.com/jdj7sg
    /// 3 - 4 - 5
    /// | / | \ |
    /// 0 - 1 - 2
    /// </summary>
	void Start()
    {
        m_meshTriangle = GetComponent<MeshFilter>().mesh;
        MeshRenderer m_meshRendererTriangle = GetComponent<MeshRenderer>();
        m_meshTriangle.Clear();

        m_meshTriangle.vertices = new Vector3[]
        {
            new Vector3(-width/2, -height/2, 0), new Vector3(0, -height/2, 0), new Vector3(width/2, -height/2, 0),
            new Vector3(-width/2, height/2, 0), new Vector3(0, height/2, 0), new Vector3(width/2, height/2, 0)
        };

        m_meshTriangle.triangles = new int[] {
            0, 3, 4,
            1, 0, 4,
            2, 1, 4,
            5, 2, 4,
        };

        m_meshTriangle.normals = new Vector3[]
        {
            Vector3.forward, Vector3.forward, Vector3.forward,
            Vector3.forward, Vector3.forward, Vector3.forward
        };

        m_meshTriangle.uv = new Vector2[] {
            new Vector2(0, 0), new Vector2(0.5f, 0f), new Vector2(1f, 0f),
            new Vector2(0, 1f), new Vector2(0.5f, 1f), new Vector2(1f, 1f)
        };

        m_meshRendererTriangle.material = material;

        verticesDefaultPosition = m_meshTriangle.vertices;
    }

    void Update()
    {
        Animation();
    }

    /// <summary>
    /// If the cube is going down, stick vertice 0 and 2 to to vertice 3 and 5, while reseting those last one to initial position
    /// If the cube is going up, stick vertice 3 and 5 to to vertice 0 and 2, while reseting those last one to initial position
    /// also reset the timer of the not used animation
    /// </summary>
    void Animation()
    {
        float timer = cRigidbody.velocity.y > 0 ? animUpTimer : animDownTIMER;
        Vector3[] vertices = m_meshTriangle.vertices;
        if(timer < animTIMER)
        {
            if (cRigidbody.velocity.y < 0)
            {
                //RESTORE UP SIDE
                vertices[3] = Vector2.Lerp(m_meshTriangle.vertices[3], verticesDefaultPosition[3], (timer / animTIMER));
                vertices[5] = Vector2.Lerp(m_meshTriangle.vertices[5], verticesDefaultPosition[5], (timer / animTIMER));
                //FACE DOWN
                vertices[0] = Vector2.Lerp(m_meshTriangle.vertices[0], m_meshTriangle.vertices[3], (timer / animTIMER));
                vertices[2] = Vector2.Lerp(m_meshTriangle.vertices[2], m_meshTriangle.vertices[5], (timer / animTIMER));
            }
            else
            {
                //RESTORE DOWN SIDE
                vertices[0] = Vector2.Lerp(m_meshTriangle.vertices[0], verticesDefaultPosition[0], (timer / animTIMER));
                vertices[2] = Vector2.Lerp(m_meshTriangle.vertices[2], verticesDefaultPosition[2], (timer / animTIMER));

                //FACE UP
                vertices[3] = Vector2.Lerp(m_meshTriangle.vertices[3], m_meshTriangle.vertices[0], (timer / animTIMER));
                vertices[5] = Vector2.Lerp(m_meshTriangle.vertices[5], m_meshTriangle.vertices[2], (timer / animTIMER));
            }
            timer += TimeManager.instance.deltaTime;
            m_meshTriangle.vertices = vertices;
        }

        if (cRigidbody.velocity.y > 0)
        {
            animUpTimer = timer;
            animDownTIMER = 0;
        }
        else
        {
            animDownTIMER = timer;
            animUpTimer = 0;
        }

    }
}
