using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBarrier : MonoBehaviour
{
    public GameObject boss;

    void Update(){
        if (boss==null)
            gameObject.SetActive(false);
    }
    


}
