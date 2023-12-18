using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using TMPro;

public class MenuController : SerializedMonoBehaviour
{
    public GameObject Buttons;
    public GameObject Locales;
    public GameObject Leaderboards;
    public GameObject Credits;
    public GameObject GameModifiers;

    public Sprite[] DoggoHeadImages;
    public ConstantRotator[] RotatingHeads;

    public string ShootingDoggoPoolID;
    public (float, float) ShootingStrengthRanges;
    public (float, float) ShootingAngleRanges;
    public (float, float) ShootingTimeRanges;
    public float YViewportOffset = 0.2f;

    private Camera cam;

    public Transform DoggoHeadsParent;

    public LocalizedString[] VersionLocalization;
    public TextMeshProUGUI[] VersionTextComponents;

    public ToggleButton SoundToggleButton;
    public ToggleButton MusicToggleButton;

    private void Awake() {
        foreach (var head in RotatingHeads) {
            head.GetComponent<Image>().sprite = DoggoHeadImages.RandomElement();
        }

        cam = Camera.main;
        AssignVersionTranslations();

        LocalizationSettings.SelectedLocaleChanged += LocaleChanged;
    }

    private void OnDestroy() {
        LocalizationSettings.SelectedLocaleChanged -= LocaleChanged;
    }

    private void LocaleChanged (Locale _l) {
        AssignVersionTranslations();
    }

    private void Start() {
        StartCoroutine(StartLaunchingDoggosInTheAir());
    }

    private void AssignVersionTranslations() {
        for (int i = 0; i < VersionLocalization.Length; ++i) {
            VersionTextComponents[i].text = string.Format(VersionLocalization[i].GetLocalizedString(), Application.version);
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
        Buttons.SetActive(false);
        Credits.SetActive(false);
        Leaderboards.SetActive(true);
        Locales.SetActive(false);
        GameModifiers.SetActive(false);
    }

    public void ShowLocaleSelect() {
        Buttons.SetActive(false);
        Credits.SetActive(false);
        Leaderboards.SetActive(false);
        Locales.SetActive(true);
        GameModifiers.SetActive(false);
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

    private IEnumerator StartLaunchingDoggosInTheAir() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(ShootingTimeRanges.Item1, ShootingTimeRanges.Item2));

            var positionOutsideViewport = new Vector2(0, YViewportOffset);
            var pos = cam.ViewportToWorldPoint(positionOutsideViewport);

            Vector2 shootingDirection = new Vector2(
                Random.Range(ShootingAngleRanges.Item1, ShootingAngleRanges.Item2),
                1f
            ).normalized;
            float shootingStrength = Random.Range(ShootingStrengthRanges.Item1, ShootingStrengthRanges.Item2);
            var obj = UnityObjectPool.Instance.Get(ShootingDoggoPoolID);
            obj.transform.position = pos.Copy(x: 0f, z: 0f);
            obj.transform.SetParent(DoggoHeadsParent);
            obj.GetComponent<Rigidbody2D>().AddForce(shootingDirection * shootingStrength, ForceMode2D.Impulse);
            obj.GetComponentInChildren<SpriteRenderer>().sprite = DoggoHeadImages.RandomElement();
            obj.GetComponent<ConstantRotator>().RotationSpeed = shootingStrength * 4 * -Mathf.Sign(shootingDirection.x);

            StartCoroutine(ReclaimDoggoHeadAfterDelay(obj));
        } 
    }

    private IEnumerator ReclaimDoggoHeadAfterDelay(GameObject doggoHeadObj) {
        yield return new WaitForSeconds(5f);

        UnityObjectPool.Instance.Reclaim(ShootingDoggoPoolID, doggoHeadObj);
    }
}
