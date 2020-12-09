using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public GameObject turnPartner;
    public bool passedThrough = false;
    public Sprite turnSprite;
    private UserInterface ui;

    public void Awake()
    {
        ui = GameObject.Find("HUD").GetComponent<UserInterface>();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (passedThrough)
            {
                ui.HideSign();
                passedThrough = false;
            }
            else if (turnPartner.GetComponent<TurnManager>().passedThrough)
            {
                ui.HideSign();
                passedThrough = false;
                turnPartner.GetComponent<TurnManager>().passedThrough = false;
            }
            else
            {
                ui.DrawSign(turnSprite);
                passedThrough = true;
            }
        }
    }
}
