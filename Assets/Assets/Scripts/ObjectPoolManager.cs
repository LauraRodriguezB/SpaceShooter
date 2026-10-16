using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance { get; private set; }

    // Player Bullets
    public ObjectPool<Bullet> PoolPlayerBullets;
    public GameObject PlayerBulletPrefab;
    public int PlayerBulletSize = 20;
    //

    // Enemies
    public ObjectPool<Enemy> PoolEnemies;
    public GameObject EnemyPrefab;
    public int EnemyPoolSize = 15;
    //

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (PlayerBulletPrefab != null)
        {
            PoolPlayerBullets = new ObjectPool<Bullet>(PlayerBulletPrefab, PlayerBulletSize, transform);
        }
        if (EnemyPrefab != null)
        {
            PoolEnemies = new ObjectPool<Enemy>(EnemyPrefab, EnemyPoolSize, transform);
        }
    }

    public void SpawnPlayerBullet(Vector2 pos)
    {
        Bullet bullet = PoolPlayerBullets.GetNext();
        bullet.transform.position = pos;
    }

    public void SpawnEnemy(float posY)
    {
        float _boundPosX = 8.0f;
        float randomX = Random.Range(-_boundPosX, _boundPosX);

        Vector2 spawnPosition = new Vector2(randomX, posY);

        Enemy enemy = PoolEnemies.GetNext();

        enemy.Init(randomX);
        enemy.transform.position = spawnPosition;
    }
}
