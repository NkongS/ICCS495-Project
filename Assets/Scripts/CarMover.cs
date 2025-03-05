using UnityEngine;
using System.Collections;

public class CarMover : MonoBehaviour
{
    public float speed = 10f;
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
            other.GetComponent<PlayerController>().Die();
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
