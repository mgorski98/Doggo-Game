using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Localization;
using UnityEngine.Audio;

public class UIHelpers : MonoBehaviour
{
    public DoggoConfirmationDialog Dialog;
    public LocalizedString ExitGameText;
    public LocalizedString ExitToMenuText;

    public AudioMixer Mixer;

    [SerializeField]
    private bool SoundOn;
    [SerializeField]
    private bool MusicOn;

    [SerializeField]
    private MusicPlayer MusicPlayer;

    private void Awake() {
        //todo: update buttons
    }

    public void ShowExitGameDialog() {
        if (PlayerProgress.Instance.GameOver)
            return;
        Dialog.Show(ExitGameText.GetLocalizedString(), this.ExitGame);
    }

    public void ShowExitToMenuDialog() {
        if (PlayerProgress.Instance.GameOver)
            return;
        Dialog.Show(ExitToMenuText.GetLocalizedString(), () => SceneManager.LoadScene("MenuScene"));
    }

    public void ExitGame() {
#if !UNITY_EDITOR
            Application.Quit();
#else
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void ToggleRetroMode() => GameModifiers.Instance.RetroMode = !GameModifiers.Instance.RetroMode;

    public void ToggleMusic() {
        MusicOn = !MusicOn;
        //Mixer.SetFloat("MusicVolume", Mathf.Log10(0.0001f) * 20);
    }

    public void ToggleSounds() {
        SoundOn = !SoundOn;
        //Mixer.SetFloat("SoundVolume", Mathf.Log10(0.0001f) * 20);
    }
}

