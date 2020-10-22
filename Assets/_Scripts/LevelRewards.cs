using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelRewards : MonoBehaviour
{
    public enum ConditionType{Time, Kills, DamageTaken}
    public enum ComparisonType{AtMost, AtLeast, EqualTo}

    public ConditionType rewardOneCondition;
    public ComparisonType rewardOneComparison;
    public float rewardOneValue;
    public GameObject rewardOnePrefab;


    public ConditionType rewardTwoCondition;
    public ComparisonType rewardTwoComparison;
    public float rewardTwoValue;
    public GameObject rewardTwoPrefab;
    
  
    public ConditionType rewardThreeCondition;
    public ComparisonType rewardThreeComparison;
    public float rewardThreeValue;
    public GameObject rewardThreePrefab;


    private GameController controller;
    void Awake()
    {
        controller = GameObject.Find("GameControllerObject").GetComponent<GameController>();
    }

    public List<GameObject> GetRewards(){
        List<GameObject> rewards = new List<GameObject>();
        LevelStats stats = controller.GetLevelStats();
        
        ConditionType type = rewardOneCondition;
        ComparisonType comparison = rewardOneComparison;
        float value = rewardOneValue;
        if(ConditionMet(stats, type, comparison, value)) rewards.Add(rewardOnePrefab);

        type = rewardTwoCondition;
        comparison = rewardTwoComparison;
        value = rewardTwoValue;
        if(ConditionMet(stats, type, comparison, value)) rewards.Add(rewardTwoPrefab);

        type = rewardThreeCondition;
        comparison = rewardThreeComparison;
        value = rewardThreeValue;
        if(ConditionMet(stats, type, comparison, value)) rewards.Add(rewardThreePrefab);

        return rewards;
    }


    private bool ConditionMet(LevelStats stats, ConditionType type, ComparisonType comparison, float value){
        switch(type){
            case ConditionType.Kills:
                switch(comparison){
                    case ComparisonType.AtLeast:
                        return (stats.kills >= value);
                    case ComparisonType.AtMost:
                        return (stats.kills <= value);
                    case ComparisonType.EqualTo:
                        return (stats.kills == value);
                    default:
                    return false;
                }
            case ConditionType.Time:
                switch(comparison){
                    case ComparisonType.AtLeast:
                        return (stats.time >= value);
                    case ComparisonType.AtMost:
                        return (stats.time <= value);
                    case ComparisonType.EqualTo:
                        return (stats.time == value);
                    default:
                    return false;
                }
            case ConditionType.DamageTaken:
                switch(comparison){
                    case ComparisonType.AtLeast:
                        return (stats.damageTaken >= value);
                    case ComparisonType.AtMost:
                        return (stats.damageTaken <= value);
                    case ComparisonType.EqualTo:
                        return (stats.damageTaken == value);
                    default:
                    return false;
                }
            default:
            return false;
        }
    }

}
