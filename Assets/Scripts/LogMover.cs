using UnityEngine;
using System.Collections;

public class LogMover : MonoBehaviour
{
    public float speed = 10f;
    private float destructionThreshold;

    void Start()
    {
        destructionThreshold = transform.position.x + 250f;
    }

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        if (transform.position.x >= destructionThreshold)
        {
        if (Random.value < 0.5f)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.position = new Vector3(transform.position.x - 250f, transform.position.y, transform.position.z); // Move back to spawn point
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
