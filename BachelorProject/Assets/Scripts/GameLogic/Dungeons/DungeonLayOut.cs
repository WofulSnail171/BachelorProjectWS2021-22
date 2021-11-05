using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonLayOut : MonoBehaviour
{
    public DungeonNode startNode;
    public DungeonNode endNode;
    public DungeonType dungeonType;

    //not to fill out
    public List<DungeonNode> nodes;

    public void SetupDungeonDailySeed(int _seed)
    {
        UnityEngine.Random.InitState(_seed);
        //set dungeon nodes
        nodes = new List<DungeonNode>();
        SetupNodeDailySeed(startNode);
        DontDestroyOnLoad(this);
        UnityEngine.Random.InitState((int)DateTime.Now.Ticks);
    }

    public void SetupDungeonRunSeed(int _seed)
    {
        UnityEngine.Random.InitState(_seed);
        nodes = new List<DungeonNode>();
        SetupNodeRunSeed(startNode);
        UnityEngine.Random.InitState((int)DateTime.Now.Ticks);
    }

    public void ResetNodes()
    {
        foreach (var item in nodes)
        {
            item.eventHealth = item.maxEventHealth;
            item.currentGrowth = item.defaultGrowth;
            item.chosenPathIndex = -1;
            //maybe i also need to reset hero information in the future
        }
    }

    public void SetupNodeDailySeed(DungeonNode _node)
    {
        if (nodes.Contains(_node))
            return;
        nodes.Add(_node);
        _node.nodeType = DatabaseManager._instance.eventData.nodeTypes[UnityEngine.Random.Range(0, DatabaseManager._instance.eventData.nodeTypes.Length)];


        for (int i = 0; i < _node.nextNodes.Length; i++)
        {
            _node.nextPaths.Add(DatabaseManager._instance.eventData.pathTypes[UnityEngine.Random.Range(0, DatabaseManager._instance.eventData.pathTypes.Length)]);
            SetupNodeDailySeed(_node.nextNodes[i]);
        }
    }

    public void SetupNodeRunSeed(DungeonNode _node)
    {
        if (nodes.Contains(_node))
            return;
        nodes.Add(_node);
        //_node.statType = DatabaseManager._instance.eventData.eventDecks.[UnityEngine.Random.Range(0, DatabaseManager._instance.eventData.nodeTypes.Length)];
        _node.nodeEvent  = DatabaseManager._instance.eventData.eventDecks[DatabaseManager._instance.eventData.GetNodeTypeIndex(_node.nodeType)].deck[UnityEngine.Random.Range(0, DatabaseManager._instance.eventData.eventDecks[DatabaseManager._instance.eventData.GetNodeTypeIndex(_node.nodeType)].deck.Length)];

        _node.maxEventHealth = UnityEngine.Random.Range(10, 20);
        _node.eventHealth = _node.maxEventHealth;
        _node.defaultGrowth = UnityEngine.Random.Range(10, 20);
        _node.currentGrowth = _node.defaultGrowth;
        for (int i = 0; i < _node.nextNodes.Length; i++)
        {
            SetupNodeRunSeed(_node.nextNodes[i]);
        }
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        
        Gizmos.color = Color.green;
        if(startNode != null)
            Gizmos.DrawSphere(startNode.transform.position, .3f);
        Gizmos.color = Color.red;
        if (endNode != null)
            Gizmos.DrawSphere(endNode.transform.position, .3f);

        // Draws a blue line from this transform to the target

    }
}

//create and recreate a dungeon run:
//seed for randomization
//name of the dungeon layout -> to use the right prefab
//step count -> can be used to recreate any point of time ine this dungeon run

//two runs of dungeon creation + randomisation
//1: choosing different dungeon layouts and applying nodetypes to them (Seed 1)
//2: (needs set party information and starting time) assigns actual events to each nodetype based on event list (also event health), then calculates all steps until end of dungeon. Important: decision makings are relative to seed 2
