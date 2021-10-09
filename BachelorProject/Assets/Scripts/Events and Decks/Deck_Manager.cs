using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck_Manager : MonoBehaviour
{

    #region vars
    [SerializeField] private List<Event_Deck> event_Decks = new List<Event_Deck>();
    [SerializeField] private int seed;
    #endregion


    private void Start()
    {
        StartCoroutine(Test());

        //testing seeds
        Random.InitState(seed);

        int test = Random.Range(0, 10);

        Debug.Log($"the random number is {test} for {seed}");
    }


    //waiting for key input in coroutine
    IEnumerator Test()
    {
        Debug.Log("hoi");
        yield return new WaitUntil(KeyPressed);

        if (event_Decks.Count > 0)
        {
            Event_Deck _Deck = event_Decks[0];
            Card _Event = _Deck.Draw_Card();
        }

        yield return new WaitForSeconds(3f);
        StartCoroutine(Test());
    }

    bool KeyPressed()
    {
        if (Input.GetKeyDown(KeyCode.A))
            return true;

        else
            return false;
    }
}
