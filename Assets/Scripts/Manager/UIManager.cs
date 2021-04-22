using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject pauseMenu;
    public GameObject gameOverPanel;
    public GameObject healthBar;
    public Slider bossHealthBar;

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void UpdateHealth(float currenthealth)
    {

        for (int i = 0; i < currenthealth; i++)
            healthBar.transform.GetChild(i).gameObject.SetActive(true);
        for (int j = 0; j < 3 - currenthealth; j++)
            healthBar.transform.GetChild(2 - j).gameObject.SetActive(false);
    }


    public void PasueGame()
    {
      
        pauseMenu.SetActive(true);
        Time.timeScale = 0;

    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void GameOverUI(bool isDead)
    {
        gameOverPanel.SetActive(isDead);
    }

    public void SetBossHealth(float health)
    {
        bossHealthBar.maxValue = health;
    }

    public void UpdateBossHealth(float health)
    {
        bossHealthBar.value = health;
    }
}
