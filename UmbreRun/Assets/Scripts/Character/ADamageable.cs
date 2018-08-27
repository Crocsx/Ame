using UnityEngine;

public abstract class ADamageable : MonoBehaviour
{
    [SerializeField]
    private Collider2D m_collider;
    public Collider2D Collider
    {
        get { return m_collider; }
    }

    public virtual void OnHitObstacle()
    {
        GameManager.Instance.NotifyLose();
    }
}
