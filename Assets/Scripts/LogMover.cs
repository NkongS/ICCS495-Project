using UnityEngine;
using System.Collections;

public class LogMover : MonoBehaviour
{
    public float speed = 5f;
    public float respawnThreshold = 50f;

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        if (transform.position.x >= respawnThreshold)
        {
            StartCoroutine(RespawnDelay());
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

    IEnumerator RespawnDelay()
    {
        float delay = Random.Range(1f, 3f);
        yield return new WaitForSeconds(delay);
        Respawn();
    }

    void Respawn()
    {
        transform.position = new Vector3(-respawnThreshold, transform.position.y, transform.position.z);
    }
}
