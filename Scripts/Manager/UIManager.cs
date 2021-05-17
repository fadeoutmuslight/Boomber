using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject gameOverPanel;
    [Header("UI Element")]
    public GameObject healthBar;
    public GameObject pauseMenu;
    public GameObject bossHealthBarWithBackground;
    public Slider bossHealthBar;
    public GameObject rollOnBtn;
    public GameObject rollOffBtn;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }
    public void UpdateHealth(float currentHealth) {
        switch (currentHealth) {
            case 3:
                healthBar.transform.GetChild(0).gameObject.SetActive(true);
                healthBar.transform.GetChild(1).gameObject.SetActive(true);
                healthBar.transform.GetChild(2).gameObject.SetActive(true);
                break;
            case 2:
                healthBar.transform.GetChild(0).gameObject.SetActive(true);
                healthBar.transform.GetChild(1).gameObject.SetActive(true);
                healthBar.transform.GetChild(2).gameObject.SetActive(false);
                break;
            case 1:
                healthBar.transform.GetChild(0).gameObject.SetActive(true);
                healthBar.transform.GetChild(1).gameObject.SetActive(false);
                healthBar.transform.GetChild(2).gameObject.SetActive(false);
                break;
            case 0:
                healthBar.transform.GetChild(0).gameObject.SetActive(false);
                healthBar.transform.GetChild(1).gameObject.SetActive(false);
                healthBar.transform.GetChild(2).gameObject.SetActive(false);
                break;
        }
    }
    
    public void pauseGame() {
        pauseMenu.SetActive(true);
        Time.timeScale = 0; //Time.timeScale is used to control the speed of game running, when it is 0, the game will be paused until it become a value greater than 0.
    }
    public void continueGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1; //Time.timeScale is used to control the speed of game running, when it is 0, the game will be paused until it become a value greater than 0.
    }
    public void SetBossHealth(float health) {
        bossHealthBar.maxValue = health;
    }
    public void UpdateBossHealth(float health) {
        bossHealthBar.value = health;
        if (health <= 0)
        {
            bossHealthBarWithBackground = null;
        }
    }
    public void gameOverUI(bool playerDead)
    {
        gameOverPanel.SetActive(playerDead);
    }
}
