using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class ConfirmationDialog : MonoBehaviour
{
    public Button ConfirmationButton;
    public TextMeshProUGUI DialogContentText;
    [SerializeField]
    private Vector2 StartPosition;
    [SerializeField]
    private float ShowHideTime = 0.5f;

    private void Awake() {
        StartPosition = (transform as RectTransform).anchoredPosition;
    }

    public void Show(string contents, UnityAction confirmationCallback) {
        ConfirmationButton.onClick.RemoveAllListeners();

        DialogContentText.text = contents;
        ConfirmationButton.onClick.AddListener(confirmationCallback);
        gameObject.SetActive(true);
        StartMoveCoro(true);
    }

    public void Hide() {
        StartMoveCoro(false);
    }

    private void StartMoveCoro(bool showing) {
        StartCoroutine(MoveDialog_Coro(showing));
    }

    private IEnumerator MoveDialog_Coro(bool showing) {
        var target = showing ? Vector2.zero : StartPosition;
        (transform as RectTransform).DOAnchorPosY(target.y, ShowHideTime);
        yield return new WaitForSeconds(ShowHideTime);

        gameObject.SetActive(showing);
    }
}
