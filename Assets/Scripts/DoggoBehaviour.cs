using UnityEngine;
using UnityEngine.EventSystems;

public class DoggoBehaviour : MonoBehaviour, IPoolable, IPointerClickHandler {
    public bool HasCollidedAlready = false;
    public DoggoData DoggoData;
    private SpriteRenderer DoggoSpriteRenderer;
    [SerializeField]
    private bool EligibleForGameLoss = false;

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
        if (collision.gameObject.CompareTag("Doggo") && !HasCollidedAlready && !PlayerProgress.Instance.GameOver) {
            var behaviour = collision.gameObject.GetComponent<DoggoBehaviour>();
            if (behaviour.HasCollidedAlready)
                return;
            var otherDoggoData = behaviour.DoggoData;
            if (otherDoggoData.ID != DoggoData.ID)
                return;
            behaviour.HasCollidedAlready = true;
            HasCollidedAlready = true;
            GameManager.Instance.QueueMerge(gameObject, collision.gameObject, otherDoggoData);
        }

        if (collision.gameObject.CompareTag("Doggo")) {
            EligibleForGameLoss = true;
        }
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
