using UnityEngine;

public class Rain : MonoBehaviour
{
    private void Start()
    {
        EnvironmentManager.Instance.RegisterRain(this);
    }

    private void Update()
    {
    }
}
