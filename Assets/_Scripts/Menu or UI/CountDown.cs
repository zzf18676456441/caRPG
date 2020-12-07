using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    public static bool counting;
    float startTime;
    Text text;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.realtimeSinceStartup;
        Time.timeScale = 0;
        counting = true;
        text = GetComponent<Text>();
    }

    void Update(){
        if(Time.realtimeSinceStartup - startTime > 5f){
            Time.timeScale = 1;
            counting = false;
            gameObject.SetActive(false);
            GameObject.Find("LevelRewards").GetComponent<LevelRewards>().Hide();
        }
        text.text = (5 - (int)(Time.realtimeSinceStartup - startTime)).ToString();
    }


    
}
