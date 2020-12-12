using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    private GameObject _pausePanel;

    GameController controller;

    // Start is called before the first frame update
    void Awake()
    {
        controller = GameObject.Find("GameControllerObject").GetComponent<GameController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        _pausePanel = GameObject.Find("HUD").transform.Find("Pause Menu").gameObject;
        _pausePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!_pausePanel.activeSelf)
            {
                _pausePanel.SetActive(true);
                GameObject.Find("HUD").transform.Find("LevelRewards").GetComponent<LevelRewards>().Show();
                Time.timeScale = 0;
                if (CountDown.counting){
                    CountDown.Pause();
                }
            }
            else
            {
                Continue();
            }
        }
    }


    private void Continue(){
        if(!CountDown.counting){
            Time.timeScale = 1;
            _pausePanel.SetActive(false);
            GameObject.Find("HUD").transform.Find("LevelRewards").GetComponent<LevelRewards>().Hide();
        } else {
            _pausePanel.SetActive(false);
            CountDown.UnPause();
        }
    }
    // Reloads the current scene
    public void RestartLevel()
    {
        Time.timeScale = 1;
        controller.RetryLevel();
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1;
        controller.MainMenu();
    }

    public void ResumeGame(){
        Continue();
    }
}
