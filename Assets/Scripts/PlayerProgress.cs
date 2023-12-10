using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;

public class PlayerProgress : SingletonBehaviour<PlayerProgress> {
    public ScoreCountingLabel ScoreText;

    public DoggoSpawner Spawner;

    public ObservableValue<long> Score = new(0L);
    public Dictionary<string, int> MergedDoggos = new();

    public GameFinishedWindow GameOverWindow;

    public const int MAX_LEADERBOARDS_SIZE = 10;

    public bool GameOver = false;

    protected override void Awake() {
        base.Awake();

        this.Score.onValueChanged.AddListener(p => ScoreText.StartCounting(p.Item2));
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
            var scores = ParseLeaderboards(leaderboards);
            scores.Add(this.Score);
            scores = scores.OrderByDescending(l => l).Take(Mathf.Min(MAX_LEADERBOARDS_SIZE, scores.Count)).ToList();
            updatedLeaderboards = string.Join(',', scores);
            result = scores.IndexOf(this.Score);
        }
        PlayerPrefs.SetString("LEADERBOARDS", updatedLeaderboards);
        return result;
    }

    public List<long> ParseLeaderboards(string input) => string.IsNullOrWhiteSpace(input) ? new() : input.Split(',').Select(long.Parse).OrderByDescending(l => l).ToList();
}
