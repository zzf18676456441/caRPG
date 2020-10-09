using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject carPrefab;
    public GameObject stickPrefab;
    private Player player;
    private GameObject car;

    // Start is called before the first frame update

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.LoadScene("Level 1", LoadSceneMode.Single);
    }
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
        tStick.ApplyTo(car.GetComponent<PowerupManager>());
    }

    public GameObject GetCar(){
        if (car != null){
            return car;
        }
        car = Instantiate(carPrefab);
        return car;
    }

    public Player GetPlayer(){
        if (player != null){
            return player;
        }
        player = new Player();
        return player;
    }
}

public class Player{
    public float maxHealth = 100;
    public float currentHealth = 60;
}
