using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public event Action<int> OnScoreChanged;
    public event Action<int> OnLifesChanged;
    public event Action<int> OnLaserAmmoChanged;
    public PowerUp LaserPowerUp;
    public int Score;
    public int Lifes;
    public int LaserAmmo;
    public float EnemySpawnTime = 1.5f;
    int _highScoreScene = 1;

    int _minEnemiesOnScreen = 3;
    float _spawnTimer;
    List<int> _highScoresList;
    bool _isGameOver;
    float _powerUpSpawnTimer;
    bool _canSpawnPowerUp;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        //PlayerPrefs.DeleteAll();
        _isGameOver = false;
        Score = 0;
        Lifes = 3;
        LaserAmmo = 0;
        _spawnTimer = EnemySpawnTime;
        _canSpawnPowerUp = true;
        ResetPowerUpTimer();
    }

    void Update()
    {
        if (_isGameOver)
        {
            return;
        }

        if (SceneManager.GetActiveScene().buildIndex != _highScoreScene)
        {
            HandleEnemySpawning();
        }

        //Debug.Log("Active Enemies: " + ObjectPoolManager.Instance.PoolEnemies.InUse.Count);

        if (Lifes <= 0)
        {
            _isGameOver = true;
            SaveHighScore();
            SceneManager.LoadScene(_highScoreScene);
        }

        RunPowerUpTimer();
    }

    void HandleEnemySpawning()
    {
        if (ObjectPoolManager.Instance.PoolEnemies == null)
        {
            return;
        }

        if (ObjectPoolManager.Instance.PoolEnemies.InUse.Count < _minEnemiesOnScreen)
        {
            ObjectPoolManager.Instance.SpawnEnemy(5.5f);
        }

        _spawnTimer -= Time.deltaTime;
        if (_spawnTimer <= 0f)
        {
            float randomY = UnityEngine.Random.Range(6.0f, 10.0f);
            ObjectPoolManager.Instance.SpawnEnemy(randomY);
            _spawnTimer = EnemySpawnTime;
        }
    }

    public void AddScore(int amount)
    {
        Score += amount;

        OnScoreChanged?.Invoke(Score);
    }

    public void AddLifes(int amount)
    {
        Lifes += amount;

        OnLifesChanged?.Invoke(Lifes);
    }

    public void AddLaserAmmo(int amount)
    {
        LaserAmmo += amount;

        OnLaserAmmoChanged?.Invoke(LaserAmmo);
    }

    void SaveHighScore()
    {
        _highScoresList = new List<int>();

        for (int i = 0; i < 10; i++)
        {
            _highScoresList.Add(PlayerPrefs.GetInt("HighScore_" + i, 0));
        }

        // Add current score only if it's higher than last position's score
        if (Score > _highScoresList[_highScoresList.Count - 1])
        {
            _highScoresList.Add(Score);
            _highScoresList.Sort((a, b) => b.CompareTo(a));
            _highScoresList.RemoveAt(_highScoresList.Count - 1);

            for (int i = 0; i < _highScoresList.Count; i++)
            {
                PlayerPrefs.SetInt("HighScore_" + i, _highScoresList[i]);
            }
            PlayerPrefs.Save();
        }
    }

    public List<int> GetHighScores()
    {
        List<int> highScores = new List<int>();
        for (int i = 0; i < 10; i++)
        {
            highScores.Add(PlayerPrefs.GetInt("HighScore_" + i, 0));
        }
        highScores.Sort((a, b) => b.CompareTo(a));
        return highScores;
    }

    void RunPowerUpTimer()
    {
        _powerUpSpawnTimer -= Time.deltaTime;

        if (_powerUpSpawnTimer <= 0f && _canSpawnPowerUp)
        {
            LaserPowerUp.SpawnPowerUp();
            _canSpawnPowerUp = false;
        }
    }

    public void ResetPowerUpTimer()
    {
        _powerUpSpawnTimer = UnityEngine.Random.Range(5.0f, 15.0f);
        _canSpawnPowerUp = true;
    }
}
