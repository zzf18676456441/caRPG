using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreControl : MonoBehaviour
{
    float time = 0;
    int reward = 1;
    LevelRewards rewards;
    LevelStats stats;
    GameController controller;
    Text scoreText;
    LevelRewards.ConditionType condition;
    // Update is called once per frame
    void Start(){
        controller = GameObject.Find("GameControllerObject").GetComponent<GameController>();
        rewards = GameObject.Find("HUD").transform.Find("LevelRewards").GetComponent<LevelRewards>();
        scoreText = GameObject.Find("HUD").transform.Find("Score").GetComponent<Text>();
        stats = controller.GetPlayer().GetLevelStats();
        Rotate();
    }

    void Update()
    {
        stats = controller.GetPlayer().GetLevelStats();

        float value = 0f;
        if (Time.realtimeSinceStartup > time + 3){
            Rotate();
        }

        switch(reward){
            case 1:
                condition = rewards.rewardOneCondition;
                value = stats.stats[condition];
            break;
            case 2:
                condition = rewards.rewardTwoCondition;
                value = stats.stats[condition];
            break;
            case 3:
                condition = rewards.rewardThreeCondition;
                value = stats.stats[condition];
            break;
        }

        if (value == (int) value){
            scoreText.text = value.ToString();
        } else {
            scoreText.text = value.ToString("0.00");
        }
        if (rewards.IsMet(reward)) scoreText.color = Color.green;
        else scoreText.color = Color.red;
    }


    private void Rotate(){
        time = Time.realtimeSinceStartup;
        stats = controller.GetPlayer().GetLevelStats();
        float value = 0;
        reward++;
        if (reward>3) reward = 1;

        switch(reward){
            case 1:
                condition = rewards.rewardOneCondition;
                value = stats.stats[condition];
            break;
            case 2:
                condition = rewards.rewardTwoCondition;
                value = stats.stats[condition];
            break;
            case 3:
                condition = rewards.rewardThreeCondition;
                value = stats.stats[condition];
            break;
        }



        switch(condition){
            case LevelRewards.ConditionType.Kills:
                gameObject.GetComponent<Text>().text = "Kills:";
            break;
            case LevelRewards.ConditionType.EnemyContacts:
                gameObject.GetComponent<Text>().text = "Hits:";
            break;
            case LevelRewards.ConditionType.WallContacts:
                gameObject.GetComponent<Text>().text = "Crashes:";
            break;
            case LevelRewards.ConditionType.Brakes:
                gameObject.GetComponent<Text>().text = "Brakes:";
            break;
            case LevelRewards.ConditionType.Time:
                gameObject.GetComponent<Text>().text = "Time:";
            break;
            case LevelRewards.ConditionType.DamageDealt:
                gameObject.GetComponent<Text>().text = "Dealt:";
            break;
            case LevelRewards.ConditionType.DamageTaken:
                gameObject.GetComponent<Text>().text = "Taken:";
            break;
            case LevelRewards.ConditionType.TopSpeed:
                gameObject.GetComponent<Text>().text = "Speed:";
            break;
        }

        
    }
}
