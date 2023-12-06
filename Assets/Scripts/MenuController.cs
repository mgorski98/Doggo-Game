using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject Buttons;
    public GameObject Locales;
    public GameObject Leaderboards;

    public void ExitGame() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void ShowLeaderboards() {

    }

    public void ShowLocaleSelect() {

    }
}
