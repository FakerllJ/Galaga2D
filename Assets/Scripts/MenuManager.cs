using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public TextMeshProUGUI highScoreText;

    void Start()
    {
        Time.timeScale = 1f;
        if (highScoreText != null)
            highScoreText.text = "Recorde: " + PlayerPrefs.GetInt("HighScore", 0);
    }

    public void Play()
    {
        SceneManager.LoadScene("Game");
    }

    public void Quit()
    {
        Application.Quit();   // so funciona no jogo compilado (build)
    }
}
