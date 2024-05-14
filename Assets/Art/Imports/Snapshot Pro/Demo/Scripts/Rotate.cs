using UnityEngine;

public class Rotate : MonoBehaviour {
    [SerializeField] private Vector3 eulerRotation;

    private void Update() {
        transform.Rotate(eulerRotation * Time.deltaTime);
    }
}