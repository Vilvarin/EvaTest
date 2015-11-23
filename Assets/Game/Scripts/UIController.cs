using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour 
{
    public Button exitButton;
    public Text scoreText;
    public Board _board;

    void Start()
    {
        exitButton.onClick.AddListener(click_exitButton);
        _board.addPoints += _board_addPoints;
    }

    void _board_addPoints(object sender, ScoreEventArgs e)
    {
        scoreText.text = "Score: " + e.Value;
    }

    void click_exitButton()
    {
        Application.Quit();
    }
}
