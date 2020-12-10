using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float velx = 50f;
    public float vely = 1f;
    public float x = 0f;
    public float y = 0f;
    public float rot = 0f;

    public int lifespan = 1200;

    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.position = new Vector2(x, y);
        rb.AddRelativeForce(new Vector2(velx, vely));
    }

    void Update()
    {
        if (lifespan <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            lifespan--;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Weapon" || other.gameObject.tag == "Equipment")
        {
        }
        else if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<Enemy>().ApplyDamage(new Damage(20, DamageType.VelocityMitigated, this.gameObject), new Vector2(0,0));
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
