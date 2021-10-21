using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HeroData
{
    public string heroID;
    public string description;
    public string race;
    public string job;

    public int rarity;
    public string texture;
    
    public HeroStatus status;

    public Stat physical;
    public Stat magical;
    public Stat social;

    public MapNode buff;
    public MapNode debuff;

    public PathType pathAffinity;
}

[System.Serializable]
public class IncomingHeroData
{
    public DefaultHero[] defaultHeroList;
}

[System.Serializable]
public class DefaultHero
{
    public string heroid;
    public string description;
    public string race;
    public string job;
    public int rarity;
    public string texture;

    public int pdef;
    public int pmin;
    public int pdefpot;
    public int pmaxpot;
           
    public int mdef;
    public int mmin;
    public int mdefpot;
    public int mmaxpot;
           
    public int sdef;
    public int smin;
    public int sdefpot;
    public int smaxpot;

    public MapNode nodebuff;
    public MapNode nodedebuff;
    public PathType pathaff;
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
