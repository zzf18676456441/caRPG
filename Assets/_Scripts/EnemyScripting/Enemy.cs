using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable, IDamager
{

    public float health = 50;

    public float baseDamage = 20;
    public DamageType damageType = DamageType.VelocityMitigated;
    public DamageFlag[] damageFlags = { DamageFlag.Impact };
    public float knockbackForce = 0f;
    public GameObject aliengore;
    public GameObject alienblood;

    private SpriteRenderer sprite;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (health <= 0)
        {
            die();
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
        Damage result = new Damage(baseDamage, damageType);
        foreach (DamageFlag flag in damageFlags)
        {
            result.AddDamageFlag(flag);
            if (flag == DamageFlag.Knockback) { result.knockbackForce = knockbackForce; }
        }
        return result;
    }

    public void ApplyDamage(Damage damage, Vector2 speed)
    {
        FlashRed();
        health -= damage.baseDamage;
    }

    IEnumerator FlashRed()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(1f);
        sprite.color = Color.white;
    }
}
