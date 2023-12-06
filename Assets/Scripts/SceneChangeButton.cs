using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SceneChangeButton : MonoBehaviour
{
    public string SceneName;
    public Button Button;

    private void Awake() {
        Button = GetComponent<Button>();
        Button.onClick.AddListener(() => SceneManager.LoadScene(SceneName));
    }
}
