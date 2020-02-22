using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject enemy;
    public GameObject loseCanvas;

    public float startingBeat;
    public float beatIncrement;
    public bool loseGame = false;
    public bool test = false;

    public float score;
    public int combo;
    public float multiplier = 1;

    //public Text loseText;
    public Text scoreText;
    public Text multiText;

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        combo = 0;
        if (test)
        {
            loseGame = true;
            return;
        }
        //for (int i=0; i<8; i++)
        //{
        //    SpawnEnemy(10, 5-i);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
        if (loseGame)
            return;
        if (startingBeat < Conductor.instance.songPosInBeats)
        {
            SpawnEnemy(10,Random.Range(-4.0f,4.0f));
            startingBeat += beatIncrement;
        }
    }

    public void SpawnEnemy(float x, float y)
    {
        //Debug.Log("Spawning enemy at: " + x.ToString() + y.ToString());
        GameObject newEnemy = Instantiate(enemy, new Vector3(x,y,0), Quaternion.identity);
    }

    public void AddScore(float addScore)
    {
        combo++;
        CheckMultiplier();
        score += addScore * multiplier;
        scoreText.text = score.ToString();

    }

    private void CheckMultiplier()
    {
        switch(multiplier)
        {
            case 1:
                if (combo > 10)
                {
                    multiplier++;
                    multiText.text = "2x";
                }
                break;
            case 2:
                if (combo > 20)
                {
                    multiplier++;
                    multiText.text = "3x";
                }
                break;
        }
    }

    public void ClearMultiplier()
    {
        multiplier = 1;
        combo = 1;
        multiText.text = "1x";
    }

    public void LoseGame()
    {
        loseGame = true;
        Debug.Log("You lose!");
        loseCanvas.SetActive(true);
    }
}
