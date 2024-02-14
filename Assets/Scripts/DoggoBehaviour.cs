using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class DoggoBehaviour : MonoBehaviour, IPoolable, IPointerClickHandler {
    public bool HasCollidedAlready = false;
    public DoggoData DoggoData;
    private SpriteRenderer DoggoSpriteRenderer;
    [SerializeField]
    private bool EligibleForGameLoss = false;

    public static bool CollisionOccurredInFrame = false;

    [SerializeField]
    [Min(1)]
    private int WaitFrameAfterMergeCount = 4;

    private Rigidbody2D Rbody;

    [SerializeField]
    [Tooltip("Necessary value for clamping the magnitude, since sometimes physics go brrrr and doggos fly into space")]
    private float MaxVelocityMagnitude = 20f;

    private void Awake() {
        DoggoSpriteRenderer = GetComponent<SpriteRenderer>();
        DoggoSpriteRenderer.sprite = DoggoData.DoggoIcon;
        Rbody = GetComponent<Rigidbody2D>();

        if (GameModifiers.Instance.BouncyDoggos) {
            Rbody.sharedMaterial = GameModifiers.Instance.BouncyMat;
        }
    }

    private void Start() {
        EligibleForGameLoss = false; //ffs
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Doggo") && CanDoggoCollide) {
            HandleCollisionWithDoggo(collision.gameObject.GetComponent<DoggoBehaviour>());
        }

        if (collision.gameObject.CompareTag("Doggo")) {
            EligibleForGameLoss = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Doggo") && CanDoggoCollide) {
            HandleCollisionWithDoggo(collision.gameObject.GetComponent<DoggoBehaviour>());
        }
    }

    private bool CanDoggoCollide => !HasCollidedAlready && !PlayerProgress.Instance.GameOver && !CollisionOccurredInFrame;

    private void HandleCollisionWithDoggo(DoggoBehaviour otherDoggo) {
        if (otherDoggo.HasCollidedAlready)
            return;
        var otherDoggoData = otherDoggo.DoggoData;
        if (otherDoggoData.ID != DoggoData.ID)
            return;
        otherDoggo.HasCollidedAlready = true;
        HasCollidedAlready = true;
        CollisionOccurredInFrame = true;
        GameManager.Instance.StartCoroutine(ClearCollisionFlag());
        GameManager.Instance.QueueMerge(gameObject, otherDoggo.gameObject, otherDoggoData);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("GameOverLine") && EligibleForGameLoss && !PlayerProgress.Instance.GameOver) {
            PlayerProgress.Instance.HandleGameOver();
        }
        if (collision.gameObject.CompareTag("Box")) {
            EligibleForGameLoss = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("GameOverLine") && EligibleForGameLoss && !PlayerProgress.Instance.GameOver) {
            PlayerProgress.Instance.HandleGameOver();
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("GameOverLine")) {
            EligibleForGameLoss = true;
        }
    }

    private IEnumerator ClearCollisionFlag() {
        for (int i = 0; i < WaitFrameAfterMergeCount; ++i)
            yield return null;
        CollisionOccurredInFrame = false;
    } 

    private void FixedUpdate() {
        Rbody.velocity = Vector2.ClampMagnitude(Rbody.velocity, MaxVelocityMagnitude);
    }

    private void OnEnable() {
        HasCollidedAlready = false;
        Reset();
    }

    public void Reset() {
        EligibleForGameLoss = false;
        DoggoSpriteRenderer.color = Color.white;
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        if (!GameManager.Instance.DoggoPickUpEnabled)
            return;
        if (GameManager.Instance.PickUpsLeft <= 0)
            return;

        GameManager.Instance.PickUpDoggo(this);
    }
}
