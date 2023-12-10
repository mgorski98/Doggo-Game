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
            var targetVol = p.Item2 ? 1f : 0.0001f;
            Mixer.SetFloat("SoundVolume", targetVol.ToDecibels());
            SoundToggleButton.Toggled = p.Item2;
            PlayerPrefs.SetInt("SOUND_VOLUME", p.Item2.ToInt());
        });
        MusicOn.onValueChanged.AddListener(p => {
            var targetVol = p.Item2 ? 1f : 0.0001f;
            Mixer.SetFloat("MusicVolume", targetVol.ToDecibels());
            MusicToggleButton.Toggled = p.Item2;
            PlayerPrefs.SetInt("MUSIC_VOLUME", p.Item2.ToInt());

            if (MusicPlayer != null)
                MusicPlayer.MusicVolume = targetVol;
        });

        Mixer.SetFloat("MusicVolume", (PlayerPrefs.GetInt("MUSIC_VOLUME", 1).ToBoolean() ? 1f : 0.0001f).ToDecibels());
        Mixer.SetFloat("SoundVolume", (PlayerPrefs.GetInt("SOUND_VOLUME", 1).ToBoolean() ? 1f : 0.0001f).ToDecibels());
    }

    private void Start() {
        MusicToggleButton.onClick.RemoveAllListeners();
        SoundToggleButton.onClick.RemoveAllListeners();
        UpdateAudioButtons();
    }

    private void UpdateAudioButtons() {
        SoundOn.Value = PlayerPrefs.GetInt("SOUND_VOLUME", 1).ToBoolean();
        MusicOn.Value = PlayerPrefs.GetInt("MUSIC_VOLUME", 1).ToBoolean();
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

