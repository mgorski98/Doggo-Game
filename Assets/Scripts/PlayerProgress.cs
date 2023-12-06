using TMPro;
using UnityEngine;

public class PlayerProgress : MonoBehaviour {
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI GameTimeText;

    public long Score;

    public static PlayerProgress Instance { get; private set; } = null;

    private void Awake() {
        Instance = this;
    }

    private void OnDestroy() {
        if (Instance == this) {
            Instance = null;
        }
    }

    public void AddScore(int score) {
        this.Score += score;
        ScoreText.text = this.Score.ToString();
    }
}
