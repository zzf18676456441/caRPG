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
    private int gameFreeze;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
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

    public void StartLevel(string levelName){
        SceneManager.LoadScene(levelName, LoadSceneMode.Single);
    }
}

public class Player : IDamagable
{
    public float maxHealth = 100;
    public float currentHealth = 100;
    public float maxNO2 = 100;
    public float currentNO2 = 100;
    public GameController controller;
    private Dictionary<GameObject, float> recentDamagers = new Dictionary<GameObject, float>();

    public void ApplyDamage(Damage damage, Vector2 velocity)
    {
        if(recentDamagers.ContainsKey(damage.source)){
            if (Time.time < recentDamagers[damage.source] + 1f) return;
        }
        recentDamagers[damage.source] = Time.time;

        float damageTaken = damage.baseDamage;

        switch(damage.type){
            case DamageType.VelocityMitigated:
                damageTaken /= (1 + (controller.GetCar().GetComponent<Rigidbody2D>().velocity.magnitude * .5f));
            break;
            case DamageType.VelocityAmplified:
                damageTaken *= velocity.magnitude / 25f;
            break;
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
public enum DamageFlag { Impact, Explosion, Projectile, Knockback, Wall };

public class Damage
{
    public float baseDamage;
    public DamageType type;
    public Dictionary<DamageFlag, DamageFlag> flags;
    public float knockbackForce;
    public GameObject source;

    public Damage(float _baseDamage, DamageType _type, GameObject _source)
    {
        baseDamage = _baseDamage;
        type = _type;
        flags = new Dictionary<DamageFlag, DamageFlag>();
        source = _source;
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
    public Dictionary<LevelRewards.ConditionType, float> stats = new Dictionary<LevelRewards.ConditionType, float>();

    public void AddStat(LevelRewards.ConditionType type, float value){
        stats[type] += value;
    }

    public void SetStat(LevelRewards.ConditionType type, float value){
        stats[type] = value;
    }
}
