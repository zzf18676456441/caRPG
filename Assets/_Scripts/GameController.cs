using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject carPrefab;
    public string[] levels;

    private int nextLevel = 0;

    private Player player;
    private GameObject car;
    private LevelRewardsPassable rewardsPassable;

    public bool handbreakDown = false;
    private bool handbreakSFXPlayed = false;
    private CarSFXHandler carSFX;
    private bool started = false;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        // Create PlayerPrefs to be sited elsewhere or pulls out their data
        if (!PlayerPrefs.HasKey("CurrentLevel"))
            PlayerPrefs.SetInt("CurrentLevel", 0);
        else
            nextLevel = PlayerPrefs.GetInt("CurrentLevel");
        if (!PlayerPrefs.HasKey("Unlocks"))
            PlayerPrefs.SetString("Unlocks", "");
        else
            LoadSavedUnlocks();
    }

    /// <summary>
    /// Sets all of the saved Unlocks to be Owned.
    /// </summary>
    /// <remarks>
    /// Uses the string at PlayerPrefs["Unlocks"]. The string is broken apart and
    /// read to find the item which is owned and the list which contains it in the
    /// InventoryMaster class. Upon finding the correct prefab with a PowerupMain
    /// component, calls the SetOwned method with true.
    /// </remarks>
    private void LoadSavedUnlocks()
    {
        InventoryMaster invMast = gameObject.GetComponent<InventoryMaster>();
        // Get the things that are unlocked
        string ownedItemsStr = PlayerPrefs.GetString("Unlocks"); // follows the form "<slot>:<item name>,<slot>:<item name>,..."
        string[] ownedItems = ownedItemsStr.Split(',');
        foreach (string item in ownedItems)
        {
            if (item == "")
                continue;
            string[] itemParts = item.Split(':');
            string slot = itemParts[0];
            string itemName = itemParts[1];
            if (itemName == "stick")
                continue;
            System.Predicate<GameObject> findByName = (gO) => gO.name == itemName;
            switch (slot)
            {
                case "Bumpers":
                    invMast.BumperMods.Find(findByName).GetComponent<PowerupMain>().SetOwned(true); // Look for the game object in this list with the
                    break;                                                                          // name that matches itemName, then grab its PowerupMain and call SetOwned
                case "Engine":
                    invMast.EngineMods.Find(findByName).GetComponent<PowerupMain>().SetOwned(true);
                    break;
                case "Frame":
                    invMast.FrameMods.Find(findByName).GetComponent<PowerupMain>().SetOwned(true);
                    break;
                case "Tires":
                    invMast.TireMods.Find(findByName).GetComponent<PowerupMain>().SetOwned(true);
                    break;
                case "Trunk":
                    invMast.TrunkMods.Find(findByName).GetComponent<PowerupMain>().SetOwned(true);
                    break;
                case "Grill":
                    invMast.GrillWeapons.Find(findByName).GetComponent<PowerupMain>().SetOwned(true);
                    break;
                case "Hitch":
                    invMast.HitchWeapons.Find(findByName).GetComponent<PowerupMain>().SetOwned(true);
                    break;
                case "Roof":
                    invMast.RoofWeapons.Find(findByName).GetComponent<PowerupMain>().SetOwned(true);
                    break;
                case "Doors":
                    invMast.DoorWeapons.Find(findByName).GetComponent<PowerupMain>().SetOwned(true);
                    break;
                case "Wheels":
                    invMast.WheelWeapons.Find(findByName).GetComponent<PowerupMain>().SetOwned(true);
                    break;
            }
        }
    }

    void FixedUpdate()
    {
        if (!started)
        {
            started = true;
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }

        if (GetPlayer().currentHealth <= 0)
        {
            FinishLevel();
            SceneManager.LoadScene("GameOver", LoadSceneMode.Single); //"Game Over" screen.
            GetPlayer().currentHealth = 100; //sets player health to 100 so we can get back in the game
        }
        if (GetPlayer().currentHealth < GetPlayer().maxHealth)
        {
            GetPlayer().currentHealth += .0005f + (.001f * (GetCar().GetComponent<Rigidbody2D>().velocity.magnitude));
        }

        //audio code
        if (handbreakDown && !handbreakSFXPlayed && !(carSFX is null))
        {
            carSFX.PlaySqueal();
            handbreakSFXPlayed = true;
        }
        else if (!handbreakDown)
        {
            handbreakSFXPlayed = false;
        }
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

    private GameObject CreateCar()
    {
        GameObject car = Instantiate(carPrefab);
        car.transform.SetParent(gameObject.transform);
        car.SetActive(false);
        if (carSFX is null)
        {
            carSFX = car.GetComponentInChildren<CarSFXHandler>();
            player.SetCarSFX(carSFX);
        }
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
        player.SetupPlayer();
        return player;
    }

    public void FinishLevel()
    {
        player.GetLevelStats().SetStat(LevelRewards.ConditionType.Time, Time.timeSinceLevelLoad);
        LevelRewards levelRewards = GameObject.Find("HUD").transform.Find("LevelRewards").GetComponent<LevelRewards>();
        List<GameObject> rewards = levelRewards.GetRewards();
        rewardsPassable = levelRewards.Save();
        foreach (GameObject reward in rewards)
        {
            reward.GetComponent<PowerupMain>().SetOwned(true);
        }
        car.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        car.GetComponent<Rigidbody2D>().angularVelocity = 0f;
        car.transform.up = new Vector3(0, 1, 0);
        player.currentNO2 = player.maxNO2;
        car.SetActive(false);

        if (nextLevel > PlayerPrefs.GetInt("CurrentLevel"))
            PlayerPrefs.SetInt("CurrentLevel", nextLevel);

        SceneManager.LoadScene("LevelComplete", LoadSceneMode.Single);
    }

    public LevelRewardsPassable GetRewards()
    {
        return rewardsPassable;
    }

    public void StartGarageLevel()
    {
        SceneManager.LoadScene("Garage", LoadSceneMode.Single);
    }

    public void RetryLevel()
    {
        player.ResetStats();
        SceneManager.LoadScene(levels[nextLevel - 1], LoadSceneMode.Single);
    }

    public void StartNextLevel()
    {
        if (nextLevel >= levels.Length) nextLevel = 0;
        player.ResetStats();
        SceneManager.LoadScene(levels[nextLevel++], LoadSceneMode.Single);
    }

    public void StartNextLevelWithGarage()
    {
        if (PlayerPrefs.GetString("Unlocks").Length > 1)
        {
            StartGarageLevel();
        }
        else
        {
            if (nextLevel > 0)
            {
                StartGarageLevel();
            }
            else
            {
                StartNextLevel();
            }
        }
    }

    public void StartLevel(int index)
    {
        player.ResetStats();
        nextLevel = index;
        StartGarageLevel();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        player.ResetStats();
        car.SetActive(false);
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void StartGarageMidLevel()
    {
        nextLevel--;
        StartGarageLevel();
    }

    public void StartNewGame()
    {
        PlayerPrefs.SetInt("CurrentLevel", 0);
        PlayerPrefs.SetString("Unlocks", "");
        InventoryMaster invMaster = GetComponent<InventoryMaster>();
        foreach (List<GameObject> list in invMaster.AllItems)
        {
            foreach (GameObject obj in list)
            {
                obj.GetComponent<PowerupMain>().SetOwned(false);
            }
        }
        nextLevel = 0;
        StartNextLevel();
    }

    public void UnlockAllLevels()
    {
        PlayerPrefs.SetInt("CurrentLevel", levels.Length);
    }

    public void UnlockAllItems()
    {
        InventoryMaster invMaster = GetComponent<InventoryMaster>();
        foreach (List<GameObject> list in invMaster.AllItems)
        {
            foreach (GameObject obj in list)
            {
                obj.GetComponent<PowerupMain>().SetOwned(true);
            }
        }
    }

}

public class Player : IDamagable
{
    public float maxHealth = 100;
    public float currentHealth = 100;
    public float currentArmor = 0;
    public float maxNO2 = 100;
    public float currentNO2 = 100;
    public GameController controller;
    private Dictionary<GameObject, float> recentDamagers = new Dictionary<GameObject, float>();
    private LevelStats levelStats = new LevelStats();

    public StatPack baseStats;

    private CarSFXHandler carSFX;

    public void SetupPlayer()
    {
        baseStats = new StatPack();
        baseStats.SetAdd(StatPack.StatType.Health, 100);
        baseStats.SetAdd(StatPack.StatType.Nitro, 100);

        baseStats.SetAdd(StatPack.StatType.Weight, controller.GetCar().GetComponent<Rigidbody2D>().mass);
        baseStats.SetAdd(StatPack.StatType.TopSpeed, controller.GetCar().GetComponent<Driving>().topSpeed);
        baseStats.SetAdd(StatPack.StatType.Grip, controller.GetCar().GetComponent<Driving>().staticCoefficientOfFriction);
        baseStats.SetAdd(StatPack.StatType.Acceleration, controller.GetCar().GetComponent<Driving>().acceleration);
        GarageStats.SetBaseStats(baseStats);
        GarageStats.SetCurrentStats(baseStats);
    }

    public void SetCarSFX(CarSFXHandler sfx)
    {
        carSFX = sfx;
    }

    public void ReApplyStats(StatPack powerups)
    {
        StatPack newStats = StatPack.ApplyToBase(baseStats, powerups);
        maxHealth = newStats.GetAdd(StatPack.StatType.Health);
        maxNO2 = newStats.GetAdd(StatPack.StatType.Nitro);
        currentArmor = newStats.GetAdd(StatPack.StatType.Armor);
        controller.GetCar().GetComponent<Rigidbody2D>().mass = newStats.GetAdd(StatPack.StatType.Weight);
        controller.GetCar().GetComponent<Driving>().topSpeed = newStats.GetAdd(StatPack.StatType.TopSpeed);
        controller.GetCar().GetComponent<Driving>().staticCoefficientOfFriction = newStats.GetAdd(StatPack.StatType.Grip);
        controller.GetCar().GetComponent<Driving>().acceleration = newStats.GetAdd(StatPack.StatType.Acceleration);
        GarageStats.SetCurrentStats(newStats);
    }




    public void ResetStats()
    {
        levelStats = new LevelStats();
        currentHealth = maxHealth;
        currentNO2 = maxNO2;
    }

    public LevelStats GetLevelStats()
    {
        return levelStats;
    }

    public void ApplyDamage(Damage damage, Vector2 velocity)
    {
        if (recentDamagers.ContainsKey(damage.source))
        {
            if (Time.time < recentDamagers[damage.source] + 1f) return;
        }
        recentDamagers[damage.source] = Time.time;

        float damageTaken = damage.baseDamage;

        switch (damage.type)
        {
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

        foreach (DamageFlag flag in damage.flags.Keys)
        {
            switch (flag)
            {
                case DamageFlag.Knockback:
                    controller.GetCar().GetComponent<Rigidbody2D>().AddForce(velocity * damage.knockbackForce / velocity.magnitude);
                    break;
                case DamageFlag.Wall:
                    levelStats.AddStat(LevelRewards.ConditionType.WallContacts, 1f);
                    break;
                case DamageFlag.Piercing:
                    damageTaken += currentArmor;
                    break;
                default:
                    break;
            }
        }

        if (damageTaken - currentArmor <= 0)
        {
            damageTaken = 0;
        }
        else
        {
            damageTaken -= currentArmor;
        }

        if (damageTaken >= 1)
        {
            carSFX.PlayDamage();
        }

        currentHealth -= damageTaken;
        levelStats.AddStat(LevelRewards.ConditionType.DamageTaken, damageTaken);
    }

    public void AddNO2(float amount)
    {
        if (currentNO2 < maxNO2)
            currentNO2 += amount;
    }
}

public static class DamageSystem
{
    public static void ApplyDamage(IDamager damager, IDamagable damagable, Vector2 velocity)
    {
        damagable.ApplyDamage(damager.GetDamage(), velocity);
    }
    public static void ApplyDamage(IDamager damager, IDamagable damagable)
    {
        damagable.ApplyDamage(damager.GetDamage(), new Vector2(0, 0));
    }
}


public enum DamageType { Fixed, VelocityAmplified, VelocityMitigated };
public enum DamageFlag { Impact, Explosion, Projectile, Knockback, Wall, Piercing };

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

public class LevelStats
{
    public Dictionary<LevelRewards.ConditionType, float> stats;

    public LevelStats()
    {
        stats = new Dictionary<LevelRewards.ConditionType, float>();
        stats.Add(LevelRewards.ConditionType.Brakes, 0f);
        stats.Add(LevelRewards.ConditionType.DamageDealt, 0f);
        stats.Add(LevelRewards.ConditionType.DamageTaken, 0f);
        stats.Add(LevelRewards.ConditionType.EnemyContacts, 0f);
        stats.Add(LevelRewards.ConditionType.Kills, 0f);
        stats.Add(LevelRewards.ConditionType.Time, 0f);
        stats.Add(LevelRewards.ConditionType.TopSpeed, 0f);
        stats.Add(LevelRewards.ConditionType.WallContacts, 0f);
    }

    public void AddStat(LevelRewards.ConditionType type, float value)
    {
        stats[type] += value;
    }

    public void SetStat(LevelRewards.ConditionType type, float value)
    {
        stats[type] = value;
    }

}
