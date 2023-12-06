using UnityEngine;

public class DoggoBehaviour : MonoBehaviour
{
    public bool HasCollidedAlready = false;
    public DoggoData DoggoData;

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
}
