using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HighScoreManager : MonoBehaviour
{
    public TextMeshProUGUI HighScoreText;
    int _gameplayScene = 0;

    void Start()
    {
        DisplayHighScores();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (GameManager.Instance != null)
            {
                Destroy(GameManager.Instance.gameObject);
            }

            SceneManager.LoadScene(_gameplayScene);
        }
    }

    void DisplayHighScores()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("Couldn't find GameManager Instance");
            return;
        }

        List<int> highScoresList;
        highScoresList = GameManager.Instance.GetHighScores();

        string formattedScores = "";

        for (int i = 0; i < highScoresList.Count; i++)
        {
            formattedScores += highScoresList[i].ToString("D6") + "\n";
        }

        HighScoreText.text = formattedScores;
    }
}
