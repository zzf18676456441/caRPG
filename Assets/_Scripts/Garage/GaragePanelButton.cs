using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaragePanelButton : MonoBehaviour
{
    public void ActivateWeaponPanel(){
        gameObject.GetComponent<Button>().interactable = false;
        gameObject.transform.parent.Find("ModSelect").GetComponent<Button>().interactable = true;
        gameObject.transform.parent.Find("Weapons").gameObject.SetActive(true);
        gameObject.transform.parent.Find("Mods").gameObject.SetActive(false);       
    }

    public void ActivateModPanel(){
        gameObject.GetComponent<Button>().interactable = false;
        gameObject.transform.parent.Find("WeaponSelect").GetComponent<Button>().interactable = true;
        gameObject.transform.parent.Find("Weapons").gameObject.SetActive(false);
        gameObject.transform.parent.Find("Mods").gameObject.SetActive(true);       
    }
}
