﻿using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;

public class MergeManager : SingletonBehaviour<MergeManager> {
    [OdinSerialize]
    public Dictionary<string, GameObject> DoggoPrefabs = new();

    [SerializeField]
    private TimedModeManager TimedModeMgr;
    [SerializeField]
    private AudioClip[] PopSounds;
    [SerializeField]
    private AudioSource ASource;

    public void QueueMerge(GameObject obj1, GameObject obj2, DoggoData data) {
        if (!data.IsBiggestDoggo) {
            //spawn new one
            var prefab = UnityObjectPool.Instance.Get(data.MergesInto.ID);
            var pos = (obj1.transform.position + obj2.transform.position) / 2f;
            var rot = (obj1.transform.eulerAngles + obj2.transform.eulerAngles) / 2f;
            prefab.transform.SetPositionAndRotation(pos, Quaternion.Euler(rot));
            prefab.SetActive(true);
        }

        int score = data.MergeScore;
        PlayerProgress.Instance.Score.Value += score;

        if (data.BarkSounds.Length > 0) {
            var clip = data.BarkSounds.RandomElement();
            AudioSource.PlayClipAtPoint(clip, Vector3.zero);
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
}
