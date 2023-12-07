using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;

public class PlayerProgress : SerializedMonoBehaviour {
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI GameTimeText;

    public DoggoSpawner Spawner;

    public long Score;
    public Dictionary<string, int> MergedDoggos = new();

    public GameFinishedWindow GameOverWindow;

    public const int MAX_LEADERBOARDS_SIZE = 10;

    public bool GameOver = false;

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

    public void IncrementMergedDoggos(string doggoId, int incrBy) {
        if (MergedDoggos.TryGetValue(doggoId, out int count)) {
            MergedDoggos[doggoId] = count + incrBy;
        } else {
            MergedDoggos[doggoId] = incrBy;
        }
    }

    public void HandleGameOver() {
        GameOver = true;
        Spawner.GameOver = true;
        int scoreIndex = UpdateLeaderboards();
        GameOverWindow.Show(scoreIndex);
    }

    private int UpdateLeaderboards() {
        var leaderboards = PlayerPrefs.GetString("LEADERBOARDS", "");
        int result = 0;
        string updatedLeaderboards = "";
        if (string.IsNullOrWhiteSpace(leaderboards)) {
            updatedLeaderboards = this.Score.ToString();
        } else {
            var scores = leaderboards.Split(",").Select(long.Parse).ToList();
            scores.Sort();
            for (int i = 0; i < scores.Count; ++i) {
                if (this.Score > scores[i]) {
                    result = i;
                    scores[i] = this.Score;
                    break;
                }
            }
            updatedLeaderboards = string.Join(',', scores.Select(i => i.ToString()));
        }
        PlayerPrefs.SetString("LEADERBOARDS", updatedLeaderboards);
        return result;
    }
}
