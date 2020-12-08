using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButtonSpecial : MonoBehaviour
{
    void Start()
    {
        if (PlayerPrefs.GetInt("CurrentLevel") == 0){
            this.GetComponent<Text>().text = "Start Game";
        }
    }

}
