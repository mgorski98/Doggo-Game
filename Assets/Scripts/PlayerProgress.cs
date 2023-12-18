﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlayerProgress : SingletonBehaviour<PlayerProgress> {
    public ScoreCountingLabel ScoreText;

    public DoggoSpawner Spawner;

    public ObservableValue<long> Score = new(0L);
    public Dictionary<string, int> MergedDoggos = new();
    public HashSet<string> DoggosAppeared = new();
    public Dictionary<string, DoggoData> DoggoDatas = new();

    public GameFinishedWindow GameOverWindow;

    public const int MAX_LEADERBOARDS_SIZE = 10;

    public bool GameOver = false;

    [SerializeField]
    private AudioSource ASource;

    protected override void Awake() {
        base.Awake();

        this.Score.onValueChanged.AddListener(p => ScoreText.StartCounting(p.Item2));
        Resources.LoadAll<DoggoData>("Doggos").ForEach(data => DoggoDatas.Add(data.ID, data));
    }

    public void AddAppearedDoggo(string ID) => DoggosAppeared.Add(ID);
    public bool HasAppearedAlready(string ID) => DoggosAppeared.Contains(ID);
    public void PlayDoggoBark(string ID) {
        var probabilityToPlay = !HasAppearedAlready(ID) ? 1f : 0.2f;
        var shouldPlay = Random.value <= probabilityToPlay;
        if (!shouldPlay)
            return;

        var prefab = GameManager.Instance.DoggoPrefabs[ID];
        var data = prefab.GetComponent<DoggoBehaviour>().DoggoData;
        if (!data.BarkSounds.IsEmpty()) {
            ASource.PlayOneShot(data.BarkSounds.RandomElement());
        }
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
        return UpdateLeaderboards(this.Score);
    }

    public static int UpdateLeaderboards(long score) {
        if (GameModifiers.Instance.IsAnyModifierActive) {
            return -1;
        }
        var leaderboards = PlayerPrefs.GetString("LEADERBOARDS", "");
        int result = 0;
        string updatedLeaderboards = "";
        if (string.IsNullOrWhiteSpace(leaderboards)) {
            updatedLeaderboards = score.ToString();
        } else {
            var scores = ParseLeaderboards(leaderboards);
            scores.Add(score);
            scores = scores.OrderByDescending(l => l).Take(Mathf.Min(MAX_LEADERBOARDS_SIZE, scores.Count)).ToList();
            updatedLeaderboards = string.Join(',', scores);
            result = scores.IndexOf(score);
        }
        PlayerPrefs.SetString("LEADERBOARDS", updatedLeaderboards);
        return result;
    }

    public static List<long> ParseLeaderboards(string input) => string.IsNullOrWhiteSpace(input) ? new() : input.Split(',').Select(long.Parse).OrderByDescending(l => l).ToList();
}
