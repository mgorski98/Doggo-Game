using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DoggosProgressionPanel : MonoBehaviour
{
    private DoggoData[] Doggos;

    [SerializeField]
    private GameObject ProgressionPrefab;
    [SerializeField]
    private RectTransform ProgressionPanel;
    [SerializeField]
    private Sprite POWSprite;
    [SerializeField]
    private Scrollbar SBar;

    private void Awake()
    {
        Doggos = Resources.LoadAll<DoggoData>("Doggos").OrderBy(d => d.MergeScore).ToArray();
    }

    private void Start() {
        for (int i = 0; i < Doggos.Length; ++i) {
            var data = Doggos[i];
            var widget = Instantiate(ProgressionPrefab, ProgressionPanel);
            widget.transform.Find("from").GetComponent<Image>().sprite = data.DoggoIcon;
            widget.transform.Find("to").GetComponent<Image>().sprite = data.IsBiggestDoggo ? POWSprite : Doggos[i + 1].DoggoIcon;
        }

        SBar.value = 1;
    }
}
