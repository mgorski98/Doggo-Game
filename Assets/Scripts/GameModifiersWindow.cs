using UnityEngine;
using UnityEngine.UI;

public class GameModifiersWindow : MonoBehaviour
{
    private GameModifiers Modifiers;

    public Toggle RetroMode;
    public Toggle BouncyDogs;
    public Toggle InvisiblePreview;
    public Toggle TimedMode;

    private void Start()
    {
        Modifiers = GameModifiers.Instance;
        Init();
    }

    private void Init() {
        RetroMode.isOn = Modifiers.RetroMode;
        InvisiblePreview.isOn = Modifiers.InvisibleDoggoPreview;
        BouncyDogs.isOn = Modifiers.BouncyDoggos;
        TimedMode.isOn = Modifiers.TimedMode;

        RetroMode.onValueChanged.AddListener(newValue => { 
            Modifiers.RetroMode = newValue;
            Modifiers.SaveModifiers();
        });
        BouncyDogs.onValueChanged.AddListener(newValue => { 
            Modifiers.BouncyDoggos = newValue;
            Modifiers.SaveModifiers();
        });
        InvisiblePreview.onValueChanged.AddListener(newValue => {
            Modifiers.InvisibleDoggoPreview = newValue;
            Modifiers.SaveModifiers();
        });
        TimedMode.onValueChanged.AddListener(newValue => {
            Modifiers.TimedMode = newValue;
            Modifiers.SaveModifiers();
        });
    }
}
