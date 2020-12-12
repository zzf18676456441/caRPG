using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    public static bool counting;
    float startTime;
    private float pauseTime;
    private static bool paused;
    private static float lastTime;
    Text text;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.realtimeSinceStartup;
        Time.timeScale = 0;
        counting = true;
        paused = false;
        text = GetComponent<Text>();
    }

    void Update(){
        if (paused){
            pauseTime += Time.realtimeSinceStartup - lastTime;
            lastTime = Time.realtimeSinceStartup;
            return;
        }
        if(Time.realtimeSinceStartup - pauseTime - startTime > 5f){
            Time.timeScale = 1;
            counting = false;
            gameObject.SetActive(false);
            GameObject.Find("LevelRewards").GetComponent<LevelRewards>().Hide();
        }
        text.text = (5 - (int)(Time.realtimeSinceStartup - pauseTime - startTime)).ToString();
    }

    public static void Pause(){
        paused = true;
        lastTime = Time.realtimeSinceStartup;
    }

    public static void UnPause(){
        paused = false;
    }

    
}
