using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    // Start is called before the first frame update
    GameController controller;
    void Awake()
    {
        controller = GameObject.Find("GameControllerObject").GetComponent<GameController>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.tag == "Player"){
            controller.GetPlayer().currentHealth = 400;
            SceneManager.LoadScene(1);
            controller.Invoke("AddStick", 1f);
        }
    }
}

