using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Event_System/Card", order = 0)]

public class Card : ScriptableObject
{
    [SerializeField] private string test_text;
    [SerializeField] private bool unique;
    [SerializeField] private bool endless;

    [Range (1, 20)]
    [SerializeField] private int execution_amount;

    void Execute()
    {
        Debug.Log(test_text);
    }
}

