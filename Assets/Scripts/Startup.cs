using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;
using UnityEngine.SceneManagement;


public class Startup : MonoBehaviour {
    private void Awake() {
        var selectedLang = PlayerPrefs.GetString("LANGUAGE_ID", "English(en)");
        var index = 0;
        for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; ++i) {
            var locale = LocalizationSettings.AvailableLocales.Locales[i];
            if (locale.Identifier.ToString() == selectedLang) {
                index = i;
                break;
            }
        }
        I18n.Instance.ChangeLocale(index);
    }

    private void Update() {
        if (!I18n.Instance.ChangeInProgress) {
            SceneManager.LoadScene("MenuScene");
        }
    }
}
