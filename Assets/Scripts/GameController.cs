using UnityEngine.SceneManagement;
using UnityEngine;


public class GameController : MonoBehaviour
{
    public GameObject ecranMort;
    public GameObject ecranPause;
    public bool isGamePaused;
    public bool isPlayerDead = false;

    public void Start()
    {
        Time.timeScale = 1;
        isGamePaused = false;
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void ReloadGame()
    {
        ecranMort.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void PlayerDies()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        ecranMort.SetActive(true);
        isPlayerDead = true;
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        isGamePaused = false;
        ecranPause.SetActive(false);
        Time.timeScale = 1; //Reprise du temps normale
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            isGamePaused = !isGamePaused;
            ecranPause.SetActive(isGamePaused);
            if (isGamePaused)
            {
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Time.timeScale = 1; //Reprise du temps normale
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    if ((isGamePaused || isPlayerDead) && Input.GetButtonDown("Retry"))
        {
            ReloadGame();
        }
    }
}

