using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartHandler : MonoBehaviour
{
    public SpriteRenderer backgroundImage;
    void Update()
    {
        //makes the background flash
        backgroundImage.color = new Color(.6f, .35f, .35f, Mathf.Abs(Mathf.Sin(Time.time)) + .1f);
        if (Input.anyKey)
        {
            SceneManager.LoadScene(0);
        }
    }
}
