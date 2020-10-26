using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable, IDamager
{

    public float health = 50;

    public float baseDamage = 20;

    void Update()
    {
        if(health <= 0)
        {
            die();
        }
    }


    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            health -= collision.gameObject.GetComponent<Player>().damage;
            StartCoroutine(Wait());
        }
    }
    
    IEnumerator Wait()
    {
        gameObject.GetComponent<EnemyAI>().enabled = false;
        yield return new WaitForSeconds(3);
        gameObject.GetComponent<EnemyAI>().enabled = true;
    }
    */

    private void die()
    {
        Destroy(gameObject);
    }

    public Damage GetDamage(){
        return new Damage(baseDamage, DamageType.Fixed);
    }

    public void ApplyDamage(Damage damage, float speed){
        health -= damage.baseDamage;
    }
}
