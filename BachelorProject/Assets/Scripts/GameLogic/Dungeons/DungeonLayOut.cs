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

    public void SetupDungeon()
    {
        //set dungeon nodes
        DungeonEvent thisDungeonEvent = null;
        nodes = new List<DungeonNode>();
        SetupNode(startNode);
    }

    public void SetupNode(DungeonNode _node)
    {
        if (nodes.Contains(_node))
            return;
        nodes.Add(_node);


        foreach (var nextNode in _node.nextNodes)
        {

        }
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        
        Gizmos.color = Color.green;
        if(startNode != null)
            Gizmos.DrawSphere(startNode.transform.position, .5f);
        Gizmos.color = Color.red;
        if (endNode != null)
            Gizmos.DrawSphere(endNode.transform.position, .5f);

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
