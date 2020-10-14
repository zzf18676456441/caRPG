using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Tooltip("The game object which this camera should follow")]
    private GameObject toFollow;
    GameController controller;
    void Awake()
    {
        controller = GameObject.Find("GameControllerObject").GetComponent<GameController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        toFollow = controller.GetCar();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(toFollow.transform.position.x, toFollow.transform.position.y, -10f);
    }
}
