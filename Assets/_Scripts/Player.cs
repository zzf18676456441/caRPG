using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
