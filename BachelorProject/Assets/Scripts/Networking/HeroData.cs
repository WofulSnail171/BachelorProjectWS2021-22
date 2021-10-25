using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerHero
{
    public string heroId;
    public HeroStatus status;

    public int pVal;
    public int pPot;
    public int mVal;
    public int mPot;
    public int sVal;
    public int sPot;

    public string lastOwner;
    public string origOwner;
    public int invIndex;
    public int traded;
    public int runs;
}

[System.Serializable]
public class IncomingHeroData
{
    public DefaultHero[] defaultHeroList;
    public Dictionary<string, DefaultHero> defaultHeroDictionary;

    public void FillDictionary()
    {
        if (defaultHeroList == null)
            return;
        defaultHeroDictionary = new Dictionary<string, DefaultHero>();
        foreach (var hero in defaultHeroList)
        {
            if(!defaultHeroDictionary.ContainsKey(hero.heroId))
                defaultHeroDictionary.Add(hero.heroId, hero);
        }
    }
}

[System.Serializable]
public class DefaultHero
{
    public string heroId;
    public string description;
    public string race;
    public string job;
    public int rarity;
    public string texture;

    public int pDef;
    public int pMin;
    public int pDefPot;
    public int pMaxPot;

    public int mDef;
    public int mMin;
    public int mDefPot;
    public int mMaxPot;

    public int sDef;
    public int sMin;
    public int sDefPot;
    public int sMaxPot;

    public string nodeBuff;
    public string nodeDebuff;
    public string pathAff;
}

public enum HeroRace
{
    Human,
    Bird,
    Lizardfolk
}

public enum HeroJob
{
    Fighter,
    Mage,
    Student
}

public enum HeroStatus
{
    Idle,
    Trading,
    Exploring
}

public enum MapNode
{
    Triangle,
    Star,
    Rectangle
}

public enum PathType
{
    Yellow,
    Black,
    Red
}

[System.Serializable]
public struct Stat
{
    //gets fetched from default Hero lookup
    public float defaultVal;
    public float minVal;
    public float maxVal;
    public float defaultPot;
    public float maxPot;
    
    //changes overtime
    public float currVal;
    public float currPot;
}
