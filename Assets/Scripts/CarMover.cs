using UnityEngine;
using System.Collections;

public class CarMover : MonoBehaviour
{
    public float speed = 10f;
    private float destructionThreshold;

    private float dist = 200f;

    void Start()
    {
        destructionThreshold = transform.position.x + dist;
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        if (transform.position.x >= destructionThreshold)
        {
            if (Random.value < 0.3f)
            {
                Destroy(gameObject);
            }
            else
            {
                transform.position = new Vector3(transform.position.x - dist, transform.position.y, transform.position.z);
            }
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
