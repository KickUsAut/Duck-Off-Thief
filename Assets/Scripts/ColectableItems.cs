using UnityEngine;

public class ColectableItem : MonoBehaviour
{
    public float rotationSpeed = 50f;

    private void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Optional: hier Sound oder Partikel abspielen
            Destroy(gameObject);
        }
    }
}
