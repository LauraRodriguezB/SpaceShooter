using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Sprite IdleSprite;
    public Sprite LeftSprite;
    public Sprite RightSprite;
    public GameObject LaserPrefab;
    private GameObject _activeLaser;

    float _speed;
    SpriteRenderer _spriteRenderer;

    void Start()
    {
        _speed = 5.0f;
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (LaserPrefab != null)
        {
            _activeLaser = Instantiate(LaserPrefab);
            _activeLaser.SetActive(false);
        }
    }

    void Update()
    {
        Movement();
        Shoot();
    }

    void Movement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector2 movement = new Vector2(moveX, moveY) * _speed * Time.deltaTime;

        transform.Translate(movement);

        Vector2 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0));
        Vector2 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1));

        float paddingX = _spriteRenderer.size.x / 2.0f;
        float paddingY = _spriteRenderer.size.y / 2.0f;
        float minX = bottomLeft.x + paddingX;
        float maxX = topRight.x - paddingX;
        float minY = bottomLeft.y + paddingY;
        float maxY = topRight.y - paddingY;

        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);

        transform.position = new Vector2(clampedX, clampedY);

        if (moveX < 0)
        {
            _spriteRenderer.sprite = LeftSprite;
        }
        else if (moveX > 0)
        {
            _spriteRenderer.sprite = RightSprite;
        }
        else
        {
            _spriteRenderer.sprite = IdleSprite;
        }
    }

    void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (GameManager.Instance != null && GameManager.Instance.LaserAmmo > 0)
            {
                if (_activeLaser != null && _activeLaser.activeSelf)
                {
                    return;
                }

                if (_activeLaser != null)
                {
                    float offset = _spriteRenderer.bounds.size.y / 2.0f;
                    Vector2 spawnPos = new Vector2(transform.position.x, transform.position.y + offset);

                    LaserBullet laserBullet = _activeLaser.GetComponent<LaserBullet>();
                    if (laserBullet != null)
                    {
                        laserBullet.Shoot(spawnPos);
                        GameManager.Instance.AddLaserAmmo(-1);
                    }
                }
            }
            else
            {
                SpawnBullet();
            }
        }
    }

    public void SpawnBullet()
    {
        if (ObjectPoolManager.Instance != null)
        {
            ObjectPoolManager.Instance.SpawnPlayerBullet(transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddLifes(-1);
                StartCoroutine(DamageEffect());
            }
        }
    }

    IEnumerator DamageEffect()
    {
        _spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(0.2f);

        _spriteRenderer.color = Color.white;
    }
}
