using UnityEngine;

public class Enemy : MonoBehaviour, IPooleable
{
    float _speed;
    float _spawnX;
    float _amplitude;
    float _maxPosY;

    void Start()
    {
        _speed = 2.0f;
        _amplitude = 1.2f;
        _maxPosY = -5.5f;
    }

    void Update()
    {
        Movement();
    }

    void Movement()
    {
        float posY = transform.position.y - (_speed * Time.deltaTime);

        float posX = _spawnX + Mathf.Sin(posY * 2.0f) * _amplitude;

        transform.position = new Vector2(posX, posY);

        if (transform.position.y <= _maxPosY)
        {
            ReturnToAvailable();
        }
    }

    public void Init(float spawnX)
    {
        _spawnX = spawnX;
    }

    public void OnActivate()
    {
        gameObject.SetActive(true);
    }

    public void OnDeactivate()
    {
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Bullet"))
        {
            ReturnToAvailable();
        }
    }

    void ReturnToAvailable()
    {
        if (ObjectPoolManager.Instance != null)
        {
            ObjectPoolManager.Instance.PoolEnemies.ReturnToAvailable(this);
        }
    }
}
