using UnityEngine;

public class PowerUp : MonoBehaviour
{
    float _speed;
    float _posX;
    float _posY = 5.5f;
    float _xLimit;

    void Start()
    {
        _speed = 3.0f;
        _xLimit = 7.0f;
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (!gameObject.activeSelf)
        {
            return;
        }

        Movement();
    }

    void Movement()
    {
        float posY = transform.position.y - (_speed * Time.deltaTime);
        transform.position = new Vector2(_posX, posY);

        Debug.Log("POWER UP POS: " + transform.position);

        if (transform.position.y <= -5.5f)
        {
            DeactivatePowerUp();
        }
    }

    public void SpawnPowerUp()
    {
        _posX = Random.Range(-_xLimit, _xLimit);
        transform.position = new Vector2(_posX, _posY);
        gameObject.SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddLaserAmmo(5);
                DeactivatePowerUp();
            }
        }
    }

    void DeactivatePowerUp()
    {
        gameObject.SetActive(false);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetPowerUpTimer();
        }
    }
}
