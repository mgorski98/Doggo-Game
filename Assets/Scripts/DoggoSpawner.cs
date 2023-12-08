using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

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
    private GameObject CurrentDoggoSelected;
    [OdinSerialize]
    private float SpawnWaitTime = 1f;
    private bool InputEnabled = true;
    private Camera cam;
    private Quaternion spawnRotation;

    private void Awake() {
        var tempList = new List<GameObject>();
        foreach (var data in PossibleSpawnedDoggosData) {
            tempList.AddRange(Enumerable.Repeat(data.DoggoPrefab, data.Weight));
        }
        DoggoGenerator = tempList.ToArray();
        cam = Camera.main;

        if (GameModifiers.Instance.InvisibleDoggoPreview)
            CurrentDoggoShown.enabled = false;
    }

    private void Start() {
        WaitForNewDoggo(0f);
    }

    private void Update() {
        if (GameOver)
            return;
        if (Input.GetMouseButtonDown(0) && InputEnabled) {
            Instantiate(CurrentDoggoSelected, SpawnPoint.position, spawnRotation);
            CurrentDoggoShown.sprite = null;
            WaitForNewDoggo(SpawnWaitTime);
        }
    }

    private void LateUpdate() {
        if (GameOver)
            return;
        MoveSpawner();
    }

    private void MoveSpawner() {
        var pos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        transform.position = new Vector3(
            Mathf.Clamp(pos.x, XSpawnerBoundaries.Min, XSpawnerBoundaries.Max),
            transform.position.y,
            transform.position.z
        );
    }

    private void GenerateNewDoggo() {
        CurrentDoggoSelected = DoggoGenerator[Random.Range(0, this.DoggoGenerator.Length)];
    }

    private void WaitForNewDoggo(float waitTime) {
        InputEnabled = false;
        StartCoroutine(WaitForNewDoggo_Coro(waitTime));
    }

    private IEnumerator WaitForNewDoggo_Coro(float spawnTime) {
        yield return new WaitForSeconds(spawnTime);
        GenerateNewDoggo();
        spawnRotation = Quaternion.Euler(Vector3.forward * Random.Range(-360f, 360f));
        var doggoData = CurrentDoggoSelected.GetComponent<DoggoBehaviour>().DoggoData;
        CurrentDoggoShown.sprite = doggoData.DoggoIcon;
        CurrentDoggoShown.transform.localScale = CurrentDoggoSelected.transform.localScale;
        CurrentDoggoShown.transform.rotation = spawnRotation;
        InputEnabled = true;
    }
}
