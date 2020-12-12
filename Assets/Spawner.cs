using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;
    public float delay;

    private float despawnedTime = -1;
    private GameObject spawned;
    // Update is called once per frame
    void Update()
    {
        if (spawned == null){
            if (despawnedTime < 0){
                spawned = Instantiate(prefab, transform.position, Quaternion.identity);
                despawnedTime = delay;
            } else {
                despawnedTime -= Time.deltaTime;
            }
        }
    }
}
