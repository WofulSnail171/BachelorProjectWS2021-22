using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextReplacer
{
    static public string[] replacements = new string[] 
    { 
        "$HeroId",
        "$HeroJob",
        "$HeroRace",
        "$HeroRar",
        "$HeroVal",
        "$HeroPhy",
        "$HeroMag",
        "$HeroSoc",
        "$RandHeroId",
        "$RandHeroJob",
        "$RandHeroRace",
        "$RandHeroRar",
        "$RandHeroVal",
        "$RandHeroPhy",
        "$RandHeroMag",
        "$RandHeroSoc",
        "$Enemy",
        "$NodeType",
        "$PathType",
        "$EventName"
    };
    public static string ReplaceWordsBulk(string _input)
    {
        string result = _input;
        foreach (var entry in replacements) {
            if(_input.Contains(entry))
                result = _input.Replace(entry, ReplaceWord(entry));
        }
        return result;
    }

    static string ReplaceWord(string _word)
    {
        string result = "---";
        if(!DatabaseManager.CheckDatabaseValid() || DatabaseManager._instance.dungeonData.currentRun == null || !DatabaseManager._instance.dungeonData.currentRun.valid || DungeonManager._instance.currentCalcRun == null)
        {
            return "upsi";
        }
        switch (_word)
        {
            case "$HeroId":
                result = DatabaseManager._instance.dungeonData.currentRun.party[DungeonManager._instance.currentCalcRun.nextHero].heroId;
                break;
            case "$HeroJob":
                result = DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[ DatabaseManager._instance.dungeonData.currentRun.party[DungeonManager._instance.currentCalcRun.nextHero].heroId].job;
                break;
            case "$HeroRace":
                result = DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[ DatabaseManager._instance.dungeonData.currentRun.party[DungeonManager._instance.currentCalcRun.nextHero].heroId].race;

                break;
            case "$HeroRar":
                result = DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[ DatabaseManager._instance.dungeonData.currentRun.party[DungeonManager._instance.currentCalcRun.nextHero].heroId].rarity.ToString();

                break;
            case "$HeroVal":
                switch (DungeonManager._instance.currentCalcRun.currentNode.nodeEvent.statType)
                {
                    case "physical":
                        result = DatabaseManager._instance.dungeonData.currentRun.party[DungeonManager._instance.currentCalcRun.nextHero].pVal.ToString();
                        break;
                    case "magical":
                        result = DatabaseManager._instance.dungeonData.currentRun.party[DungeonManager._instance.currentCalcRun.nextHero].mVal.ToString();
                        break;
                    case "social":
                        result = DatabaseManager._instance.dungeonData.currentRun.party[DungeonManager._instance.currentCalcRun.nextHero].sVal.ToString();
                        break;
                    default:
                        break;
                }

                break;
            case "$HeroPhy":
                result = DatabaseManager._instance.dungeonData.currentRun.party[DungeonManager._instance.currentCalcRun.nextHero].pVal.ToString();
                break;
            case "$HeroMag":
                result = DatabaseManager._instance.dungeonData.currentRun.party[DungeonManager._instance.currentCalcRun.nextHero].mVal.ToString();
                break;
            case "$HeroSoc":
                result = DatabaseManager._instance.dungeonData.currentRun.party[DungeonManager._instance.currentCalcRun.nextHero].sVal.ToString();
                break;
            case "$RandHeroId":
                result = DatabaseManager._instance.dungeonData.currentRun.party[DungeonManager._instance.currentCalcRun.RandomNum(0, DatabaseManager._instance.dungeonData.currentRun.party.Count)].heroId;

                break;
            case "$RandHeroJob":
                result = DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[DatabaseManager._instance.dungeonData.currentRun.party[DungeonManager._instance.currentCalcRun.RandomNum(0, DatabaseManager._instance.dungeonData.currentRun.party.Count)].heroId].job;
                break;
            case "$RandHeroRace":
                result = DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[DatabaseManager._instance.dungeonData.currentRun.party[DungeonManager._instance.currentCalcRun.RandomNum(0, DatabaseManager._instance.dungeonData.currentRun.party.Count)].heroId].race;
                break;
            case "$RandHeroRar":
                result = DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[DatabaseManager._instance.dungeonData.currentRun.party[DungeonManager._instance.currentCalcRun.RandomNum(0, DatabaseManager._instance.dungeonData.currentRun.party.Count)].heroId].rarity.ToString();
                break;
            case "$RandHeroVal":
                switch (DungeonManager._instance.currentCalcRun.currentNode.nodeEvent.statType)
                {
                    case "physical":
                        result = DatabaseManager._instance.dungeonData.currentRun.party[DungeonManager._instance.currentCalcRun.RandomNum(0, DatabaseManager._instance.dungeonData.currentRun.party.Count)].pVal.ToString();
                        break;
                    case "magical":
                        result = DatabaseManager._instance.dungeonData.currentRun.party[DungeonManager._instance.currentCalcRun.RandomNum(0, DatabaseManager._instance.dungeonData.currentRun.party.Count)].mVal.ToString();
                        break;
                    case "social":
                        result = DatabaseManager._instance.dungeonData.currentRun.party[DungeonManager._instance.currentCalcRun.RandomNum(0, DatabaseManager._instance.dungeonData.currentRun.party.Count)].sVal.ToString();
                        break;
                    default:
                        break;
                }
                break;
            case "$RandHeroPhy":
                result = DatabaseManager._instance.dungeonData.currentRun.party[DungeonManager._instance.currentCalcRun.RandomNum(0, DatabaseManager._instance.dungeonData.currentRun.party.Count)].pVal.ToString();
                break;
            case "$RandHeroMag":
                result = DatabaseManager._instance.dungeonData.currentRun.party[DungeonManager._instance.currentCalcRun.RandomNum(0, DatabaseManager._instance.dungeonData.currentRun.party.Count)].mVal.ToString();
                break;
            case "$RandHeroSoc":
                result = DatabaseManager._instance.dungeonData.currentRun.party[DungeonManager._instance.currentCalcRun.RandomNum(0, DatabaseManager._instance.dungeonData.currentRun.party.Count)].sVal.ToString();
                break;
            case "$Enemy":
                result = DungeonManager._instance.currentCalcRun.currentNode.eventEnemy;
                //ToDo
                break;
            case "$NodeType":
                result = DungeonManager._instance.currentCalcRun.currentNode.nodeType;
                //ToDo
                break;
            case "$PathType":
                if(DungeonManager._instance.currentCalcRun.currentNode.chosenPathIndex != -1)
                {
                    result = DungeonManager._instance.currentCalcRun.currentNode.nextPaths[DungeonManager._instance.currentCalcRun.currentNode.chosenPathIndex];
                }
                //ToDo
                break;
            case "$EventName":
                result = DungeonManager._instance.currentCalcRun.currentNode.nodeEvent.eventName;
                //ToDo
                break;
            default:
                break;
        }

        return result;
    }
}

[System.Serializable]
public class Replacement
{
    public string from;
    public string to;
}
