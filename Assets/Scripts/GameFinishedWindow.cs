﻿using UnityEngine;
using TMPro;
using System.Linq;
using DG.Tweening;

public class GameFinishedWindow : MonoBehaviour {
    public float ShowWaitTime = 0.33f;

    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI MergedDoggosText;

    [ColorUsage(true, true)]
    public Color[] FirstPlaceColors;

    public float TargetStatsDisplayX;
    public RectTransform DoggoStatsParent;
    public RectTransform DoggoStatsWindow;
    public GameObject DoggoStatImageCounterDisplayPrefab;

    public void Show(int leaderboardIndex) {
        var doggosMerged = PlayerProgress.Instance.MergedDoggos;
        var score = PlayerProgress.Instance.Score;

        MergedDoggosText.text = doggosMerged.Sum(kvp => kvp.Value).ToString();
        ScoreText.text = score.ToString() + ((leaderboardIndex < FirstPlaceColors.Length && leaderboardIndex >= 0) ? $" <color=#{ColorUtility.ToHtmlStringRGB(FirstPlaceColors[leaderboardIndex])}>(#{leaderboardIndex+1} place!)</color>" : "");
        gameObject.SetActive(true);

        ShowWindow();
    }

    public void ShowWindow() {
        (transform as RectTransform).DOAnchorPosY(0, ShowWaitTime);
    }

    public void ShowMergeDetails() {
        PlayerProgress.Instance.MergedDoggos.ForEach(kvp => {
            var (doggoId, count) = kvp;
            var data = PlayerProgress.Instance.DoggoDatas[doggoId];
            var obj = Instantiate(DoggoStatImageCounterDisplayPrefab, DoggoStatsParent);
            var counter = obj.GetComponent<ImageCounter>();
            counter.Image.sprite = data.DoggoIcon;
            counter.UpdateDisplay(count);
        });
        DoggoStatsWindow.gameObject.SetActive(true);
        DoggoStatsWindow.DOAnchorPosX(TargetStatsDisplayX, ShowWaitTime);
    }
}
