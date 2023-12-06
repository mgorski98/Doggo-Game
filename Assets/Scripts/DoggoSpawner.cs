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

    private void Awake() {
        var tempList = new List<GameObject>();
        foreach (var data in PossibleSpawnedDoggosData) {
            tempList.AddRange(Enumerable.Repeat(data.DoggoPrefab, data.Weight));
        }
        DoggoGenerator = tempList.ToArray();
        cam = Camera.main;
    }

    private void Start() {
        WaitForNewDoggo(0f);
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0) && InputEnabled) {
            Instantiate(CurrentDoggoSelected, SpawnPoint.position, Quaternion.identity);
            CurrentDoggoShown.sprite = null;
            WaitForNewDoggo(SpawnWaitTime);
        }
    }

    private void LateUpdate() {
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
        var doggoData = CurrentDoggoSelected.GetComponent<DoggoBehaviour>().DoggoData;
        CurrentDoggoShown.sprite = doggoData.DoggoIcon;
        InputEnabled = true;
    }
}
