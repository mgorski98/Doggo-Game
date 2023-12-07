using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using System.Collections.Generic;

public class MenuController : SerializedMonoBehaviour
{
    public GameObject Buttons;
    public GameObject Locales;
    public GameObject Leaderboards;
    public GameObject Credits;

    public Sprite[] DoggoHeadImages;

    public RectTransform[] PossibleSpawnPositions;
    public (int, int) RotatingDoggosCountRange;
    public (float, float) RotatingDoggosSpeedRange;

    private void Awake() {
        if (PossibleSpawnPositions.Length <= 0)
            return;

        int doggosCount = Random.Range(RotatingDoggosCountRange.Item1, RotatingDoggosCountRange.Item2);
        var positions = new HashSet<RectTransform>();

        while (positions.Count < doggosCount) {
            positions.Add(PossibleSpawnPositions[Random.Range(0,PossibleSpawnPositions.Length)]);
        }

        foreach (var position in positions) {
            var img = position.GetComponent<Image>();
            img.sprite = DoggoHeadImages[Random.Range(0, DoggoHeadImages.Length)];
            position.GetComponent<ConstantRotator>().SetRotationSpeed(Random.Range(RotatingDoggosSpeedRange.Item1, RotatingDoggosSpeedRange.Item2));
            position.gameObject.SetActive(true);
        }
    }

    public void ExitGame() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void ShowLeaderboards() {

    }

    public void ShowLocaleSelect() {

    }

    public void ShowCredits() {
        Buttons.SetActive(false);
        Credits.SetActive(true);
        Leaderboards.SetActive(false);
        Locales.SetActive(false);
    }

    public void ShowMenuAgain() {
        Buttons.SetActive(true);
        Credits.SetActive(false);
        Leaderboards.SetActive(false);
        Locales.SetActive(false);
    }
}
