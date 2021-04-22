using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private PlayerController player;

    public bool gameOver;

    public Door doorExit;

    public List<Enemy> enemies;

    public List<int> SceneNum= new List<int> { 1, 2, 3, 4 };

    public void Awake()
    {

        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        RandomNum(SceneNum);

        player = FindObjectOfType<PlayerController>();

        doorExit = FindObjectOfType<Door>();
    }

    private void Update()
    {

        if(player !=null)
        gameOver = player.isDead;
        UIManager.instance.GameOverUI(gameOver);
    }


    public void IsEnemy(Enemy enemy)
    {
        enemies.Add(enemy);
    }

    public void EnemyDead(Enemy enemy)
    {

        enemies.Remove(enemy);
        if(enemies.Count == 0&&SceneManager.GetActiveScene().buildIndex!=4)
        {
            doorExit.OpenDoor();
            SaveData();
        }
    }


    public void RestartScene()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        PlayerPrefs.DeleteKey("playerHealth");
    }

    public void NewGame()
    {
        PlayerPrefs.DeleteAll();
        RandomNum(SceneNum);
        SceneManager.LoadScene(SceneNum[0]);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneNum[SceneManager.GetActiveScene().buildIndex+1]);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public float LoadHealth()
    {
        if(!PlayerPrefs.HasKey("playerHealth"))
        {
            PlayerPrefs.SetFloat("playerHealth", 3f);
        }
        float currentHealth = PlayerPrefs.GetFloat("playerHealth");

        
        return currentHealth;
    }


    public void IsPlayer(PlayerController controller)
    {
        player = controller;
    }


    public void IsDoor(Door door)
    {
        doorExit = door;
    }
    public void SaveData()
    {
        PlayerPrefs.SetFloat("playerHealth", player.health);
        PlayerPrefs.Save();
    }


    public static void RandomNum(List<int> arr)
    {
        for (int i = 0; i < arr.Count-1; i++)
        {
            int index = new System.Random().Next(i, arr.Count-1);
            int tmp = arr[i];
            int ran = arr[index];
            arr[i] = ran;
            arr[index] = tmp;
        }
    }

}
