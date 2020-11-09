using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    int alpha = 0;
    int frames;
    Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        CountFrom(5);
    }


    public void CountFrom(int n){
        frames = n*50;
    }
    

    void FixedUpdate()
    {
        if (frames > 0){
            if (frames % 50 == 0){
                text.text = (frames/50).ToString();
                alpha = 400;
            }
            if (alpha > 0){
                alpha -= 10;
                text.color = new Color(text.color.r, text.color.g, text.color.b, alpha/250f);
            }
            frames--;
        } else {
            gameObject.SetActive(false);
        }
        
    }
}
