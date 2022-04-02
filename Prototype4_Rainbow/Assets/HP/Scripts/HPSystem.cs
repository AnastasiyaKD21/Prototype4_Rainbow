using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPSystem : MonoBehaviour
{
    public int health;
    public int numberOfLives;
    public GameObject enemy;
    public GameObject player;
    public Transform SpawnPos;

    [SerializeField] private GameObject losePanel;
    [SerializeField] private Score scoreScript;

    private CharacterController controller;
    private PlayerController playerController;
    private EnemyController enemyController;

    public Image[] lives;
    public Sprite fullLive;
    public Sprite emptyLive;

    void Start()
    {
        
    }

    void Awake()
    {
        playerController = player.GetComponent<PlayerController>();
        enemyController = enemy.GetComponent<EnemyController>();
    }

    
    void Update()
    {
        if(health > numberOfLives)
            health = numberOfLives;
        
        for(int i = 0; i < lives.Length; i++)
        {
            if(i < health)
                lives[i].sprite = fullLive;
            else
                lives[i].sprite = emptyLive;

            if(i < numberOfLives)
                lives[i].enabled = true;
            else
                lives[i].enabled = false;
        } 
        
    }

    private void SpawnEnemy()
    {
        Instantiate(enemy, SpawnPos.position, SpawnPos.rotation);
        enemyController.player = playerController.transform;
        
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.tag == "Obstacle")
        {
            health--;
            Destroy(hit.gameObject);
            enemyController.Espeed = playerController.speed;
            enemyController.lineToMove = playerController.lineToMove;
            SpawnEnemy();
            if(health == 0)
            {
                losePanel.SetActive(true);
                Time.timeScale = 0;
                int lastRunScore = int.Parse(scoreScript.scoreText.text.ToString());
                PlayerPrefs.SetInt("lastRunScore", lastRunScore);
            }
        }
    }
}
