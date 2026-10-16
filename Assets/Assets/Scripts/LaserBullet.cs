using System.Collections;
using UnityEngine;

public class LaserBullet : MonoBehaviour
{
    public float ActiveDuration = 0.15f;

    public void Shoot(Vector2 spawnPosition)
    {
        transform.position = spawnPosition;
        gameObject.SetActive(true);
        StartCoroutine(DisableRoutine());
    }

    IEnumerator DisableRoutine()
    {
        yield return new WaitForSeconds(ActiveDuration);
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(50);
            }
        }
    }
}
