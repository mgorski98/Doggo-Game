using UnityEngine;

public class DoggoBehaviour : MonoBehaviour
{
    public bool HasCollidedAlready = false;
    public DoggoData DoggoData;
    private SpriteRenderer DoggoSpriteRenderer;
    [SerializeField]
    [Range(0f, 1f)]
    private float GraceTime = 0.75f;
    private bool EligibleForGameLoss = false;

    private void Awake() {
        DoggoSpriteRenderer = GetComponent<SpriteRenderer>();
        DoggoSpriteRenderer.sprite = DoggoData.DoggoIcon;

        Invoke(nameof(ReenableGameLoss), GraceTime);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Doggo") && !HasCollidedAlready) {
            var behaviour = collision.gameObject.GetComponent<DoggoBehaviour>();
            if (behaviour.HasCollidedAlready)
                return;
            var otherDoggoData = behaviour.DoggoData;
            if (otherDoggoData.ID != DoggoData.ID)
                return;
            behaviour.HasCollidedAlready = true;
            HasCollidedAlready = true;
            MergeManager.Instance.QueueMerge(gameObject, collision.gameObject, otherDoggoData);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("GameOverLine") && EligibleForGameLoss && !PlayerProgress.Instance.GameOver) {
            PlayerProgress.Instance.HandleGameOver();
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("GameOverLine") && EligibleForGameLoss && !PlayerProgress.Instance.GameOver) {
            PlayerProgress.Instance.HandleGameOver();
        }
    }

    private void ReenableGameLoss() {
        EligibleForGameLoss = true;
    }
}
