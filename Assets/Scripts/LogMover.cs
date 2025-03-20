using UnityEngine;
using System.Collections;

public class LogMover : MonoBehaviour
{
    public float speed = 20f;
    private float destructionThreshold;
    private float dist = 200f;

    void Start()
    {
        destructionThreshold = transform.position.x + dist;
    }

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);

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
            other.transform.parent = transform; // Attach player to log
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent = null; // Detach player when stepping off
        }
    }
}
