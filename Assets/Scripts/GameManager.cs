using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class GameManager : MonoBehaviour
{
    public List<GameObject> enemyList;
    public List<SpawnObject> spawnList;
    public int spawnIndex = 0;

    public GameObject loseCanvas;

    public float beatIncrement;
    public bool loseGame = false;
    public bool test = false;

    public float score;
    public int combo;
    public float multiplier = 1;

    //public Text loseText;
    public Text scoreText;
    public Text multiText;

    //public List<GameObject>

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        combo = 0;

        //Check for test mode (no enemies)
        if (test)
        {
            loseGame = true;
            return;
        }
        
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
        if (loseGame)
            return;

        CheckForNewSpawn();
        
    }

    private void CheckForNewSpawn()
    {
        if (spawnIndex >= spawnList.Count)
            return;
        //Check if the next enemy in the spawn list has a beat greater than or equal to the current song beat. If so, spawn it
        if (Conductor.instance.songPosInBeats >= spawnList[spawnIndex].beat)
        {
            Debug.Log("Hit the spawn item for spawn " + spawnIndex.ToString());
            SpawnEnemy(spawnList[spawnIndex].enemyIndex, 8, spawnList[spawnIndex].ySpawn, spawnList[spawnIndex].beat);
            spawnIndex++;
        }
    }

    public void SpawnEnemy(int enemyIndex, float x, float y, float beat)
    {
        //Debug.Log("Spawning enemy at: " + x.ToString() + y.ToString());
        GameObject newEnemy = Instantiate(enemyList[enemyIndex], new Vector3(x,y,0), Quaternion.identity);
        newEnemy.GetComponent<EnemyAI>().Initialize(beat);
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

    [System.Serializable]
    public class SpawnObject
    {
        [SerializeField]
        public int enemyIndex;
        [SerializeField]
        public float beat;
        [SerializeField]
        public float ySpawn;
    }
}


