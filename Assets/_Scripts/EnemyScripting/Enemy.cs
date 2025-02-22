﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable, IDamager
{

    public float health = 50;

    public float baseDamage = 20;
    public float nO2Reward = 10f;
    public DamageType damageType = DamageType.VelocityMitigated;
    public DamageFlag[] damageFlags = { DamageFlag.Impact };
    public float knockbackForce = 0f;
    public GameObject aliengore;
    public GameObject alienblood;
    public EnemyAudioHandler audioHandler;
    private float recoveryTimer = .1f;
    private float recoveryTime;

    private SpriteRenderer sprite;
    private Dictionary<GameObject, float> recentDamagers = new Dictionary<GameObject, float>();
    private Color originColor;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        originColor = sprite.color;
        recoveryTime = recoveryTimer;
    }
    void Update()
    {
        if (recoveryTime <= 0 && sprite.color.a == 0)
        {
            sprite.color = originColor;
            recoveryTime = recoveryTimer;
        }
        else if (recoveryTime > 0 && sprite.color.a == 0)
        {
            recoveryTime -= Time.deltaTime;
        }
    }

    private void die()
    {
        Instantiate(aliengore);
        GameObject g = Instantiate(alienblood, transform.position, Quaternion.identity);
        g.GetComponent<ParticleSystem>().Play();
        Destroy(gameObject);
    }

    public Damage GetDamage()
    {
        Damage result = new Damage(baseDamage, damageType, this.gameObject);
        foreach (DamageFlag flag in damageFlags)
        {
            result.AddDamageFlag(flag);
            if (flag == DamageFlag.Knockback) { result.knockbackForce = knockbackForce; }
        }
        return result;
    }

    public void ApplyDamage(Damage damage, Vector2 velocity)
    {
        Player player = GameObject.Find("GameControllerObject").GetComponent<GameController>().GetPlayer();

        if (recentDamagers.ContainsKey(damage.source))
        {
            if (this.name == "Boss")
            {
                if (Time.time < recentDamagers[damage.source] + 0.5f) return;
            }
            else
            if (Time.time < recentDamagers[damage.source] + 0.2f) return;
        }
        recentDamagers[damage.source] = Time.time;
        float damageTaken = damage.baseDamage;

        switch (damage.type)
        {
            case DamageType.VelocityMitigated:
                damageTaken /= (1 + (gameObject.GetComponent<Rigidbody2D>().velocity.magnitude * .5f));
                break;
            case DamageType.VelocityAmplified:
                damageTaken *= (velocity.magnitude + 25f) / 25f;
                break;
            case DamageType.Fixed:
            default:
                break;
        }

        foreach (DamageFlag flag in damage.flags.Keys)
        {
            switch (flag)
            {
                case DamageFlag.Wall:
                    if (velocity.magnitude > 25f)
                    {
                        damageTaken *= 25;
                    }
                    else
                        damageTaken = 0;
                    break;
                default:
                    player.GetLevelStats().AddStat(LevelRewards.ConditionType.EnemyContacts, 1);
                    break;
            }
        }

        health -= damageTaken;
        if (damageTaken > 0)
        {
            try
            {
                audioHandler.PlayDeathSound();
            }
            catch (Exception) { }
            sprite.color = new Color(0f, 0f, 0f, 0f);
        }
        player.GetLevelStats().AddStat(LevelRewards.ConditionType.DamageDealt, damageTaken);
        if (health <= 0)
        {
            player.AddNO2(nO2Reward);
            player.GetLevelStats().AddStat(LevelRewards.ConditionType.Kills, 1);
            die();
        }
    }

    /// Collision damage
    private void OnCollisionEnter2D(Collision2D collision)
    {
        IDamager damager = (IDamager)collision.collider.GetComponent(typeof(IDamager));
        IDamagable damagable = (IDamagable)collision.collider.GetComponent(typeof(IDamagable));
        Enemy enemyCheck = (Enemy)collision.collider.GetComponent(typeof(Enemy));
        if (enemyCheck != null) return;
        if (damager != null)
        {
            DamageSystem.ApplyDamage(damager, this, collision.relativeVelocity);
        }
        if (damagable != null)
        {
            DamageSystem.ApplyDamage(this, damagable, collision.relativeVelocity);
        }
    }


    void FlashRed()
    {
        sprite.color = Color.red;
    }
}
