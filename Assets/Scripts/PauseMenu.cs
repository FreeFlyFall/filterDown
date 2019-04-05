using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public static bool isPaused = false;
    public GameObject pauseMenuUI;
    [SerializeField] private ScoreSO scoreOB;
    public StateManager state;

    void Start()
    {
       //Cursor.visible = false;
    }

    void Update()
    {
        // If 'i' is pressed, reload scene
        if (Input.GetKeyDown(KeyCode.I))
        {
            state.SaveAndReloadScene();
        }
        // If 'Esc' is pressed, pause or resume game, depending on the state
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            } else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        //Cursor.visible = false;
    }

    public void Pause()
    {
        scoreOB.RecordScore(scoreOB.score, scoreOB.topScore);
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        //Cursor.visible = true;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
