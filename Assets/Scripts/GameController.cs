using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public GhostMovement[] ghosts;
    public TextMeshProUGUI scoreText;
    
    public int score = 0;
    float edibletimer = 0;
    public int dotsCollected = 0;

    public GameObject WinText;
    public GameObject GameOverText;

    public GameObject player;
    PacmanMovement Movement; 

    // Start is called before the first frame update
    void Start()
    {
        Movement = player.GetComponent<PacmanMovement>();

        WinText.SetActive(false);
        GameOverText.SetActive(false);
        edibletimer = 0;
        SetScoreText();
    }

    private void FixedUpdate()
    {
        if (edibletimer > 0)
            edibletimer -= Time.deltaTime;

        else
        {
            foreach (GhostMovement ghost in ghosts)
                ghost.SetNotEdible();
        }
    }
        // Update is called once per frame
    void Update()
    {
        if (dotsCollected >= 190)
            Win();
    }

    void Win()
    {
        Movement.canMove = false;
        WinText.SetActive(true);
    }
    public void SetScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }
    public void GameOver()
    {
        Movement.canMove = false;
        gameObject.SetActive(false);
        GameOverText.SetActive(true);
    }

}
