using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject stickPrefab;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("AddStick",2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AddStick(){
        PowerupMain tStick = Instantiate<GameObject>(stickPrefab).GetComponent<PowerupMain>();
        tStick.ApplyTo(GameObject.Find("car").GetComponent<PowerupManager>());
    }
}
