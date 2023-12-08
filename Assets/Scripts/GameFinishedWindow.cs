using UnityEngine;
using TMPro;
using System.Linq;
using System.Collections;
using DG.Tweening;

public class GameFinishedWindow : MonoBehaviour {
    public float ShowWaitTime = 0.33f;

    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI MergedDoggosText;

    [ColorUsage(true, true)]
    public Color[] FirstPlaceColors;

    private Vector2 StartPosition;

    private void Awake() {
        StartPosition = (transform as RectTransform).anchoredPosition;
    }

    public void Show(int leaderboardIndex) {
        //todo: zobaczymy, może zrobić nowy layout gdzie wyświetlamy obrazki i ile piesków danego typu było połączonych
        var doggosMerged = PlayerProgress.Instance.MergedDoggos;
        var score = PlayerProgress.Instance.Score;

        MergedDoggosText.text = doggosMerged.Sum(kvp => kvp.Value).ToString();
        ScoreText.text = score.ToString() + ((leaderboardIndex < FirstPlaceColors.Length && leaderboardIndex >= 0) ? $" <color=#{ColorUtility.ToHtmlStringRGB(FirstPlaceColors[leaderboardIndex])}>(#{leaderboardIndex+1} place!)</color>" : "");
        gameObject.SetActive(true);
        
        StartCoroutine(Show_Coro());
    }

    public IEnumerator Show_Coro() {
        (transform as RectTransform).DOAnchorPosY(0, ShowWaitTime);
        yield return new WaitForSeconds(ShowWaitTime);
    }
}
