using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickKiller : MonoBehaviour
{
    private int minFrames = 50;
    // Update is called once per frame
    void Update()
    {
        minFrames--;
        if (Input.anyKey && minFrames < 0){
            Destroy(gameObject);
        }
    }
}
