using UnityEngine;
using Sirenix.OdinInspector;

public class DoggoSpawner : SerializedMonoBehaviour
{
    public Transform SpawnPoint;
    public (float Min, float Max) XSpawnerBoundaries;

    private void Update() {
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, XSpawnerBoundaries.Min,XSpawnerBoundaries.Max), 
            transform.position.y, 
            transform.position.z
        );
    }
}
