using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Collections;

public class MenuController : SerializedMonoBehaviour
{
    public GameObject Buttons;
    public GameObject Locales;
    public GameObject Leaderboards;
    public GameObject Credits;
    public GameObject GameModifiers;

    public Sprite[] DoggoHeadImages;
    public ConstantRotator[] RotatingHeads;

    public (float, float) FlyingDoggoSpawningTimeRanges;
    public (float, float) FlyingDoggoYPosSpawningRanges;
    public (float, float) FlyingDoggosSpeedRange;
    public GameObject FlyingDoggoPrefab;

    private void Awake() {
        foreach (var head in RotatingHeads) {
            head.GetComponent<Image>().sprite = DoggoHeadImages[Random.Range(0,DoggoHeadImages.Length)];
        }
    }

    private void Start() {
        //StartCoroutine(SpawnDoggos());
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

    public void ShowGameModifiersWindow() {
        Buttons.SetActive(false);
        Credits.SetActive(false);
        Leaderboards.SetActive(false);
        Locales.SetActive(false);
        GameModifiers.SetActive(true);
    }

    public void ShowCredits() {
        Buttons.SetActive(false);
        Credits.SetActive(true);
        Leaderboards.SetActive(false);
        Locales.SetActive(false);
        GameModifiers.SetActive(false);
    }

    public void ShowMenuAgain() {
        Buttons.SetActive(true);
        Credits.SetActive(false);
        Leaderboards.SetActive(false);
        Locales.SetActive(false);
        GameModifiers.SetActive(false);
    }

    private IEnumerator SpawnDoggos() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(FlyingDoggoSpawningTimeRanges.Item1, FlyingDoggoSpawningTimeRanges.Item2));

            float direction = Random.Range(0f, 100f) <= 50f ? -1f : 1f;
        }
    }
}
