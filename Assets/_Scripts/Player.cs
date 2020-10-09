using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public class Player : MonoBehaviour
{
    public float health;
    public float damage;

    void Update()
    {
        if(health <= 0)
        {
            die();
        }
    }

    private void die()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Equipment")
        {
            other.gameObject.GetComponent<CircleCollider2D>().enabled = false;
            other.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            PowerupMain equip = other.gameObject.GetComponent<PowerupMain>();
            equip.ApplyTo(GetComponent<PowerupManager>());
        }
    }
}
*/