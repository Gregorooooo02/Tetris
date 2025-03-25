using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TMP_Text[] scoreTexts; // Player 1 and Player 2 score texts
    private int[] scores = {0, 0}; // Player 1 and Player 2 scores

    public void AddScore(int playerIndex, int points) {
        scores[playerIndex] += points;
        UpdateUI(playerIndex);
    }

    private void UpdateUI(int playerIndex) {
        scoreTexts[playerIndex].text = $"Player {playerIndex + 1}: {scores[playerIndex]}";
    }
}
