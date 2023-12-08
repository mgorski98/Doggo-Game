using UnityEngine;
using UnityEngine.UI;

public class GameModifiersWindow : MonoBehaviour
{
    private GameModifiers Modifiers;

    public Toggle RetroMode;
    public Toggle BouncyDogs;
    public Toggle InvisiblePreview;

    private void Start()
    {
        Modifiers = GameModifiers.Instance;
        Init();
    }

    private void Init() {
        RetroMode.isOn = Modifiers.RetroMode;
        InvisiblePreview.isOn = Modifiers.InvisibleDoggoPreview;
        BouncyDogs.isOn = Modifiers.BouncyDoggos;

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
    }
}
