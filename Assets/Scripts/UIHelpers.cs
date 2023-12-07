using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Localization;

public class UIHelpers : MonoBehaviour
{
    public ConfirmationDialog Dialog;
    public LocalizedString ExitGameText;
    public LocalizedString ExitToMenuText;

    public void ShowExitGameDialog() {
        Dialog.Show(ExitGameText.GetLocalizedString(), () => {
#if !UNITY_EDITOR
            Application.Quit();
#else
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        });
    }

    public void ShowExitToMenuDialog() {
        Dialog.Show(ExitToMenuText.GetLocalizedString(), () => SceneManager.LoadScene("MenuScene"));
    }
}
