using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;

public class DoggoSpawner : SerializedMonoBehaviour
{
    [System.Serializable]
    public class DoggoSpawnProbability {
        public GameObject DoggoPrefab;
        public int Weight;
    }

    public bool GameOver = false;

    public Transform SpawnPoint;
    public (float Min, float Max) XSpawnerBoundaries;
    public DoggoSpawnProbability[] PossibleSpawnedDoggosData;
    private GameObject[] DoggoGenerator;
    [OdinSerialize]
    private SpriteRenderer CurrentDoggoShown;
    private DoggoBehaviour CurrentDoggoSelected;
    [OdinSerialize, Min(0.3f)]
    private float SpawnWaitTime = 1f;
    [OdinSerialize]
    private bool InputEnabled = true;
    private Camera cam;
    private Quaternion spawnRotation;
    [OdinSerialize]
    private GameObject DropPositionIndicator;
    [SerializeField]
    private float DoggoHeadTweeningTime = 0.2f;

    public bool PickUpModeEnabled = false;

    private void Awake() {
        var tempList = new List<GameObject>();
        foreach (var data in PossibleSpawnedDoggosData) {
            tempList.AddRange(Enumerable.Repeat(data.DoggoPrefab, data.Weight));
        }
        DoggoGenerator = tempList.ToArray();
        cam = Camera.main;

        if (GameModifiers.Instance.InvisibleDoggoPreview) {
            CurrentDoggoShown.enabled = false;
            DropPositionIndicator.SetActive(true);
        }
    }

    private void Start() {
        WaitForNewDoggo(0f);
    }

    private void Update() {
        if (GameOver)
            return;
        var isPressingUIButton = Utils.IsPointerOverUIElement();
        if (IsDoggoDropRequested() && InputEnabled && !isPressingUIButton && !PickUpModeEnabled) {
            var obj = UnityObjectPool.Instance.Get(CurrentDoggoSelected.DoggoData.ID);
            obj.transform.SetPositionAndRotation(SpawnPoint.position, spawnRotation);
            obj.SetActive(true);
            CurrentDoggoShown.sprite = null;
            WaitForNewDoggo(SpawnWaitTime);
        }
    }

    private void LateUpdate() {
        if (GameOver)
            return;
        if (!PickUpModeEnabled)
            MoveSpawner();
    }

    private void MoveSpawner() {
        transform.position = GetNewSpawnerPosition();
    }

    private bool IsDoggoDropRequested() {
#if UNITY_ANDROID
        if (Input.touchCount > 0) {
            Touch t = Input.GetTouch(0);
            return t.phase == TouchPhase.Ended;
        } else return false;
#else
        return Input.GetMouseButtonDown(0);
#endif
    }

    private Vector3 GetNewSpawnerPosition() {
        var targetPosition = transform.position;
#if UNITY_ANDROID
        if (Input.touchCount > 0 && !Utils.IsPointerOverUIElement()) {
            Touch t = Input.GetTouch(0);
            var converted = cam.ScreenToWorldPoint(t.position);
            targetPosition = new Vector3(Mathf.Clamp(converted.x, XSpawnerBoundaries.Min, XSpawnerBoundaries.Max), targetPosition.y, targetPosition.z);
        }
#else
        var pos = cam.ScreenToWorldPoint(Input.mousePosition.Copy(z: 0f));
        targetPosition = new Vector3(
            Mathf.Clamp(pos.x, XSpawnerBoundaries.Min, XSpawnerBoundaries.Max),
            transform.position.y,
            transform.position.z
        );
#endif
        return targetPosition;
    }

    private void GenerateNewDoggo() {
        CurrentDoggoSelected = DoggoGenerator.RandomElement().GetComponent<DoggoBehaviour>();
    }

    private void WaitForNewDoggo(float waitTime) {
        InputEnabled = false;
        StartCoroutine(WaitForNewDoggo_Coro(waitTime));
    }

    private IEnumerator WaitForNewDoggo_Coro(float spawnTime) {
        GenerateNewDoggo();
        yield return new WaitForSeconds(spawnTime);

        ShowSelectedDoggo();

        PlayerProgress.Instance.PlayDoggoBark(CurrentDoggoSelected.DoggoData.ID);
        InputEnabled = true;
    }

    public void EnableInput() => InputEnabled = true;
    public void DisableInput() => InputEnabled = false;

    public void HandleDoggoPickedUp(DoggoBehaviour doggoBehaviour) {
        CurrentDoggoSelected = doggoBehaviour;
        ShowSelectedDoggo();
        Invoke(nameof(DisablePickUpMode), 0.05f); //because the doggo was immediately dropped...
        GameManager.Instance.PickUpsLeft.Value--;
    }

    private void DisablePickUpMode() => GameManager.Instance.DoggoPickUpEnabled.Value = false;

    private void ShowSelectedDoggo() {
        spawnRotation = Quaternion.Euler(Vector3.forward * Random.Range(-360f, 360f));
        var doggoData = CurrentDoggoSelected.GetComponent<DoggoBehaviour>().DoggoData;

        CurrentDoggoShown.sprite = doggoData.DoggoIcon;
        CurrentDoggoShown.transform.localScale = Vector3.zero;
        CurrentDoggoShown.transform.DOScale(CurrentDoggoSelected.transform.localScale, DoggoHeadTweeningTime);
        CurrentDoggoShown.transform.DORotateQuaternion(spawnRotation, DoggoHeadTweeningTime);
    }
}
