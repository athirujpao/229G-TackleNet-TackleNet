using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUIManager : MonoBehaviour
{
    public TMP_Text coinText;
    public TMP_Text timerText;
    public GameObject gameOverPanel;

    private int totalCoins;
    private int coinsCollected = 0;
    private float timer = 0f;
    private bool gameEnded = false;

    void Start()
    {
        totalCoins = GameObject.FindGameObjectsWithTag("Coin").Length;
        UpdateUI();
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    void Update()
    {
        if (gameEnded) return;

        timer += Time.deltaTime;
        timerText.text = "Time: " + timer.ToString("F2");
    }

    public void AddCoin()
    {
        coinsCollected++;
        UpdateUI();

        if (coinsCollected >= totalCoins)
        {
            EndGame();
        }
    }

    void UpdateUI()
    {
        coinText.text = "Coins: " + coinsCollected + " / " + totalCoins;
    }

    void EndGame()
    {
        gameEnded = true;
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
        Time.timeScale = 0; // หยุดเกม
    }
}
