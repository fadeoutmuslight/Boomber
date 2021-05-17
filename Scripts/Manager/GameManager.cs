using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private PlayerController player;
    private Door doorExit;
    public bool gameOver;
    public List<Enemy> enemiesAlive;
    public bool bombRollMode=false;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        //player = FindObjectOfType<PlayerController>();
        //doorExit = FindObjectOfType<Door>();
    }
    public void Update()
    {
        if (player != null)
        {
            gameOver = player.isDead;
        }
        UIManager.instance.gameOverUI(gameOver); //By calling the single instance of UI Manager, we can control the game over panel on or off by the gameover boolean
    }
    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        PlayerPrefs.DeleteKey("playerHealth");
        Time.timeScale = 1;
    }
    public void enemyRegister(Enemy enemy)
    {
        enemiesAlive.Add(enemy);
        
    }
    public void playerRegister(PlayerController controller)
    {
        player = controller;
    }
    public void doorRegister(Door door)
    {
        doorExit = door;
    }
    public void enemyDead(Enemy enemy)
    {
        enemiesAlive.Remove(enemy);
        if (enemiesAlive.Count == 0)
        {
            doorExit.DoorOpen();
            SaveData();
        }
    }
    public void newGame()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }
    public void continueGame()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("levelIndex"));
        Time.timeScale = 1;
    }
    public void nextLevel()
    {
        if (PlayerPrefs.HasKey("levelIndex"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            newGame();
        }
        
    }
    public void quitGame()
    {
        Application.Quit();
    }
    public void backToMenu()
    {
        SceneManager.LoadScene(0);
    }
    public float LoadHealth()
    {
        if (!PlayerPrefs.HasKey("playerHealth"))
        {
            PlayerPrefs.SetFloat("playerHealth", 3f);
        }
        float currentHealth = PlayerPrefs.GetFloat("playerHealth");
        
        return currentHealth;
    }
    public void SaveData()
    {
        PlayerPrefs.SetFloat("playerHealth", player.Health);
        PlayerPrefs.SetInt("levelIndex", SceneManager.GetActiveScene().buildIndex + 1);
        PlayerPrefs.Save();
    }
    public void RollOnClick()
    {
        bombRollMode = true;
        UIManager.instance.rollOnBtn.SetActive(false);
        UIManager.instance.rollOffBtn.SetActive(true);
    }
    public void RollOffClick()
    {
        bombRollMode = false;
        UIManager.instance.rollOffBtn.SetActive(false);
        UIManager.instance.rollOnBtn.SetActive(true);
    }
}
