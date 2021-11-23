using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class DungeonManager : MonoBehaviour
{
    public static DungeonManager _instance;
    public static int PityGrowth = 3;
    public static int MaxGrowth = 8;

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

    public int chosenDailyDungeonIndex = 0;

    public LayoutList layoutList;
    public CalculatedDungeonRun currentCalcRun;
    public PlayerHero rewardHero;
    public bool CheckCalcRun()
    {
        if (currentCalcRun != null && currentCalcRun.currentNode != null)
            return true;
        return false;
    }
    //public Transform playerParty;

    private bool AutoPLay = true;
    public float AutoPlayWaitTimeSec = 1.0f;

    
    // Start is called before the first frame update
    void OnEnable()
    {        
        StartCoroutine(AutoplayRoutine());
        currentCalcRun = null;
    }
    private void OnDisable()
    {        
        StopCoroutine(AutoplayRoutine());
    }

    public void StartDungeonRun()
    {
        //utilizes one of the daily dungeons. creates a dungeonRun and a calculated dungeonRun
    }

    public void ProceedDungeonRun()
    {
        //utilizes saved dungeonRun object to recreate dungeonRun
    }

    public void ShowDungeonLayout(int _dailyDungeonIndex)
    {
        HideDungeonLayouts();
        if(_dailyDungeonIndex < DatabaseManager._instance.dungeonData.dailyDungeons.Length)
        {
            DatabaseManager._instance.dungeonData.dailyDungeons[_dailyDungeonIndex].dungeonLayout.gameObject.SetActive(true);
            DatabaseManager._instance.dungeonData.dailyDungeons[_dailyDungeonIndex].dungeonLayout.FocusCamera();
        }            
    }

    public void HideDungeonLayouts()
    {
        foreach (var dd in DatabaseManager._instance.dungeonData.dailyDungeons)
        {
            dd.dungeonLayout.gameObject.SetActive(false);
        }
    }

    public int CurrentStep()
    {
        double elapsedSeconds = 0;
        if (DatabaseManager._instance.dungeonData.currentRun != null && DatabaseManager._instance.dungeonData.currentRun.date != null)
        {
            elapsedSeconds = DateTime.Now.Subtract(DateTime.Parse(DatabaseManager._instance.dungeonData.currentRun.date)).TotalSeconds;
            if(AutoPlayWaitTimeSec != 0)
            {
                elapsedSeconds /= AutoPlayWaitTimeSec;
            }
        }
        return (int)elapsedSeconds;
    }

    public void CreateDailyDungeons()
    {
        //create a new set of dailyDungeons
        if(DatabaseManager._instance.dungeonData == null)
        {
            DatabaseManager._instance.dungeonData = new DungeonData();
            DatabaseManager._instance.dungeonData.currentRun = new DungeonRun { valid = false };
        }
        if(DatabaseManager._instance.dungeonData.currentRun != null && DatabaseManager._instance.dungeonData.currentRun.valid)
        {
            DatabaseManager._instance.dungeonData.currentRun.dungeon.InitDungeonLayout();
            DatabaseManager._instance.dungeonData.currentRun.dungeon.dungeonLayout.gameObject.SetActive(false);
        }
        if (DatabaseManager._instance.dungeonData.dailyDungeons != null && DatabaseManager._instance.dungeonData.dailyDungeons.Length > 0)
        {
            //check if current daily dungeon data is older than 24 hours
            if (false )//DateTime.Parse(DatabaseManager._instance.dungeonData.dailyDungeons[0].date).Date == DateTime.Now.Date)
            {
                Debug.Log("Current daily dungeons are still valid");
                //if (DatabaseManager._instance.dungeonData.dailyDungeons[0].dungeonLayout == null)
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
                date = DateTime.Now.ToString("o"),
                questName = DatabaseManager._instance.eventData.basicQuestDeck[UnityEngine.Random.Range(0, DatabaseManager._instance.eventData.basicQuestDeck.Length)].eventName,
                type = DungeonType.basic,
                difficutlyIndex = i
            };
            tempList.Add(tempDungeon);
        }
        DailyDungeon tempDoomDungeon = new DailyDungeon
        {
            dailySeed = UnityEngine.Random.Range(1, 3000),
            layoutId = layoutList.layouts[UnityEngine.Random.Range(0, layoutList.layouts.Length)].name,
            date = DateTime.Now.ToString("o"),
            questName = DatabaseManager._instance.eventData.doomQuestDeck[UnityEngine.Random.Range(0, DatabaseManager._instance.eventData.doomQuestDeck.Length)].eventName,
            type = DungeonType.doom,
            difficutlyIndex = DatabaseManager._instance.rewardTable.dungeonDifficulties.Count - 1
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
                date = DateTime.Now.ToString("o"),
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
        if (currentCalcRun == null || !DatabaseManager._instance.dungeonData.currentRun.valid) //currentCalcRun.currentStep > DatabaseManager._instance.dungeonData.currentRun.maxSteps)
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
            NextStepRun();
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
        currentCalcRun = null;
    }

    void StartCalcRun()
    {
        if (DatabaseManager._instance.dungeonData.currentRun == null || DatabaseManager._instance.dungeonData.currentRun.valid == false)
            return;
        DatabaseManager._instance.dungeonData.currentRun.dungeon.dungeonLayout.SetupDungeonRunSeed(DatabaseManager._instance.dungeonData.currentRun.dungeonSeed);
        currentCalcRun = new CalculatedDungeonRun(DatabaseManager._instance.dungeonData.currentRun);
        //playerParty.position = DatabaseManager._instance.dungeonData.currentRun.dungeon.dungeonLayout.startNode.transform.position;
    }

    void StepCalcRun()
    {
        currentCalcRun.currentStep++;
        if(DeleventSystem.DungeonStep != null)
            DeleventSystem.DungeonStep();
        switch (currentCalcRun.currentActivity)
        {
            case DungeonActivity.eventStart:
                CalcEventStart();
                break;
            case DungeonActivity.eventEnd:
                CalcEventEnd();
                break;
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

    void EnterNewActivityState(DungeonActivity _newActivity, bool _doExit = true)
    {
        if (_doExit)
        {
            //Exit behavior if necessary
            switch (currentCalcRun.currentActivity)
            {
                case DungeonActivity.eventStart:
                    break;
                case DungeonActivity.eventEnd:
                    break;
                case DungeonActivity.eventHandling:
                    break;
                case DungeonActivity.pathHandling:
                    break;
                case DungeonActivity.pathChoosing:
                    break;
                case DungeonActivity.startQuest:
                    break;
                case DungeonActivity.endQuest:
                    break;
                default:
                    break;
            }
        }


        //currently flat 20 steps per interaction
        currentCalcRun.SetActivitySteps(20);
        currentCalcRun.currentActivity = _newActivity;
        //enter behvior/steup if necessary
        switch (_newActivity)
        {
            case DungeonActivity.eventStart:
                currentCalcRun.UpdateLog("the party encountered " + currentCalcRun.currentNode.nodeEvent.eventName);
                EnterNewActivityState(DungeonActivity.eventHandling);
                break;
            case DungeonActivity.eventEnd:
                switch (currentCalcRun.currentNode.nodeEvent.statType)
                {
                    case "physical":
                        currentCalcRun.pStatGrowth += currentCalcRun.currentNode.currentGrowth;
                        break;
                    case "magical":
                        currentCalcRun.mStatGrowth += currentCalcRun.currentNode.currentGrowth; 
                        break;
                    case "social":
                        currentCalcRun.sStatGrowth += currentCalcRun.currentNode.currentGrowth; 
                        break;
                    default:
                        break;
                }
                currentCalcRun.UpdateLog("the party finished " + currentCalcRun.currentNode.nodeEvent.eventName);
                break;
            case DungeonActivity.eventHandling:

                break;
            case DungeonActivity.pathHandling:
                currentCalcRun.currentNode.chosenPathIndex = currentCalcRun.RandomNum(0, currentCalcRun.currentNode.nextPaths.Count);
                currentCalcRun.UpdateLog("the party decided on the " + currentCalcRun.currentNode.nextPaths[currentCalcRun.currentNode.chosenPathIndex] + " path");
                //playerParty.position = currentCalcRun.currentNode.PathPosition(currentCalcRun.currentNode.chosenPathIndex);
                break;
            case DungeonActivity.pathChoosing:
                currentCalcRun.UpdateLog("The party Finished the event and chooses a path now...");
                //string text = "#Hero tut stuff";
                //text = text.Replace("#Hero", DatabaseManager._instance.dungeonData.currentRun.party[DungeonManager._instance.currentCalcRun.nextHero].heroId);
                //currentCalcRun.UpdateLog(text);
                break;
            case DungeonActivity.startQuest:
                currentCalcRun.UpdateLog("the party embarked on a new adventure");
                break;
            case DungeonActivity.endQuest:
                currentCalcRun.UpdateLog("the party finished their epic adventure");
                break;
            default:
                break;
        }


    }

    void CalcEventStart()
    {
        if (currentCalcRun.remainingActivitySteps > 0)
        {
            currentCalcRun.remainingActivitySteps--;
        }
        else
        {
            EnterNewActivityState(DungeonActivity.eventHandling);
        }
    }

    void CalcEventEnd()
    {
        if (currentCalcRun.remainingActivitySteps > 0)
        {
            currentCalcRun.remainingActivitySteps--;
        }
        else
        {
            EnterNewActivityState(DungeonActivity.pathChoosing);
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

                if (currentCalcRun.nextHero >= DatabaseManager._instance.dungeonData.currentRun.party.Length)
                {
                    //fineished a round
                    currentCalcRun.nextHero = 0;
                    //display end round text for event
                    currentCalcRun.currentNode.currentGrowth--;
                    if (currentCalcRun.currentNode.currentGrowth < -3)
                        currentCalcRun.currentNode.currentGrowth = -3;
                    currentCalcRun.UpdateLog("Party finished their round - growth sunk to " + currentCalcRun.currentNode.currentGrowth.ToString());
                    currentCalcRun.AffectRewardHealth(-1);
                }
                else
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

                }
                if (currentCalcRun.currentNode.eventHealth < 0)
                {
                    currentCalcRun.currentNode.eventHealth = 0;
                    //event finished
                }
            }
            else
            {
                EnterNewActivityState(DungeonActivity.eventEnd);
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
                EnterNewActivityState(DungeonActivity.eventStart);
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
            EnterNewActivityState(DungeonActivity.pathHandling);
            currentCalcRun.AffectRewardHealth(-1);
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
            EnterNewActivityState(DungeonActivity.pathChoosing);
        }
    }

    void CalcRunEndQuest()
    {
        if (currentCalcRun.remainingActivitySteps > 0)
        {
            currentCalcRun.remainingActivitySteps--;
        }
        else if(!currentCalcRun.ended)
        {
            currentCalcRun.ended = true;
            //DatabaseManager._instance.dungeonData.currentRun.valid = false;
            DatabaseManager._instance.SaveGameDataLocally();
            Debug.Log("Run Ended");
            if (DeleventSystem.dungeonRunFinished != null)
            {
                DeleventSystem.dungeonRunFinished();
            }
            //Debug.LogError("Finished Boiiii");
        }
    }


    //should only be called when the dungeon Run gets dissolved
    public void ApplyGrowth()
    {
        foreach (var hero in DatabaseManager._instance.dungeonData.currentRun.party)
        {
            //hero.ApplyGrowth(currentCalcRun.pStatGrowth, currentCalcRun.mStatGrowth, currentCalcRun.sStatGrowth);
            hero.ApplyGrowth(currentCalcRun.pStatGrowth, currentCalcRun.mStatGrowth, currentCalcRun.sStatGrowth);
            hero.runs += 1;
            hero.status = HeroStatus.Idle;
        }
    }

    public void EventRewardShardHandling()
    {
        if(DatabaseManager._instance.dungeonData.currentRun.dungeon.type == DungeonType.basic)
        {
            DatabaseManager._instance.activePlayerData.shards++;
            if (DatabaseManager._instance.activePlayerData.shards >= 5)
                DatabaseManager._instance.activePlayerData.shards = 5;
        }
        else
        {
            //You dont get any shards haha
        }
    }

    public void EventRewardHeroHandling()
    {
        int rewardTier = currentCalcRun.rewardHealthBar / 10;
        if(currentCalcRun.rewardHealthBar % 10 != 0)
        {
            //integer division rounds down but we want it to go up
            rewardTier += 1;
        }
        //do something with the reward tier but for now just let it get you a random hero
        rewardHero = HeroCreator.GetHeroByRewardTier(rewardTier);
        
    }

    public void AddRewardHeroToInventory()
    {
        if(rewardHero != null)
        {
            rewardHero.invIndex = -1;
            DatabaseManager._instance.activePlayerData.inventory.Add(rewardHero);
            DatabaseManager.ValidateInventory();
        }
    }

    public void DiscardRewardHero()
    {
        rewardHero = null;
    }

    public void WrapUpDungeon()
    {
        ApplyGrowth();
        currentCalcRun = null;

        DatabaseManager._instance.dungeonData.currentRun.valid = false;
        Destroy(DatabaseManager._instance.dungeonData.currentRun.dungeon.dungeonLayout.gameObject);
        DatabaseManager._instance.dungeonData.currentRun.dungeon.InitDungeonLayout();
        DatabaseManager._instance.dungeonData.currentRun.dungeon.dungeonLayout.gameObject.SetActive(false);


        DatabaseManager._instance.SaveGameDataLocally();
        ServerCommunicationManager._instance.DoServerRequest(Request.PushPlayerData);
        ServerCommunicationManager._instance.DoServerRequest(Request.PushDungeonData);
    }



    IEnumerator AutoplayRoutine()
    {
        while (AutoPLay)
        {
            //Update Loop for dungeonRun
            if(CheckCalcRun() && DatabaseManager._instance.dungeonData.currentRun.valid)
            {
                NextStepRun();
            }
            yield return new WaitForSeconds(AutoPlayWaitTimeSec);
        }
    }
}
