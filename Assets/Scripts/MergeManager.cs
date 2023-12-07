using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;

public class MergeManager : SerializedMonoBehaviour {
    [OdinSerialize]
    public Dictionary<string, GameObject> DoggoPrefabs = new();
    public static MergeManager Instance { get; private set; }

    private void Awake() {
        Instance = this;
    }

    private void OnDestroy() {
        if (Instance == this)
            Instance = null;
    }

    public void QueueMerge(GameObject obj1, GameObject obj2, DoggoData data) {
        if (!data.IsBiggestDoggo) {
            //spawn new one
            var prefab = DoggoPrefabs[data.MergesInto.ID];
            var pos = (obj1.transform.position + obj2.transform.position) / 2f;
            var rot = (obj1.transform.eulerAngles + obj2.transform.eulerAngles) / 2f;
            Instantiate(prefab, pos, Quaternion.Euler(rot));
        }

        int score = data.MergeScore;
        PlayerProgress.Instance.AddScore(score);

        if (data.BarkSounds.Length > 0) {
            var clip = data.BarkSounds[Random.Range(0, data.BarkSounds.Length)];
            AudioSource.PlayClipAtPoint(clip, Vector3.zero);
        }

        Destroy(obj1);
        Destroy(obj2);
        PlayerProgress.Instance.IncrementMergedDoggos(data.ID, 2);
    }
}
