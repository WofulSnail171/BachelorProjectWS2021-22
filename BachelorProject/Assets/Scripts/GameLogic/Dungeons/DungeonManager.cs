using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class DungeonManager : MonoBehaviour
{
    public static DungeonManager _instance;
    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            layoutList = Resources.Load<LayoutList>("LayoutList");
            layoutList.CreateDictionary();
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    public LayoutList layoutList;
    public CalculatedDungeonRun currentCalcRun;
    //public Transform playerParty;

    
    // Start is called before the first frame update
    void OnEnable()
    {
        DeleventSystem.eventDataDownloaded += CreateDailyDungeons;
    }
    private void OnDisable()
    {
        DeleventSystem.eventDataDownloaded -= CreateDailyDungeons;
    }

    public void StartDungeonRun()
    {
        //utilizes one of the daily dungeons. creates a dungeonRun and a calculated dungeonRun
    }

    public void ProceedDungeonRun()
    {
        //utilizes saved dungeonRun object to recreate dungeonRun
    }

    public void CreateDailyDungeons()
    {
        //create a new set of dailyDungeons
        if(DatabaseManager._instance.dungeonData == null)
        {
            DatabaseManager._instance.dungeonData = new DungeonData();
            DatabaseManager._instance.dungeonData.currentRun = new DungeonRun { valid = false };
        }
        if (DatabaseManager._instance.dungeonData.dailyDungeons != null && DatabaseManager._instance.dungeonData.dailyDungeons.Length > 0)
        {
            //check if current daily dungeon data is older than 24 hours
            if (DateTime.Parse(DatabaseManager._instance.dungeonData.dailyDungeons[0].date).Date == DateTime.Now.Date)
            {
                Debug.Log("Current daily dungeons are still valid");
                if (DatabaseManager._instance.dungeonData.dailyDungeons[0].dungeonLayout == null)
                    InitDungeonLayouts();
                return;
            }
        }
        List<DailyDungeon> tempList = new List<DailyDungeon>();
        int numBasicDungeons = 3;
        for (int i = 0; i < numBasicDungeons; i++)
        {
            DailyDungeon tempDungeon = new DailyDungeon
            {
                dailySeed = UnityEngine.Random.Range(1, 3000),
                layoutId = layoutList.layouts[UnityEngine.Random.Range(0, layoutList.layouts.Length)].name,
                date = DateTime.Now.ToString(),
                questName = DatabaseManager._instance.eventData.basicQuestDeck[UnityEngine.Random.Range(0, DatabaseManager._instance.eventData.basicQuestDeck.Length)].eventName,
                type = DungeonType.basic
            };
            tempList.Add(tempDungeon);
        }
        DailyDungeon tempDoomDungeon = new DailyDungeon
        {
            dailySeed = UnityEngine.Random.Range(1, 3000),
            layoutId = layoutList.layouts[UnityEngine.Random.Range(0, layoutList.layouts.Length)].name,
            date = DateTime.Now.ToString(),
            questName = DatabaseManager._instance.eventData.doomQuestDeck[UnityEngine.Random.Range(0, DatabaseManager._instance.eventData.doomQuestDeck.Length)].eventName,
            type = DungeonType.doom
        };
        tempList.Add(tempDoomDungeon);
        DatabaseManager._instance.dungeonData.dailyDungeons = tempList.ToArray();
        DatabaseManager._instance.SaveGameDataLocally();
        InitDungeonLayouts();
        //probably try to save data online
    }

    public void InitDungeonLayouts()
    {
        foreach (var dailyDungeon in DatabaseManager._instance.dungeonData.dailyDungeons)
        {
            dailyDungeon.InitDungeonLayout();
            dailyDungeon.dungeonLayout.gameObject.SetActive(false);
        }
    }

    public DungeonRun CreateDungeonRun(int _dailyDungeon)
    {        
        DungeonRun result = null;
        List<PlayerHero> chosenParty = new List<PlayerHero>(); // overhaul later
        foreach (PlayerHero hero in DatabaseManager._instance.activePlayerData.inventory)
        {
            if (hero.status == HeroStatus.Exploring)
                chosenParty.Add(hero);
        }
        if (_dailyDungeon <= DatabaseManager._instance.dungeonData.dailyDungeons.Length && _dailyDungeon > 0 && chosenParty.Count > 0)
        {
            DailyDungeon chosenDungeon = DatabaseManager._instance.dungeonData.dailyDungeons[_dailyDungeon - 1];
            result = new DungeonRun
            {
                dungeon = chosenDungeon,
                date = DateTime.Now.ToString(),
                valid = false,
                dungeonSeed = UnityEngine.Random.Range(0, 500),
                initialRewardTier = DatabaseManager._instance.activePlayerData.rewardTierBuff,
                randomNums = new List<RandomNum>()
            };
            DatabaseManager._instance.activePlayerData.rewardTierBuff = 0;
            result.party = chosenParty.ToArray();
            result.valid = true;
            DatabaseManager._instance.dungeonData.currentRun = result;
            DatabaseManager._instance.dungeonData.currentRun.dungeon.dungeonLayout.SetupDungeonRunSeed(DatabaseManager._instance.dungeonData.currentRun.dungeonSeed);
            CalculateMaxStep(result);
            result.valid = true;
        }
        return result;
    }

    public void NextStepRun()
    {
        if (currentCalcRun == null)
            return;
        StepCalcRun();
    }

    //refresh Run and calc until step
    public void CalculateRun(int _targetStep)
    {
        currentCalcRun = null;
        //UnityEngine.Random.InitState(DatabaseManager._instance.dungeonData.currentRun.dungeonSeed);
        StartCalcRun();
        if (_targetStep > DatabaseManager._instance.dungeonData.currentRun.maxSteps)
            _targetStep = DatabaseManager._instance.dungeonData.currentRun.maxSteps;
        DatabaseManager._instance.dungeonData.currentRun.dungeon.dungeonLayout.ResetNodes();
        for (int i = 0; i <= _targetStep; i++)
        {
            StepCalcRun();
        }

        //UnityEngine.Random.InitState((int)DateTime.Now.Ticks);
    }

    //calculate until max step for setup purposes
    void CalculateMaxStep(DungeonRun _dungeonRun)
    {
        currentCalcRun = null;
        UnityEngine.Random.InitState(DatabaseManager._instance.dungeonData.currentRun.dungeonSeed);

        currentCalcRun = new CalculatedDungeonRun(DatabaseManager._instance.dungeonData.currentRun);
        //playerParty.position = DatabaseManager._instance.dungeonData.currentRun.dungeon.dungeonLayout.startNode.transform.position;
        DatabaseManager._instance.dungeonData.currentRun.dungeon.dungeonLayout.ResetNodes();
        while (currentCalcRun.currentNode != DatabaseManager._instance.dungeonData.currentRun.dungeon.dungeonLayout.endNode || currentCalcRun.remainingActivitySteps > 0)
        {
            StepCalcRun();
        }
        _dungeonRun.maxSteps = currentCalcRun.currentStep;
        DatabaseManager._instance.dungeonData.currentRun.dungeon.dungeonLayout.SetupDungeonRunSeed(DatabaseManager._instance.dungeonData.currentRun.dungeonSeed);

        UnityEngine.Random.InitState((int)DateTime.Now.Ticks);
    }

    void StartCalcRun()
    {
        if (DatabaseManager._instance.dungeonData.currentRun == null || DatabaseManager._instance.dungeonData.currentRun.valid == false)
            return;
        currentCalcRun = new CalculatedDungeonRun(DatabaseManager._instance.dungeonData.currentRun);
        //playerParty.position = DatabaseManager._instance.dungeonData.currentRun.dungeon.dungeonLayout.startNode.transform.position;
    }

    void StepCalcRun()
    {
        currentCalcRun.currentStep++;
        switch (currentCalcRun.currentActivity)
        {
            case DungeonActivity.eventHandling:
                CalcRunEventHandling();
                break;
            case DungeonActivity.pathHandling:
                CalcRunPathHandling();
                break;
            case DungeonActivity.pathChoosing:
                CalcRunPathChoosing();
                break;
            case DungeonActivity.startQuest:
                CalcRunStartQuest();
                break;
            case DungeonActivity.endQuest:
                CalcRunEndQuest();
                break;
            default:
                break;
        }
    }

    void CalcRunEventHandling()
    {
        if(currentCalcRun.remainingActivitySteps > 0)
        {
            currentCalcRun.remainingActivitySteps--;
        }
        else
        {
            if (currentCalcRun.currentNode.eventHealth > 0)
            {
                //intriguing event logic
                currentCalcRun.remainingActivitySteps = 20;
                //trigger a zwischentext?
                switch (currentCalcRun.currentNode.nodeEvent.statType)
                {
                    case "physical":
                        currentCalcRun.currentNode.eventHealth -= DatabaseManager._instance.dungeonData.currentRun.party[currentCalcRun.nextHero].pVal;
                        currentCalcRun.UpdateLog(DatabaseManager._instance.dungeonData.currentRun.party[currentCalcRun.nextHero].heroId + " dealt " + DatabaseManager._instance.dungeonData.currentRun.party[currentCalcRun.nextHero].pVal.ToString() + " physical Damage");
                        break;
                    case "magical":
                        currentCalcRun.currentNode.eventHealth -= DatabaseManager._instance.dungeonData.currentRun.party[currentCalcRun.nextHero].mVal;
                        currentCalcRun.UpdateLog(DatabaseManager._instance.dungeonData.currentRun.party[currentCalcRun.nextHero].heroId + " dealt " + DatabaseManager._instance.dungeonData.currentRun.party[currentCalcRun.nextHero].mVal.ToString() + " magical Damage");
                        break;
                    case "social":
                        currentCalcRun.currentNode.eventHealth -= DatabaseManager._instance.dungeonData.currentRun.party[currentCalcRun.nextHero].sVal;
                        currentCalcRun.UpdateLog(DatabaseManager._instance.dungeonData.currentRun.party[currentCalcRun.nextHero].heroId + " dealt " + DatabaseManager._instance.dungeonData.currentRun.party[currentCalcRun.nextHero].sVal.ToString() + " social Damage");
                        break;
                    default:
                        break;
                }
                currentCalcRun.nextHero++;
                if (currentCalcRun.nextHero >= DatabaseManager._instance.dungeonData.currentRun.party.Length)
                    currentCalcRun.nextHero = 0;
                if (currentCalcRun.currentNode.eventHealth < 0)
                    currentCalcRun.currentNode.eventHealth = 0;

            }
            else
            {
                currentCalcRun.remainingActivitySteps = 20;
                string text = "#Hero tut stuff";
                currentCalcRun.UpdateLog("The party Finished the event and chooses a path now...");
                text = text.Replace("#Hero", DatabaseManager._instance.dungeonData.currentRun.party[DungeonManager._instance.currentCalcRun.nextHero].heroId);
                currentCalcRun.UpdateLog(text);
                currentCalcRun.currentActivity = DungeonActivity.pathChoosing;
            }
        }
    }

    void CalcRunPathHandling()
    {
        if (currentCalcRun.remainingActivitySteps > 0)
        {
            currentCalcRun.remainingActivitySteps--;
        }
        else
        {
            currentCalcRun.remainingActivitySteps = 20;
            currentCalcRun.currentNode = currentCalcRun.currentNode.nextNodes[currentCalcRun.currentNode.chosenPathIndex]; 
            //playerParty.position = currentCalcRun.currentNode.transform.position;
            if(currentCalcRun.currentNode.nextNodes == null || currentCalcRun.currentNode.nextNodes.Length <= 0)
            {
                currentCalcRun.currentActivity = DungeonActivity.endQuest;
                currentCalcRun.UpdateLog("the party arrived at their final destination");
            }
            else
            {
                currentCalcRun.UpdateLog("the party encountered " + currentCalcRun.currentNode.nodeEvent.eventName);
                currentCalcRun.currentActivity = DungeonActivity.eventHandling;
            }

        }
    }

    void CalcRunPathChoosing()
    {
        if (currentCalcRun.remainingActivitySteps > 0)
        {
            currentCalcRun.remainingActivitySteps--;
        }
        else
        {
            currentCalcRun.remainingActivitySteps = 20;
            currentCalcRun.currentActivity = DungeonActivity.pathHandling;
            currentCalcRun.currentNode.chosenPathIndex = currentCalcRun.RandomNum(0, currentCalcRun.currentNode.nextPaths.Count);
            currentCalcRun.UpdateLog("the party decided on the " + currentCalcRun.currentNode.nextPaths[currentCalcRun.currentNode.chosenPathIndex] + " path");
            //playerParty.position = currentCalcRun.currentNode.PathPosition(currentCalcRun.currentNode.chosenPathIndex);
        }
    }

    void CalcRunStartQuest()
    {
        if(currentCalcRun.remainingActivitySteps > 0)
        {
            currentCalcRun.remainingActivitySteps--;
        }
        else
        {
            currentCalcRun.remainingActivitySteps = 20;
            currentCalcRun.currentActivity = DungeonActivity.pathChoosing;
            currentCalcRun.UpdateLog("the party embarked on a new adventure");
        }
    }

    void CalcRunEndQuest()
    {
        if (currentCalcRun.remainingActivitySteps > 0)
        {
            currentCalcRun.remainingActivitySteps--;
        }
        else
        {
            currentCalcRun.remainingActivitySteps = 20;
            currentCalcRun.UpdateLog("the party finished their epic adventure");
            //Debug.LogError("Finished Boiiii");
        }
    }
}
