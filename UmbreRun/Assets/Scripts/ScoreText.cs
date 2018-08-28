using UnityEngine;

public class ScoreText : MonoBehaviour
{
    [SerializeField]
    UnityEngine.UI.Text textVal = null;

    private void Awake()
    {
        if (textVal == null)
        {
            Debug.LogWarning("ScoreText.Awake() - text should be set in editor!");
            textVal = GetComponent<UnityEngine.UI.Text>();

            if (textVal == null)
            {
                Debug.LogError("ScoreText.Awake() - no Text exists!");
            }
        }
    }

    private void Start()
    {
        ScoreManager.Instance.OnScoreChanged += HandleScoreChange;
	}

    private void OnDestroy()
    {
        if (ScoreManager.Instance)
            ScoreManager.Instance.OnScoreChanged -= HandleScoreChange;
    }

    private void HandleScoreChange(int newScore)
    {
        textVal.text = newScore + " m";
    }
}
