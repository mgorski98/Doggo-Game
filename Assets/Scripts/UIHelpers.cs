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
    private ObservableValue<bool> SoundOn = new(false);
    [SerializeField]
    private ObservableValue<bool> MusicOn = new(false);

    [SerializeField]
    private MusicPlayer MusicPlayer;

    public ToggleButton SoundToggleButton;
    public ToggleButton MusicToggleButton;

    private void Awake() {
        SoundOn.onValueChanged.AddListener(p => {
            var newval = p.Item2;
            Mixer.SetFloat("SoundVolume", Mathf.Log10(p.Item2 ? 1f : 0.0001f) * 20);
            SoundToggleButton.Toggled = newval;
            PlayerPrefs.SetInt("SOUND_VOLUME", newval.ToInt());
        });
        MusicOn.onValueChanged.AddListener(p => { 
            var newval = p.Item2;
            Mixer.SetFloat("MusicVolume", Mathf.Log10(p.Item2 ? 1f : 0.0001f) * 20);
            MusicToggleButton.Toggled = newval;
            PlayerPrefs.SetInt("MUSIC_VOLUME", newval.ToInt());
        });
        UpdateAudioButtons();
    }

    private void UpdateAudioButtons() {
        float soundVolume = PlayerPrefs.GetInt("SOUND_VOLUME", 1).ToBoolean() ? 1f : 0f;
        float musicVolume = PlayerPrefs.GetInt("MUSIC_VOLUME", 1).ToBoolean() ? 1f : 0f;
        SoundOn.Value = soundVolume > 0;
        MusicOn.Value = musicVolume > 0;
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
        MusicOn.Value = !MusicOn;
    }

    public void ToggleSounds() {
        SoundOn.Value = !SoundOn;
    }
}

