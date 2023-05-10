using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ScoreHandler : MonoBehaviour
{
    #region Singleton
    public static ScoreHandler Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _highScoreText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text _finalScoreText, _finalHighScoreText;

    private int _score;
    private int _highScore;

    private void Start()
    {
        if(PlayerPrefs.HasKey("HighScore"))
        {
            _highScore = PlayerPrefs.GetInt("HighScore");
            _highScoreText.text = _highScore.ToString();
        }
        else
        {
            _highScore = 0;
            _highScoreText.text = _highScore.ToString();
        }
        _score = 0;
        UpdateScores();
    }

    private void UpdateScores()
    {
        _scoreText.text = _score.ToString();
        _highScoreText.text = _highScore.ToString();
    }

    public void AddScore(int value)
    {
        _score += value;
        if(_score > _highScore)
        {
            _highScore = _score;
            PlayerPrefs.SetInt("HighScore", _highScore);
        }
        UpdateScores();
    }

    public void ResetHighScore()
    {
        PlayerPrefs.DeleteKey("HighScore");
        _highScore = 0;
        UpdateScores();
    }

    public void FinishGame()
    {
        _finalScoreText.text = "Score : " + _score.ToString();
        _finalHighScoreText.text = "Highest Score : " + _highScore.ToString();
        LeanTween.moveLocal(gameOverPanel, new Vector2(0, 0), 0.5f).setEaseOutBack();
    }

    public void RestartGame()
    {
        LeanTween.moveLocal(gameOverPanel, new Vector2(0, -2000), 0.5f).setEaseInBack().setOnComplete(() => {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        });
    }

}
