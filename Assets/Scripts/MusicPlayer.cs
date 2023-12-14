using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using Sirenix.OdinInspector;

public class MusicPlayer : SerializedMonoBehaviour
{
    public AudioClip[] MusicClips;
    public AudioSource ASource;
    public AudioMixer Mixer;
    public string MusicVolumeProperty;

    private int CurrentIndex = -1;
    private Coroutine FadeOutCoro;

    [Header("CONFIG")]
    public float FadeInOutDurationSeconds = 2f;
    [Range(0.9f, 1f)]
    public float ProgressFadeOutThreshold = 0.9f;
    public float MusicVolume;
    public bool PlayRandom = false;

    public const float MIN_VOLUME = 0.0001f;

    private void Awake()
    {
        if (MusicClips.Length <= 0) {
            enabled = false;
            return;
        }
        Mixer.GetFloat(MusicVolumeProperty, out MusicVolume);
        MusicVolume = MusicVolume.FromDecibels();
    }

    private void Update() {
        if (MusicClips.IsEmpty())
            return;

        if (ASource.isPlaying) {
            var progress = ASource.time / ASource.clip.length;
            if (progress >= ProgressFadeOutThreshold && FadeOutCoro == null) {
                float timeLeft = ASource.clip.length - ASource.time;
                FadeOutCoro = StartCoroutine(FadeOutCurrentSound(Mathf.Min(timeLeft, FadeInOutDurationSeconds)));
            }
        }

        if (!ASource.isPlaying) {
            NextSong(true);
        }
    }

    public void NextSong(bool fadeIn) {
        if (MusicClips.IsEmpty())
            return;

        if (MusicClips.Length == 1) {
            CurrentIndex = 0;
        } else {
            var oldIndex = CurrentIndex;
            while (oldIndex == CurrentIndex) {
                CurrentIndex = PlayRandom ? Random.Range(0, MusicClips.Length) : Utils.ClampWrapping(CurrentIndex + 1, 0, MusicClips.Length - 1);
            }
        }
        ASource.clip = MusicClips[CurrentIndex];
        ASource.Play();
        if (fadeIn)
            StartCoroutine(FadeInCurrentSound(FadeInOutDurationSeconds));
    }

    private IEnumerator FadeOutCurrentSound(float timeLeft) {
        for (float t = timeLeft; t > 0; t -= Time.deltaTime) {
            var progress = 1f - (t / timeLeft);
            var interpValue = Mathf.Lerp(MusicVolume, MIN_VOLUME, progress);
            var newVol = interpValue.ToDecibels();
            Mixer.SetFloat(MusicVolumeProperty, newVol);
            yield return null;
        }
        FadeOutCoro = null;
    }

    private IEnumerator FadeInCurrentSound(float duration) {
        for (float t = 0; t < duration; t += Time.deltaTime) {
            var interpValue = Mathf.Lerp(MIN_VOLUME, MusicVolume, t / duration);
            var newVol = interpValue.ToDecibels();
            Mixer.SetFloat(MusicVolumeProperty, newVol);
            yield return null;
        }
    }
}
