using UnityEngine;

public class Bullet : MonoBehaviour, IPooleable
{
    float _speed;
    float _maxPosY;

    void Start()
    {
        _speed = 8.0f;
        _maxPosY = 5.5f;
    }

    void Update()
    {
        Movement();
    }

    void Movement()
    {
        transform.Translate(Vector2.up * _speed * Time.deltaTime);

        if (transform.position.y >= _maxPosY)
        {
            ReturnToAvailable();
        }
    }

    public void OnActivate()
    {
        gameObject.SetActive(true);
    }

    public void OnDeactivate()
    {
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

            ReturnToAvailable();
        }
    }

    void ReturnToAvailable()
    {
        if (ObjectPoolManager.Instance != null)
        {
            ObjectPoolManager.Instance.PoolPlayerBullets.ReturnToAvailable(this);
        }
    }
}
