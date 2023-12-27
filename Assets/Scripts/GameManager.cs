using Sirenix.Serialization;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : SingletonBehaviour<GameManager> {
    [OdinSerialize]
    public Dictionary<string, GameObject> DoggoPrefabs = new();

    [SerializeField]
    private TimedModeManager TimedModeMgr;
    [SerializeField]
    private AudioClip[] PopSounds;
    [SerializeField]
    private AudioSource ASource;
    [SerializeField]
    private DoggoSpawner Spawner;

    public ObservableValue<int> PickUpsLeft = new(1);
    public int PointsForNextPickUpThreshold = 10_000;

    private int TotalPickUpsEarned;

    public ObservableValue<bool> DoggoPickUpEnabled = new(false);

    public Button PickUpButton;
    public TextMeshProUGUI PickUpsText;

    [OdinSerialize]
    private Dictionary<string, Color> DoggoBlinkColors = new();
    [SerializeField]
    private float BlinkInterval = 0.5f;
    private Coroutine DoggoBlinkCoroutine;
    private bool FirstTimePickingUp = true;
    [SerializeField]
    private TextMeshProUGUI FirstTimePickingUpText;

    private void Start() {
        DoggoPickUpEnabled.onValueChanged.AddListener(_p => this.ToggleDoggoPickUpMode_Internal());
        PickUpsLeft.onValueChanged.AddListener(_p => PickUpButton.interactable = PickUpsLeft.Value > 0);
        PickUpsLeft.onValueChanged.AddListener(_p => {
            PickUpsText.text = $"{PickUpsLeft.Value}x";
        });
    }

    public void QueueMerge(GameObject obj1, GameObject obj2, DoggoData data) {
        if (!data.IsBiggestDoggo) {
            //spawn new one
            var prefab = UnityObjectPool.Instance.Get(data.MergesInto.ID);
            var pos = (obj1.transform.position + obj2.transform.position) / 2f;
            var rot = (obj1.transform.eulerAngles + obj2.transform.eulerAngles) / 2f;
            PlayerProgress.Instance.PlayDoggoBark(data.MergesInto.ID);
            PlayerProgress.Instance.DoggosAppeared.Add(data.MergesInto.ID);
            prefab.transform.SetPositionAndRotation(pos, Quaternion.Euler(rot));
            prefab.SetActive(true);
        }

        PlayerProgress.Instance.Score.Value += data.MergeScore;

        var current = PlayerProgress.Instance.Score.Value / PointsForNextPickUpThreshold;
        if (current != TotalPickUpsEarned) {
            TotalPickUpsEarned++;
            PickUpsLeft.Value++;
        }

        if (!PopSounds.IsEmpty()) {
            ASource.PlayOneShot(PopSounds.RandomElement());
        }

        UnityObjectPool.Instance.Reclaim(data.ID, obj1);
        UnityObjectPool.Instance.Reclaim(data.ID, obj2);
        PlayerProgress.Instance.IncrementMergedDoggos(data.ID, 2);
        if (GameModifiers.Instance.TimedMode && TimedModeMgr != null)
            TimedModeMgr.IncrementTime();
    }

    public void ToggleDoggoPickUpMode() {
        DoggoPickUpEnabled.Value = !DoggoPickUpEnabled.Value;
    }

    private void ToggleDoggoPickUpMode_Internal() {
        FirstTimePickingUpText.gameObject.SetActive(FirstTimePickingUp);
        Spawner.PickUpModeEnabled = !Spawner.PickUpModeEnabled;
        if (DoggoPickUpEnabled) {
            DoggoBlinkCoroutine ??= StartCoroutine(StartBlinking());
        } else {
            StopCoroutine(DoggoBlinkCoroutine);
            DoggoBlinkCoroutine = null;
            FindObjectsOfType<DoggoBehaviour>(false).ForEach(db => db.GetComponent<SpriteRenderer>().color = Color.white);
        }
        FirstTimePickingUp = false;
    }

    private IEnumerator StartBlinking() {
        var currentDoggos = FindObjectsOfType<DoggoBehaviour>(includeInactive: false);
        var currentRenderers = currentDoggos.Select(db => db.GetComponent<SpriteRenderer>()).ToArray();
        bool isNextColorWhite = false;

        while (true) {
            for (int i = 0; i < currentDoggos.Length; ++i) {
                var renderer = currentRenderers[i];
                var id = currentDoggos[i].DoggoData.ID;
                renderer.color = isNextColorWhite ? Color.white : DoggoBlinkColors[id];
            }
            isNextColorWhite = !isNextColorWhite;
            yield return new WaitForSeconds(BlinkInterval);
        }
    }

    public void PickUpDoggo(DoggoBehaviour doggoBehaviour) {
        doggoBehaviour.Reset();
        UnityObjectPool.Instance.Reclaim(doggoBehaviour.DoggoData.ID, doggoBehaviour.gameObject);
        Spawner.HandleDoggoPickedUp(doggoBehaviour);
    }
}
