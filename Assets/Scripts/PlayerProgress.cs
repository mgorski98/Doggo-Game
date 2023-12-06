using TMPro;
using UnityEngine;

public class PlayerProgress : MonoBehaviour {
    public TextMeshProUGUI Score;
    public TextMeshProUGUI GameTime;

    public static PlayerProgress Instance { get; private set; } = null;

    private void Awake() {
        Instance = this;
    }

    private void OnDestroy() {
        if (Instance == this) {
            Instance = null;
        }
    }
}
