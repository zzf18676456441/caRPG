using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStart : MonoBehaviour
{
    GameController controller;
    void Awake(){
        controller = GameObject.Find("GameControllerObject").GetComponent<GameController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = controller.GetCar();
        player.transform.position = gameObject.transform.position;
        player.transform.rotation = gameObject.transform.rotation;
        player.SetActive(true);
        player.GetComponent<Driving>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
