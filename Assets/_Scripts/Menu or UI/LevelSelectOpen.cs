using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectOpen : MonoBehaviour
{
    GameController controller;
    void Awake()
    {
        controller = GameObject.Find("GameControllerObject").GetComponent<GameController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        int index = 0;
        foreach(ButtonScripts child in gameObject.transform.GetComponentsInChildren<ButtonScripts>()){
            if (child.gameObject.name == "Close") continue;
            if (index < controller.levels.Length){
                child.transform.GetComponentInChildren<Text>().text = controller.levels[index];
                child.number = index;
                index++;
            } else {
                child.gameObject.SetActive(false);
            }
        }        
    }

}
