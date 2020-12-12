using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelRewards : MonoBehaviour
{
    public enum ConditionType{Time, Kills, DamageTaken, DamageDealt, TopSpeed, EnemyContacts, WallContacts, Brakes}
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

    public bool inLevel = true;

    private GameController controller;
    void Awake()
    {
        controller = GameObject.Find("GameControllerObject").GetComponent<GameController>();
    }

    void Start(){
        if(!inLevel){
            Load(controller.GetRewards());
        }
        UpdateText();
        Image image = gameObject.transform.Find("Reward1").GetComponent<Image>();
        image.sprite = rewardOnePrefab.GetComponent<SpriteRenderer>().sprite;
        if (rewardOnePrefab.GetComponent<PowerupMain>().IsOwned()){
            image.color = new Color(0.2f, 0.2f, 0.2f, 1);
            transform.Find("Reward1Text").GetComponent<Text>().color = Color.green;
        }
        image = gameObject.transform.Find("Reward2").GetComponent<Image>();
        image.sprite = rewardTwoPrefab.GetComponent<SpriteRenderer>().sprite;
        if (rewardTwoPrefab.GetComponent<PowerupMain>().IsOwned()){
            image.color = new Color(0.2f, 0.2f, 0.2f, 1);
            transform.Find("Reward2Text").GetComponent<Text>().color = Color.green;
        }
        image = gameObject.transform.Find("Reward3").GetComponent<Image>();
        image.sprite = rewardThreePrefab.GetComponent<SpriteRenderer>().sprite;
        if (rewardThreePrefab.GetComponent<PowerupMain>().IsOwned()){
            image.color = new Color(0.2f, 0.2f, 0.2f, 1);
            transform.Find("Reward3Text").GetComponent<Text>().color = Color.green;
        }
        if(!inLevel){
            ShowSuccessFailure();
            List<GameObject> rewards = GetRewards();
            foreach (GameObject reward in rewards)
            {
                reward.GetComponent<PowerupMain>().SetOwned(true);
            }
        }
    }



    public void Show(){
        gameObject.SetActive(true);
    }

    public void Hide(){
        gameObject.SetActive(false);
    }

    public void Load(LevelRewardsPassable source){
        rewardOneCondition = source.rewardOneCondition;
        rewardOneComparison = source.rewardOneComparison;
        rewardOneValue = source.rewardOneValue;
        rewardOnePrefab = source.rewardOnePrefab;

        rewardTwoCondition = source.rewardTwoCondition;
        rewardTwoComparison = source.rewardTwoComparison;
        rewardTwoValue = source.rewardTwoValue;
        rewardTwoPrefab = source.rewardTwoPrefab;
        
        rewardThreeCondition = source.rewardThreeCondition;
        rewardThreeComparison = source.rewardThreeComparison;
        rewardThreeValue = source.rewardThreeValue;
        rewardThreePrefab = source.rewardThreePrefab;
    }

    public LevelRewardsPassable Save(){
        return new LevelRewardsPassable(this);
    }

    public List<GameObject> GetRewards(){
        List<GameObject> rewards = new List<GameObject>();
        LevelStats stats = controller.GetPlayer().GetLevelStats();
        
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

    public int IsMet(int reward){
        LevelStats stats = controller.GetPlayer().GetLevelStats();
        ConditionType type = rewardOneCondition;
        ComparisonType comparison = rewardOneComparison;
        float value = rewardOneValue;
        if (reward > 1) {
            if (reward == 2){
                if (rewardTwoPrefab.GetComponent<PowerupMain>().IsOwned()) return 2;
                type = rewardTwoCondition;
                comparison = rewardTwoComparison;
                value = rewardTwoValue; 
            } else {
                if (rewardThreePrefab.GetComponent<PowerupMain>().IsOwned()) return 2;
                type = rewardThreeCondition;
                comparison = rewardThreeComparison;
                value = rewardThreeValue; 
            }
        } else if (rewardOnePrefab.GetComponent<PowerupMain>().IsOwned()) return 2;
        
        if(ConditionMet(stats, type, comparison, value)){
            return 1;
        }

        return 0;
    }

    private bool ConditionMet(LevelStats stats, ConditionType type, ComparisonType comparison, float value){
        float statValue = stats.stats[type];

        switch(comparison){
            case ComparisonType.AtLeast:
                return (statValue >= value);
            case ComparisonType.AtMost:
                return (statValue <= value);
            case ComparisonType.EqualTo:
                return (statValue == value);
            default:
            return false;
        }
    }

    private void UpdateText(){
        Text[] textElements = gameObject.GetComponentsInChildren<Text>();
        foreach(Text text in textElements){
            switch(text.name){
                case "Reward1Text":
                    text.text = GetDescription(rewardOneCondition,rewardOneComparison,rewardOneValue);
                break;
                case "Reward2Text":
                    text.text = GetDescription(rewardTwoCondition,rewardTwoComparison,rewardTwoValue);
                break;
                case "Reward3Text":
                    text.text = GetDescription(rewardThreeCondition,rewardThreeComparison,rewardThreeValue);
                break;
                default:
                break;
            }
        }
    }

    public void ShowSuccessFailure(){
        for(int reward = 1; reward < 4; reward++){
            string target = "Reward" + reward + "Success";
            Text[] textElements = gameObject.GetComponentsInChildren<Text>();
            int result;
            foreach(Text text in textElements){
                if(text.name == target){
                    result = IsMet(reward);
                    if (result == 1){
                        text.color = Color.green;
                        text.text = "SUCCESS";
                    } else if (result == 0) {
                        text.color = Color.red;
                        text.text = "FAILURE";
                    } else {
                        text.color = Color.blue;
                        text.text = "OWNED";
                    }
                }
            }
        }        
    }

    private string GetDescription(ConditionType condition, ComparisonType comparison, float value){
        string comparisonText;

        switch(comparison){
            case ComparisonType.AtLeast:
                comparisonText = "at least";
            break;
            case ComparisonType.AtMost:
                comparisonText = "at most";
            break;
            default:
                comparisonText = "exactly";
            break;
        }

        switch(condition){
            case (ConditionType.Brakes):
                if (comparison == ComparisonType.EqualTo && value == 0){
                    return "Complete the level without ever using your brakes.";
                }
                return "Use your brakes for a total of " + comparisonText + " " + value + " seconds.";
            case (ConditionType.DamageDealt):
                if (comparison == ComparisonType.EqualTo && value == 0){
                    return "Complete the level without dealing any damage.";
                }
                return "Deal " + comparisonText + " " + value + " damage.";
            case (ConditionType.DamageTaken):
                if (comparison == ComparisonType.EqualTo && value == 0){
                    return "Complete the level without taking any damage.";
                }
                return "Take " + comparisonText + " " + value + " damage.";
            case (ConditionType.EnemyContacts):
                if (comparison == ComparisonType.EqualTo && value == 0){
                    return "Complete the level without ever touching an enemy.";
                }
                return "Hit enemies " + comparisonText + " " + value + " times.";
            case (ConditionType.Kills):
                if (comparison == ComparisonType.EqualTo && value == 0){
                    return "Complete the level without killing any enemies.";
                }
                return "Kill " + comparisonText + " " + value + " enemies.";
            case (ConditionType.Time):
                return "Finish the level in " + comparisonText + " " + value + " seconds.";
            case (ConditionType.TopSpeed):
                if (comparison == ComparisonType.AtMost){
                    return "Complete the level without ever going above " + value + " mph.";
                }
                return "Reach a top speed of " + comparisonText + " " + value + " mph.";
            case (ConditionType.WallContacts):
                if (comparison == ComparisonType.EqualTo && value == 0){
                    return "Complete the level without hitting a wall.";
                }
                return "Hit walls " + comparisonText + " " + value + " times.";
            default:
            return "";
        }
      
    }

}

public class LevelRewardsPassable{
    public LevelRewards.ConditionType rewardOneCondition;
    public LevelRewards.ComparisonType rewardOneComparison;
    public float rewardOneValue;
    public GameObject rewardOnePrefab;


    public LevelRewards.ConditionType rewardTwoCondition;
    public LevelRewards.ComparisonType rewardTwoComparison;
    public float rewardTwoValue;
    public GameObject rewardTwoPrefab;
    
  
    public LevelRewards.ConditionType rewardThreeCondition;
    public LevelRewards.ComparisonType rewardThreeComparison;
    public float rewardThreeValue;
    public GameObject rewardThreePrefab;

    public LevelRewardsPassable(LevelRewards source){
        rewardOneCondition = source.rewardOneCondition;
        rewardOneComparison = source.rewardOneComparison;
        rewardOneValue = source.rewardOneValue;
        rewardOnePrefab = source.rewardOnePrefab;

        rewardTwoCondition = source.rewardTwoCondition;
        rewardTwoComparison = source.rewardTwoComparison;
        rewardTwoValue = source.rewardTwoValue;
        rewardTwoPrefab = source.rewardTwoPrefab;
        
        rewardThreeCondition = source.rewardThreeCondition;
        rewardThreeComparison = source.rewardThreeComparison;
        rewardThreeValue = source.rewardThreeValue;
        rewardThreePrefab = source.rewardThreePrefab;
    }
}
