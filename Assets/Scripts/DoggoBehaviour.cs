using UnityEngine;

public class DoggoBehaviour : MonoBehaviour
{
    public bool HasCollidedAlready = false;
    public DoggoData DoggoData;
    private SpriteRenderer DoggoSpriteRenderer;

    private void Awake() {
        DoggoSpriteRenderer = GetComponent<SpriteRenderer>();
        DoggoSpriteRenderer.sprite = DoggoData.DoggoIcon;
        var trigger = gameObject.AddComponent<PolygonCollider2D>();
        trigger.isTrigger = true;
        trigger.points = GetComponent<PolygonCollider2D>().points;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Doggo") && !HasCollidedAlready) {
            var behaviour = collision.gameObject.GetComponent<DoggoBehaviour>();
            var otherDoggoData = behaviour.DoggoData;
            if (otherDoggoData.ID != DoggoData.ID)
                return;
            behaviour.HasCollidedAlready = true;
            HasCollidedAlready = true;
            MergeManager.Instance.QueueMerge(gameObject, collision.gameObject, otherDoggoData);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("GameOverLine")) {
            //check direction first
            var direction = (transform.position - collision.gameObject.transform.position).normalized;
            if (direction.y < 0f) {
                PlayerProgress.Instance.HandleGameOver();
            }
        }
    }
}
