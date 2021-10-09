using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Event_Deck", menuName = "Event_System/Event_Deck", order = 1)]
public class Event_Deck : ScriptableObject
{

    [SerializeField] private List<Card> _cards_of_this_Deck = new List<Card>();



    public Card Draw_Card()
    {
        Debug.Log("card drawn");
        return null;
    }

    private void Initialize_Deck()
    {
        Shuffel_Deck();

        foreach(Card card in _cards_of_this_Deck)
        {
            Debug.Log(card.name);
        }
    }

    private void Shuffel_Deck()
    {
        // fisher–yates shuffle     
        for (int i = 0; i < _cards_of_this_Deck.Count; i++)
        {

            // Pick random Element
            int j = Random.Range(i, _cards_of_this_Deck.Count);

            // Swap Elements
            Card temp = _cards_of_this_Deck[i];
            _cards_of_this_Deck[i] = _cards_of_this_Deck[j];
            _cards_of_this_Deck[j] = temp;
        }
    }

    private void OnEnable()
    {
        Initialize_Deck();
    }
}