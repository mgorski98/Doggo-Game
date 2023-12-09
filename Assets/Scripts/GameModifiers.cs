using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System;

public class GameModifiers : SingletonBehaviour<GameModifiers> {
    public bool BouncyDoggos = false;
    public bool RetroMode = false;
    public bool InvisibleDoggoPreview = false;
    public bool TimedMode = false;

    public VolumeProfile PostProcessProfile;
    public PhysicsMaterial2D BouncyMat;

    private FilmGrain GrainEffect;
    private ChromaticAberration ChromAberrationEffect;
    private ColorAdjustments ColorAdjustEffect;
    private LensDistortion LensDistortionEffect;

    protected override void Awake() {
        base.Awake();
        PostProcessProfile.TryGet(out GrainEffect);
        PostProcessProfile.TryGet(out ChromAberrationEffect);
        PostProcessProfile.TryGet(out ColorAdjustEffect);
        PostProcessProfile.TryGet(out LensDistortionEffect);
        FetchModifiers();

        DontDestroyOnLoad(this.gameObject);
    }

    private void Update() {
        GrainEffect.active = RetroMode;
        ChromAberrationEffect.active = RetroMode;
        ColorAdjustEffect.active = RetroMode;
        LensDistortionEffect.active = RetroMode;
    }

    private void FetchModifiers() {
        RetroMode = Convert.ToBoolean(PlayerPrefs.GetInt(nameof(RetroMode), 0));
        InvisibleDoggoPreview = Convert.ToBoolean(PlayerPrefs.GetInt(nameof(InvisibleDoggoPreview), 0));
        BouncyDoggos = Convert.ToBoolean(PlayerPrefs.GetInt(nameof(BouncyDoggos), 0));
        TimedMode = Convert.ToBoolean(PlayerPrefs.GetInt(nameof(TimedMode), 0));
    }

    public void SaveModifiers() {
        PlayerPrefs.SetInt(nameof(RetroMode), Convert.ToInt32(RetroMode));
        PlayerPrefs.SetInt(nameof(InvisibleDoggoPreview), Convert.ToInt32(InvisibleDoggoPreview));
        PlayerPrefs.SetInt(nameof(BouncyDoggos), Convert.ToInt32(BouncyDoggos));
        PlayerPrefs.SetInt(nameof(TimedMode), Convert.ToInt32(TimedMode));
    }
}
