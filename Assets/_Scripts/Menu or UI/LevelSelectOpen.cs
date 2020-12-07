using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectOpen : MonoBehaviour
{
    GameController controller;
    int currentLevel = 0;
    public static bool open = false;
    void Awake()
    {
        controller = GameObject.Find("GameControllerObject").GetComponent<GameController>();
        currentLevel = PlayerPrefs.GetInt("CurrentLevel");
    }
    // Start is called before the first frame update
    void Start()
    {
        int index = 0;
        foreach(ButtonScripts child in gameObject.GetComponentsInChildren<ButtonScripts>()){
            GameObject buttonHolder = child.gameObject;
            if (buttonHolder.name == "Close") continue;
            if (index < controller.levels.Length){
                buttonHolder.GetComponentInChildren<Text>().text = controller.levels[index];
                child.number = index;

                if (index > currentLevel)
                    buttonHolder.GetComponent<Button>().interactable = false;
                
                index++;
            } else {
                buttonHolder.SetActive(false);
            }
        }        
    }

    /// <summary>
    /// Logic used to ensure that we allow people to relaunch level select after closing this.
    /// </summary>
    private void OnDestroy()
    {
        open = false;
    }

}
