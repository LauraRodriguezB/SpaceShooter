using TMPro;
using UnityEditor.Rendering.Analytics;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI LifesText;
    public TextMeshProUGUI LaserAmmoText;

    void Update()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnScoreChanged += UpdateScoreUI;
            GameManager.Instance.OnLifesChanged += UpdateLifesUI;
            GameManager.Instance.OnLaserAmmoChanged += UpdateLaserAmmoUI;

            UpdateScoreUI(GameManager.Instance.Score);
            UpdateLifesUI(GameManager.Instance.Lifes);
            UpdateLaserAmmoUI(GameManager.Instance.LaserAmmo);
        }
    }

    void UpdateScoreUI(int score)
    {
        if (ScoreText != null)
        {
            ScoreText.text = "Score: " + score.ToString();
        }
    }

    void UpdateLifesUI(int lifes)
    {
        if (LifesText != null)
        {
            LifesText.text = lifes.ToString();
        }
    }

    void UpdateLaserAmmoUI(int ammo)
    {
        if (LaserAmmoText != null)
        {
            LaserAmmoText.text = ammo.ToString();
        }
    }
}
