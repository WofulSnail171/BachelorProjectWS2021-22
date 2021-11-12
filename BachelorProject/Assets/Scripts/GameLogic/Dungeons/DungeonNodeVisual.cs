using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DungeonNodeVisual : MonoBehaviour
{
    DungeonNode dungeonNode;
    LineRenderer lineRenderer;
    public TMP_Text nodeType;
    public TMP_Text eventName;
    public TMP_Text eventGrowth;
    public CheapProgressBar pgBar;
    public GameObject pathVisualPrefab;
    public List<LineRenderer> pathVisualizer;

    public PathGradient[] pathGradients;
    Dictionary<string, Gradient> pathGradientDict;

    // Start is called before the first frame update
    void Awake()
    {
        if (dungeonNode == null)
            dungeonNode = gameObject.GetComponent<DungeonNode>();
        if (lineRenderer == null)
            lineRenderer = gameObject.GetComponent<LineRenderer>();
    }
    private void Start()
    {
        foreach (var node in dungeonNode.nextNodes)
        {
            GameObject go = Instantiate(pathVisualPrefab, transform.position, transform.rotation);
            LineRenderer lr = go.GetComponent<LineRenderer>();
            Vector3[] positions = new Vector3[]
            {
                dungeonNode.transform.position,
                node.transform.position
            };
            lr.positionCount = 2;
            lr.SetPositions(positions);
            pathVisualizer.Add(lr);
            go.transform.SetParent(this.transform);
        }
        pathGradientDict = new Dictionary<string, Gradient>();
        foreach (var item in pathGradients)
        {
            if (!pathGradientDict.ContainsKey(item.pathId))
            {
                pathGradientDict.Add(item.pathId, item.gradient);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateVisuals();
    }

    void UpdateVisuals()
    {
        nodeType.text = dungeonNode.nodeType;
        pgBar.SetVal(dungeonNode.eventHealth, dungeonNode.maxEventHealth);
        if (dungeonNode.eventHealth != dungeonNode.maxEventHealth)
        {
            eventName.text = dungeonNode.nodeEvent.eventName;
            eventGrowth.text = "Growth: " + dungeonNode.currentGrowth.ToString();
        }
        else
        {
            eventName.text = "???";
        }

        for (int i = 0; i < pathVisualizer.Count; i++)
        {
            pathVisualizer[i].colorGradient = pathGradientDict[dungeonNode.nextPaths[i]];
        }
        if(dungeonNode.chosenPathIndex != -1 && DungeonManager._instance.currentCalcRun != null && DungeonManager._instance.currentCalcRun.currentNode != dungeonNode)
        {
            pathVisualizer[dungeonNode.chosenPathIndex].colorGradient = pathGradientDict["cleared"];
        }
    }

    [System.Serializable]
    public struct PathGradient
    {
        public string pathId;
        public Gradient gradient;
    }
}
