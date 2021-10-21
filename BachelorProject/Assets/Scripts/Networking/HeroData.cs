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
}

[System.Serializable]
public class IncomingHeroData
{
    public DefaultHero[] defaultHeroList;
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

    public MapNode nodeBuff;
    public MapNode nodeDebuff;
    public PathType pathAff;
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
