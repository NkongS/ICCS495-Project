using UnityEngine;
using System.Collections;

public class LogMover : MonoBehaviour
{
    public float speed = 5f;
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
            Destroy(gameObject);
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
