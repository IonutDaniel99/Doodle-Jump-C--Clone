using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class CanvasManager : MonoBehaviour
{


    public Transform Player;
    public Text scoreText;
    public Text moneyText;

    public GameObject GameOver;
    public GameObject PauseCanvas;
    public Text GOscoreText;


    public Text GOmoneyText;


    public int LastY = 0;
    public int CollectedMoney = 0;
    public bool IsGamePaused = false;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        scoreText.text = "0";
        moneyText.text = "0";
        CollectedMoney = 0;
        Manager.Instance.IsReturningFromGame = true;

    }

    // Update is called once per frame
    void Update()
    {
        UpdateInGameText();
    }

    void UpdateInGameText()
    {
        if (Player == null)
            return;
        if (LastY < Player.position.y)
        {
            LastY = (int)Player.position.y;
            scoreText.text = LastY.ToString();
        }
    }

    public void UpdateMoneyInGameText()
    {
        CollectedMoney++;
        moneyText.text = CollectedMoney.ToString();
    }

    public void OnDeathShowCanvas()
    {
        GameOver.SetActive(true);
        GOscoreText.text = LastY.ToString();
        GOmoneyText.text = SaveManager.Instance.state.money.ToString() + " + " + CollectedMoney.ToString();
    }

    public void PlayAgain()
    {
        SaveManager.Instance.state.money += CollectedMoney;
        if (LastY > SaveManager.Instance.state.HighScore)
        {
            SaveManager.Instance.state.HighScore = LastY;
        }
        SaveManager.Instance.Save();
        SceneManager.LoadScene("Game");
    }


    public void Pause()
    {
        PauseCanvas.SetActive(true);
        Time.timeScale = 0f;
        IsGamePaused = true;
    }

    public void Resume()
    {
        PauseCanvas.SetActive(false);
        Time.timeScale = 1f;
        IsGamePaused = false;
    }
    public void MainMenu()
    {
        SaveManager.Instance.state.money += CollectedMoney;
        if (LastY > SaveManager.Instance.state.HighScore)
        {
            SaveManager.Instance.state.HighScore = LastY;
        }
        SaveManager.Instance.Save();
        SceneManager.LoadScene("Menu");
    }
}
