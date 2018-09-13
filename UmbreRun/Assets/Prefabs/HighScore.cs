using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "HighScore", order = 1)]
public class HighScore : ScriptableObject
{
    public int[] scores = new int[5];
}