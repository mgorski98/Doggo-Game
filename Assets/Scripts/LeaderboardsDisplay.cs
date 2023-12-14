using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LeaderboardsDisplay : MonoBehaviour
{
    private List<long> Scores = new();

    [SerializeField]
    private GameObject LeaderBoardEntryPrefab;
    [SerializeField]
    private TextMeshProUGUI NoScoresText;
    [SerializeField]
    private TextMeshProUGUI LeaderboardsDisabledText;
    [SerializeField]
    private RectTransform LeaderboardWidgetsParent;

    public int MaxScoreWidth = 20;

    [ColorUsage(true, true)]
    public Color[] FirstPlaceColors;

    private bool LeaderboardsDisabled = false;

    private void Awake() {
        NoScoresText.gameObject.SetActive(false);
        LeaderboardsDisabledText.gameObject.SetActive(false);

        LeaderboardsDisabled = GameModifiers.Instance.IsAnyModifierActive;
    }

    private void Start() {
        if (LeaderboardsDisabled && SceneManager.GetActiveScene().name != "MenuScene") {
            LeaderboardsDisabledText.gameObject.SetActive(true);
            return;
        }
        Scores = PlayerProgress.ParseLeaderboards(PlayerPrefs.GetString("LEADERBOARDS", ""));

        if (Scores.Count <= 0) {
            NoScoresText.gameObject.SetActive(true);
        } else {
            for (int i = 0; i < Scores.Count; ++i) {
                int place = i + 1;
                long score = Scores[i];
                var obj = Instantiate(LeaderBoardEntryPrefab, LeaderboardWidgetsParent);
                var textComps = obj.GetComponentsInChildren<TextMeshProUGUI>();
                textComps[0].text = $"<align=\"left\">{place}</align>";
                if (i < FirstPlaceColors.Length) {
                    textComps[0].text = $"<color=#{ColorUtility.ToHtmlStringRGB(FirstPlaceColors[i])}>{textComps[0].text}</color>";
                }
                textComps[1].text = $"<align=\"right\">{score}</align>";
            }
        }
    }
}
