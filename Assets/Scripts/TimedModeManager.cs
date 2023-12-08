using UnityEngine;
using TMPro;

public class TimedModeManager : MonoBehaviour {
    public float StartTime;
    public float TimePerDoggoMerged;

    public float CurrentTime;

    private int CurrentTimeInt;

    public TextMeshProUGUI TimerText;

    private void Start() {
        if (!GameModifiers.Instance.TimedMode) {
            TimerText.gameObject.SetActive(false);
            enabled = false;
            return;
        }

        TimerText.gameObject.SetActive(true);
        CurrentTime = StartTime;
    }

    private void Update() {
        if (!GameModifiers.Instance?.TimedMode ?? false)
            return;

        CurrentTime -= Time.deltaTime;
        int newValue = (int)Mathf.Ceil(CurrentTime);
        if (newValue != CurrentTimeInt) {
            CurrentTimeInt = newValue;
            TimerText.text = CurrentTimeInt.ToString();
        }

        if (CurrentTime < 10) {
            TimerText.color = Color.red;
        }

        if (CurrentTime <= 0f) {
            PlayerProgress.Instance.HandleGameOver();
            enabled = false;
        }
    }

    public void IncrementTime() {
        CurrentTime += TimePerDoggoMerged;
    }
}
