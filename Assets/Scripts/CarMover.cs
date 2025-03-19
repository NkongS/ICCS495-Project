using UnityEngine;
using System.Collections;

public class CarMover : MonoBehaviour
{
    public float speed = 10f;
    private float destructionThreshold;

    void Start()
    {
        destructionThreshold = transform.position.x + 250f;
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        if (transform.position.x >= destructionThreshold)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().Die();
        }
    }
}
