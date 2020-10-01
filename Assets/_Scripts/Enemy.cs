using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float health = 50;

    void Update()
    {
        if(health <= 0)
        {
            die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
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

    private void die()
    {
        Destroy(gameObject);
    }
}
