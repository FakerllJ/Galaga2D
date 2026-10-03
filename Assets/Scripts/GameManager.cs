using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Estado")]
    public int score = 0;
    public int lives = 3;
    public int level = 1;

    [Header("HUD")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI highScoreText;

    [Header("Telas")]
    public GameObject gameOverPanel;
    public GameObject victoryPanel;
    public GameObject pausePanel;

    [Header("Audio")]
    public AudioSource musicSource;
    public AudioClip levelMusic;
    public AudioClip bossMusic;
    public AudioClip victoryMusic;
    public AudioClip gameOverSound;

    int highScore;
    bool gameEnded;
    bool paused;

    void Awake()
    {
        Instance = this;
        Time.timeScale = 1f;
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    void Start()
    {
        Show(gameOverPanel, false);
        Show(victoryPanel, false);
        Show(pausePanel, false);
        PlayMusic(level >= 4 ? bossMusic : levelMusic);
        UpdateHUD();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !gameEnded) TogglePause();
    }

    public void AddScore(int points)
    {
        score += points;
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
        }
        UpdateHUD();
    }

    public void AddLife()
    {
        lives++;
        UpdateHUD();
    }

    public void LoseLife()
    {
        lives--;
        UpdateHUD();
        if (lives <= 0) GameOver();
    }

    public void NextLevel()
    {
        level++;
        if (level >= 4) PlayMusic(bossMusic);
        UpdateHUD();
    }

    void GameOver()
    {
        gameEnded = true;
        if (musicSource != null) musicSource.Stop();
        Sfx.Play(gameOverSound);
        Show(gameOverPanel, true);
        Time.timeScale = 0f;
    }

    public void Victory()
    {
        gameEnded = true;
        PlayMusic(victoryMusic);
        Show(victoryPanel, true);
        Time.timeScale = 0f;
    }

    public void TogglePause()
    {
        paused = !paused;
        Time.timeScale = paused ? 0f : 1f;
        Show(pausePanel, paused);
    }

    // Funcoes chamadas pelos botoes (On Click)
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    void PlayMusic(AudioClip clip)
    {
        if (musicSource == null || clip == null) return;
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    void UpdateHUD()
    {
        if (scoreText != null) scoreText.text = "Pontos: " + score;
        if (livesText != null) livesText.text = "Vidas: " + lives;
        if (levelText != null) levelText.text = level >= 4 ? "Fase: BOSS" : "Fase: " + level;
        if (highScoreText != null) highScoreText.text = "Recorde: " + highScore;
    }

    void Show(GameObject obj, bool value)
    {
        if (obj != null) obj.SetActive(value);
    }
}
