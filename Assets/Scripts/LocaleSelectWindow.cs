using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class LocaleSelectWindow : SerializedMonoBehaviour
{
    [ShowInInspector]
    private int LocaleID = 0;

    [SerializeField]
    private ToggleButtonGroup ToggleGroup;
    [SerializeField]
    private RectTransform SelectionButtonsParent;
    [SerializeField]
    private GameObject SelectionButtonPrefab;
    [SerializeField]
    private Button ConfirmButton;
    [OdinSerialize]
    public Dictionary<string, Sprite> LocaleIcons;

    private void Awake() {
        var locales = LocalizationSettings.AvailableLocales.Locales;

        for (int i = 0; i < locales.Count; ++i) {
            var obj = Instantiate(SelectionButtonPrefab, SelectionButtonsParent);
            var button = obj.GetComponent<ToggleButton>();
            ToggleGroup.Add(button);
            button.onClick.AddListener(() => {
                LocaleID = i;
            });
            obj.GetComponent<Image>().sprite = LocaleIcons[locales[i].Identifier.ToString()];
        }
    }

    public void ConfirmSelection() {
        I18n.Instance.ChangeLocale(LocaleID);
        //todo: store locale in playerprefs
    }

    private void Update() {
        ConfirmButton.interactable = ToggleGroup.Selected != null;
    }
}
