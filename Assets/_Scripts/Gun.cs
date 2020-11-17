using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    GameController controller;
    public float bulletSpeed = 10;
    public GameObject BulletPrefab;
    public bool rightSide;
    public bool leftSide;
    public bool straight;

    void Awake()
    {
        controller = GameObject.Find("GameControllerObject").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Fire();
        }
    }

    void Fire()
    {
        if (rightSide && !leftSide)
        {
            Bullet bullet = Instantiate<GameObject>(BulletPrefab, transform.position, transform.rotation).GetComponent<Bullet>();
            var pos = transform.position;
            bullet.x = pos.x;
            bullet.y = pos.y;
            float angle = AngleAboutY();
            bullet.velx = 2000f * Mathf.Cos(angle);
            bullet.vely = 2000f * Mathf.Sin(angle);
        }
        else if (!rightSide && leftSide)
        {
            Bullet bullet = Instantiate<GameObject>(BulletPrefab, transform.position, transform.rotation).GetComponent<Bullet>();
            var pos = transform.position;
            bullet.x = pos.x;
            bullet.y = pos.y;
            float angle = AngleAboutY();
            bullet.velx = -2000f * Mathf.Cos(angle);
            bullet.vely = -2000f * Mathf.Sin(angle);
        }
        else if (straight && !rightSide && !leftSide)
        {
            Bullet bullet = Instantiate<GameObject>(BulletPrefab, transform.position, transform.rotation).GetComponent<Bullet>();
            var pos = transform.position;
            bullet.x = pos.x;
            bullet.y = pos.y;
            float angle = AngleAboutY() + Mathf.PI / 2;
            bullet.velx = 2000f * Mathf.Cos(angle);
            bullet.vely = 2000f * Mathf.Sin(angle);
        }
        else if (straight && rightSide && leftSide)
        {
            Bullet bullet = Instantiate<GameObject>(BulletPrefab, transform.position, transform.rotation).GetComponent<Bullet>();
            var pos = transform.position;
            bullet.x = pos.x;
            bullet.y = pos.y;
            float angle = AngleAboutY() + Mathf.PI / 2;
            bullet.velx = 2000f * Mathf.Cos(angle);
            bullet.vely = 2000f * Mathf.Sin(angle);

            Bullet bullet1 = Instantiate<GameObject>(BulletPrefab, transform.position, transform.rotation).GetComponent<Bullet>();
            bullet1.x = pos.x;
            bullet1.y = pos.y;
            angle = AngleAboutY() + Mathf.PI / 4;
            bullet1.velx = 2000f * Mathf.Cos(angle);
            bullet1.vely = 2000f * Mathf.Sin(angle);

            Bullet bullet2 = Instantiate<GameObject>(BulletPrefab, transform.position, transform.rotation).GetComponent<Bullet>();
            bullet2.x = pos.x;
            bullet2.y = pos.y;
            angle = AngleAboutY() + 3 * Mathf.PI / 4;
            bullet2.velx = 2000f * Mathf.Cos(angle);
            bullet2.vely = 2000f * Mathf.Sin(angle);
        }
    }

    //https://answers.unity.com/questions/362690/getting-the-rotation-of-an-object-does-not-return.html
    float AngleAboutY()
    {
        Vector3 objFwd = transform.forward;
        float angle = Vector3.Angle(objFwd, Vector3.forward);
        float sign = Mathf.Sign(Vector3.Cross(objFwd, Vector3.forward).y);
        return angle * sign;
    }
}
