using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoggosProgressionPanel : MonoBehaviour
{
    private DoggoData[] Doggos;

    [SerializeField]
    private GameObject ProgressionPrefab;
    [SerializeField]
    private RectTransform ProgressionPanel;

    private void Awake()
    {
        Doggos = Resources.LoadAll<DoggoData>("Doggos");
    }

    private void Start() {
        foreach (var doggoData in Doggos) {
            //set up the view
        }
    }
}
