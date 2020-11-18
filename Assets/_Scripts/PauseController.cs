using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    private GameObject _pausePanel;
    private string _garageSceneName = "Level1";
    private string _mainMenuSceneName = "Level1";
    GameController controller;

    // Start is called before the first frame update
    void Awake()
    {
        controller = GameObject.Find("GameControllerObject").GetComponent<GameController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        _pausePanel = GameObject.Find("Pause Menu");
        _pausePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale > 0)
            {
                Time.timeScale = 0;
                _pausePanel.SetActive(true);
            }
            else
            {
                Time.timeScale = 1;
                _pausePanel.SetActive(false);
            }
        }
    }

    // Reloads the current scene
    public void RestartLevel()
    {
        controller.FinishLevel();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    public void LoadGarage()
    {
        SceneManager.LoadScene(_garageSceneName, LoadSceneMode.Single);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(_mainMenuSceneName, LoadSceneMode.Single);
    }
}
