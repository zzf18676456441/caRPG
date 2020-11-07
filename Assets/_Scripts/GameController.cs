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
    public void ApplyDamage(Damage damage, Vector2 velocity)
    {
        float damageTaken = damage.baseDamage;

        switch(damage.type){
            case DamageType.VelocityMitigated:
                damageTaken /= (1 + (controller.GetCar().GetComponent<Rigidbody2D>().velocity.magnitude * .5f));
            break;
            case DamageType.VelocityAmplified:
            case DamageType.Fixed:
            default:
            break;    
        }

        foreach(DamageFlag flag in damage.flags.Keys){
            switch(flag){
                case DamageFlag.Knockback:
                    controller.GetCar().GetComponent<Rigidbody2D>().AddForce(velocity * damage.knockbackForce / velocity.magnitude);
                break;
                default:
                break;
            }
        }

        currentHealth -= damageTaken;

    }
}

public static class DamageSystem
{
    public static void ApplyDamage(IDamager damager, IDamagable damagable, Vector2 velocity){
        damagable.ApplyDamage(damager.GetDamage(), velocity);
    }
    public static void ApplyDamage(IDamager damager, IDamagable damagable){
        damagable.ApplyDamage(damager.GetDamage(), new Vector2(0,0));
    }
}


public enum DamageType { Fixed, VelocityAmplified, VelocityMitigated };
public enum DamageFlag { Impact, Explosion, Projectile, Knockback };

public class Damage
{
    public float baseDamage;
    public DamageType type;
    public Dictionary<DamageFlag, DamageFlag> flags;
    public float knockbackForce;

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
    void ApplyDamage(Damage damage, Vector2 velocity);
}

public class LevelStats{
    public int kills;
    public float time;
    public float damageTaken;
}
