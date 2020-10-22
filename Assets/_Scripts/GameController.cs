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

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.LoadScene("Level 1", LoadSceneMode.Single);
    }

    void FixedUpdate()
    {
        if (GetPlayer().currentHealth <= 0)
        {
            FinishLevel();
            SceneManager.LoadScene(2); //"Game Over" screen.
            GetPlayer().currentHealth = 100; //sets player health to 100 so we can get back in the game
        }
        if (GetPlayer().currentHealth < GetPlayer().maxHealth)
        {
            GetPlayer().currentHealth += .0005f + (.001f * (GetCar().GetComponent<Rigidbody2D>().velocity.magnitude));
        }
    }

    public void AddStick()
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
        car = CreateCar();
        return car;
    }

    private GameObject CreateCar(){
        GameObject car = Instantiate(carPrefab);
        car.transform.SetParent(gameObject.transform);
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

    public void FinishLevel(){
        car.transform.position = gameObject.transform.position;
        car.transform.rotation = gameObject.transform.rotation;
        car.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
        car.GetComponent<Rigidbody2D>().angularVelocity = 0f;
        player.currentNO2 = player.maxNO2;
        car.SetActive(false);
    }

    public void HandleDamage(IDamager damager, IDamagable damagable, float speed){
        damagable.ApplyDamage(damager.GetDamage(), speed);
    }

    public LevelStats GetLevelStats(){
        return new LevelStats();
    }
}

public class Player : IDamagable
{
    public float maxHealth = 100;
    public float currentHealth = 100;
    public float maxNO2 = 100;
    public float currentNO2 = 100;
    public GameController controller;
    public void ApplyDamage(Damage damage, float speed)
    {
        switch(damage.type){
            case DamageType.Fixed:
                currentHealth -= damage.baseDamage / (1 + (controller.GetCar().GetComponent<Rigidbody2D>().velocity.magnitude * .5f));
            break;
            case DamageType.Velocity:
            default:
                currentHealth -= damage.baseDamage;
            break;    
        }
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
    void ApplyDamage(Damage damage, float speed);
}

public class LevelStats{
    public int kills;
    public float time;
    public float damageTaken;
}
