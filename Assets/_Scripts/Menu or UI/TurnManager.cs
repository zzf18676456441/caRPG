using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public GameObject turnPartner;
    public bool passedThrough;
    public TurnType turnType;
    private UserInterface ui;

    public void Awake()
    {
        ui = GameObject.Find("HUD").GetComponent<UserInterface>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (turnPartner.GetComponent<TurnManager>().passedThrough)
            {
                ui.HideSign();
            }
            else
            {
                ui.DrawSign(turnType);
            }
        }
    }
}

public enum TurnType
{
    Right, Left, U_Turn_Right, U_Turn_Left, Fork, Right_Shift, Left_Shift
}
