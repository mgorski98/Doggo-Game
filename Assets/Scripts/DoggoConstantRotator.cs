using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoggoConstantRotator : MonoBehaviour
{
    [SerializeField]
    private float RotationSpeed = 1f;
    [SerializeField]
    private Vector3 RotationAxes;

    private void Update() {
        transform.Rotate(RotationAxes * (RotationSpeed * Time.deltaTime));
    }

    public void SetRotationSpeed(float newSpeed) {
        RotationSpeed = newSpeed;
    }

    public void SetAxes(Vector3 axes) {
        RotationAxes = axes;
    }
}
