using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DungeonNode : MonoBehaviour
{
    //relevant for creating layoutPrefab
    public GameObject[] nextNodes;

    //Relevant for runtime logic
    public string[] nextPaths;
    public string nodeType;

    public string statType;
    public string eventName;
    public int maxEventHealth;
    public int eventHealth;
    public int defaultGrowth;
    public int currentGrowth;

    public Vector3 PathPosition(int _pathIndex)
    {
        Vector3 result = Vector3.zero;
        if(nextNodes.Length > _pathIndex && nextPaths.Length > _pathIndex)
        {
            result = transform.position + 0.5f * (nextNodes[_pathIndex].transform.position - transform.position);
        }
        return result;
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Handles.Label(transform.position, eventName);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, .3f);
        // Draws a blue line from this transform to the target
        if(nextNodes != null)
        {
            foreach (var item in nextNodes)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(transform.position, item.transform.position);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (nextNodes != null)
        {
            foreach (var item in nextNodes)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(transform.position + 0.5f * (item.transform.position - transform.position), .7f);
                Gizmos.DrawSphere(transform.position + 0.55f * (item.transform.position - transform.position), .5f);
                Gizmos.DrawSphere(transform.position + 0.60f * (item.transform.position - transform.position), .4f);
                Gizmos.DrawCube(transform.position + 0.90f * (item.transform.position - transform.position), new Vector3(.4f, .4f, .4f));
            }
        }
    }
}
