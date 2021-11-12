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

    public void ApplyGrowth(int _pGrowth, int _mGrowth, int _sGrowth, DungeonType dType = DungeonType.basic)
    {
        switch (dType)
        {
            case DungeonType.basic:
                _pGrowth = (int)((float)_pGrowth * 2.0f);
                _mGrowth = (int)((float)_pGrowth * 2.0f);
                _sGrowth = (int)((float)_pGrowth * 2.0f);
                break;
            case DungeonType.doom:
                _pGrowth = (int)((float)_pGrowth * 3.0f);
                _mGrowth = (int)((float)_pGrowth * 3.0f);
                _sGrowth = (int)((float)_pGrowth * 3.0f);
                break;
            default:
                break;
        }
        if (_pGrowth < -10)
            _pGrowth = -10;
        if (_mGrowth < -10)
            _mGrowth = -10;
        if (_sGrowth < -10)
            _sGrowth = -10;
        pVal += _pGrowth;
        if(pVal >= pPot)
        {
            pVal = pPot;
        }
        else if(pVal <= DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[heroId].pMin)
        {
            pVal = DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[heroId].pMin;
        }
        mVal += _mGrowth;
        if (mVal >= mPot)
        {
            mVal = mPot;
        }
        else if (mVal <= DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[heroId].mMin)
        {
            mVal = DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[heroId].mMin;
        }
        sVal += _sGrowth;
        if (sVal >= sPot)
        {
            sVal = sPot;
        }
        else if (sVal <= DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[heroId].sMin)
        {
            sVal = DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[heroId].sMin;
        }
    }
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
