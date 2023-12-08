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

    public GameObject ShootingDoggoPrefab;
    public (float, float) ShootingStrengthRanges;
    public (float, float) ShootingAngleRanges;
    public (float, float) ShootingTimeRanges;

    private void Awake() {
        foreach (var head in RotatingHeads) {
            head.GetComponent<Image>().sprite = DoggoHeadImages[Random.Range(0,DoggoHeadImages.Length)];
        }
    }

    private void Start() {
        //StartCoroutine(SpawnFlyingDoggos());
        //StartCoroutine(StartLaunchingDoggosInTheAir());
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

    private IEnumerator SpawnFlyingDoggos() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(FlyingDoggoSpawningTimeRanges.Item1, FlyingDoggoSpawningTimeRanges.Item2));

            float direction = Random.Range(0f, 100f) <= 50f ? -1f : 1f;
            Vector3 spawnPosition = new Vector3();
            float speed = Random.Range(FlyingDoggosSpeedRange.Item1, FlyingDoggosSpeedRange.Item2);
            var obj = Instantiate(FlyingDoggoPrefab, spawnPosition, Quaternion.identity);
            var comp = obj.GetComponent<FlyingMenuDoggoHead>();
        }
    }

    private IEnumerator StartLaunchingDoggosInTheAir() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(ShootingTimeRanges.Item1, ShootingTimeRanges.Item2));

            Vector2 shootingDirection = new Vector2(Mathf.Clamp(Random.insideUnitCircle.x, ShootingAngleRanges.Item1, ShootingAngleRanges.Item2), Mathf.Abs(Random.insideUnitCircle.y));
            Vector2 spawnPosition = new();
            float shootingStrength = Random.Range(ShootingStrengthRanges.Item1, ShootingStrengthRanges.Item2);
            var obj = Instantiate(ShootingDoggoPrefab, spawnPosition, Quaternion.identity);
            obj.GetComponent<Rigidbody2D>().AddForce(shootingDirection * shootingStrength, ForceMode2D.Impulse);
        } 
    }
}
