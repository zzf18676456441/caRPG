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
        FlashRed();
        float damageTaken = damage.baseDamage;

        switch(damage.type){
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

        health -= damageTaken;
        if (health <= 0)
        {
            die();
        }
    }

    IEnumerator FlashRed()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(1f);
        sprite.color = Color.white;
    }
}
