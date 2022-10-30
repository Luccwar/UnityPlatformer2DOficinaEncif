using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    public int totalScore;
    public TextMeshProUGUI textScore;
    public GameObject panelGameOver;
    public string nextStage;

    public static GameController instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        panelGameOver = GameObject.Find("PanelGameOver");
        //if(panelGameOver != null)
        panelGameOver.SetActive(false);
        UpdateScoreText();
    }

    public void UpdateScoreText()
    {
        textScore.text = "x" + totalScore.ToString();
    }

    public void ShowGameOver()
    {
        panelGameOver.SetActive(true);
    }

    public void ChangeScene(string LevelName)
    {
        SceneManager.LoadScene(LevelName);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
