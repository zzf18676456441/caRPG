using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompleteController : MonoBehaviour
{
    LevelStats levelStats;
    // Start is called before the first frame update
    void Start()
    {
        levelStats = GameObject.Find("GameControllerObject").GetComponent<GameController>().GetPlayer().GetLevelStats();
        UpdateText();
    }
    private void UpdateText(){
        Text[] textElements = gameObject.GetComponentsInChildren<Text>();
        foreach(Text text in textElements){
            switch(text.name){
                case "KillsText":
                    text.text = levelStats.stats[LevelRewards.ConditionType.Kills].ToString();
                break;
                case "DamageTakenText":
                    text.text = levelStats.stats[LevelRewards.ConditionType.DamageTaken].ToString();
                break;
                case "DamageDealtText":
                    text.text = levelStats.stats[LevelRewards.ConditionType.DamageDealt].ToString();
                break;
                case "TopSpeedText":
                    text.text = (int)(levelStats.stats[LevelRewards.ConditionType.TopSpeed] * 2.23694) + " mph";
                break;
                case "EnemyContactsText":
                    text.text = levelStats.stats[LevelRewards.ConditionType.EnemyContacts].ToString();
                break;
                case "WallContactsText":
                    text.text = levelStats.stats[LevelRewards.ConditionType.WallContacts].ToString();
                break;
                case "Brakes":
                    text.text = levelStats.stats[LevelRewards.ConditionType.Brakes].ToString() + "s";
                break;
                case "TimeText":
                    int time = (int)levelStats.stats[LevelRewards.ConditionType.Time];
                    text.text = time/60 + ":" + (time%60).ToString("00");
                break;
                default:
                break;
            }
        }
    }
}
