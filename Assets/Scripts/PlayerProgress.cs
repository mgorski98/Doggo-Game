using TMPro;
using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;

public class PlayerProgress : SerializedMonoBehaviour {
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI GameTimeText;

    public long Score;
    public Dictionary<string, int> MergedDoggos = new();

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
