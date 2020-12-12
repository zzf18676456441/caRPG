using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    public BossAudioHandler sfx;
    public EnemyAIShoot firingAI;
    public EnemyAI chargingAI;
    public Enemy stats;
    public SinglePointMovement spm;
    public float firingTimer;
    public float spawnTimer;
    public Animator anim;
    public GameObject bruiser, shooter, bomber, bigBruiser, bigShooter;
    public GameObject spawnParticles;
    public int maxEnemies;
    private List<GameObject> currentEnemies = new List<GameObject>();
    private bool animChanged;
    private float maxHP;
    private float timeFired;
    private float timeSpawn;
    private bool spawnedReinforcements = false;
    private Transform player;

    void Awake()
    {
        player = GameObject.Find("GameControllerObject").GetComponent<GameController>().GetCar().transform;
        timeFired = firingTimer;
        timeSpawn = spawnTimer;
        maxHP = stats.health;
    }
    void Update()
    {
        if(stats.baseDamage != 125 && stats.health/maxHP <= .5f)
        {
            stats.baseDamage = 125;
        }
        //handle ai switching based on distance and time
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > 70)
        {
            bool swapped = chargingAI;
            chargingAI.enabled = false;
            firingAI.enabled = true;
            swapped = chargingAI && swapped;
            animChanged = swapped;
        }
        else
        {
            sfx.PlayActionSound();
            bool swapped = chargingAI;
            chargingAI.enabled = true;
            firingAI.enabled = false;
            chargingAI.startChase = true;
            spm.acceleration = 4;
            timeFired = firingTimer;
            swapped = chargingAI && swapped;
            animChanged = swapped;
        }
        if (firingAI.enabled && timeFired < 0)
        {
            bool swapped = chargingAI;
            chargingAI.enabled = true;
            firingAI.enabled = false;
            chargingAI.startChase = true;
            spm.acceleration = 8;
            swapped = chargingAI && swapped;
            animChanged = swapped;
        }
        else if (firingAI.enabled && timeFired >= 0)
        {
            timeFired -= Time.deltaTime;
        }

        //anim code
        if (chargingAI.enabled && animChanged)
        {
            sfx.actionSoundTime = 0;
            anim.SetBool("isCharging", true);
            anim.Play("BossWalk");
            animChanged = false;
        }
        else if (animChanged)
        {
            sfx.PlayFiringSound();
            anim.SetBool("isCharging", false);
            anim.Play("BossFiring");
            animChanged = false;
        }

        //spawning additional enemies
        //every SpawnTimer seconds, spawn 2-4 enemies.
        if (timeSpawn < 0 && (distance > 5 && distance < 80) && currentEnemies.Count < maxEnemies)
        {
            if (Random.Range(0, 3) == 0)
            {
                if (Random.Range(0, 2) == 0)
                {
                    GameObject spawn = Instantiate(bigBruiser, transform.position + new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f)), Quaternion.identity);
                    currentEnemies.Add(spawn);
                }
                else
                {
                    GameObject spawn = Instantiate(bigShooter, transform.position + new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f)), Quaternion.identity);
                    currentEnemies.Add(spawn);
                }
            }
            else
            {
                int numToSpawn;
                if (stats.health / maxHP > .5f)
                    numToSpawn = Random.Range(2, 5);
                else
                    numToSpawn = Random.Range(4, 7);
                for (int i = 0; i < numToSpawn; i++)
                {
                    int spawnType = Random.Range(1, 6);
                    // 1/2 chance for bruiser, 1/3 chance for shooter, 1/6 chance for bomber
                    GameObject spawn = null;
                    switch (spawnType)
                    {
                        case 0:
                        case 1:
                        case 2:
                            spawn = Instantiate(bruiser, transform.position + new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f)), Quaternion.identity);
                            break;
                        case 3:
                        case 4:
                            spawn = Instantiate(shooter, transform.position + new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f)), Quaternion.identity);
                            break;
                        case 5:
                            spawn = Instantiate(bomber, transform.position + new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f)), Quaternion.identity);
                            break;
                    }
                    GameObject spawnGore = Instantiate(spawnParticles, spawn.transform.position, Quaternion.identity);
                    currentEnemies.Add(spawn);
                }
            }
            if (stats.health / maxHP <= .5f)
                timeSpawn = spawnTimer / 2;
            else
                timeSpawn = spawnTimer;
        }
        else
        {
            timeSpawn -= Time.deltaTime;
        }
    }
}
