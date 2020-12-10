using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPaint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(Random.value, Random.value, Random.value, 1f);
        gameObject.GetComponent<Rigidbody2D>().rotation += -90 + 180 * Random.value;
    }

}
