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
        Invoke("AddStick", 2f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if (GetPlayer().currentHealth <= 0)
        {
            SceneManager.LoadScene(2); //"Game Over" screen.
            GetPlayer().currentHealth = 100; //sets player health to 100 so we can get back in the game
        }
        if (GetPlayer().currentHealth < GetPlayer().maxHealth)
        {
            GetPlayer().currentHealth += .0005f + (.001f * (GetCar().GetComponent<Rigidbody2D>().velocity.magnitude));
        }
    }

    void AddStick()
    {
        PowerupMain tStick = Instantiate<GameObject>(stickPrefab).GetComponent<PowerupMain>();
        tStick.ApplyTo(car.GetComponent<PowerupManager>());
    }

    public GameObject GetCar()
    {
        if (car != null)
        {
            return car;
        }
        car = Instantiate(carPrefab);
        return car;
    }

    public Player GetPlayer()
    {
        if (player != null)
        {
            return player;
        }
        player = new Player();
        player.controller = this;
        return player;
    }
}

public class Player : IDamagable
{
    public float maxHealth = 100;
    public float currentHealth = 100;
    public GameController controller;
    public void ApplyDamage(Damage damage)
    {
        currentHealth -= damage.baseDamage / (1 + (controller.GetCar().GetComponent<Rigidbody2D>().velocity.magnitude * .5f));
    }
}



public enum DamageType { Fixed, Velocity };
public enum DamageFlag { Fire, Sonic, Physical, Piercing };

public class Damage
{
    public float baseDamage;
    public DamageType type;
    public Dictionary<DamageFlag, DamageFlag> flags;

    public Damage(float _baseDamage, DamageType _type)
    {
        baseDamage = _baseDamage;
        type = _type;
        flags = new Dictionary<DamageFlag, DamageFlag>();
    }

    public void AddDamageFlag(DamageFlag flag)
    {
        flags.Add(flag, flag);
    }
}

public interface IDamager
{
    Damage GetDamage();
}

public interface IDamagable
{
    void ApplyDamage(Damage damage);
}
