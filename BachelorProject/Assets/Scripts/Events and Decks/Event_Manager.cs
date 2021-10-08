using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_Manager : MonoBehaviour
{
    [SerializeField] private List<Event_Deck> event_Decks = new List<Event_Deck>();

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            if (event_Decks.Count > 0)
            {
                Event_Deck _Deck = event_Decks[0];
                Card _Event = _Deck.Draw_Card();
            }        
        }
    }
}
