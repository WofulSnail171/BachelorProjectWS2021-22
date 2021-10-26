using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardSlot : MonoBehaviour,IDragHandler,IBeginDragHandler,IEndDragHandler
{
    [SerializeField] private GameObject card;
    [SerializeField] private float speed;
    [SerializeField] private GameObject scroll;

    Vector3 originalPosition;



    //drag funcs
    //------------------------------------------------------------------------
    public void OnDrag(PointerEventData pointerEventData)
    {
        if (Input.touchCount == 1)
        {
            card.transform.position = Input.mousePosition;
            card.transform.position = Vector3.MoveTowards(card.transform.position, Input.mousePosition, speed * Time.deltaTime);
        }
    }

    public void OnBeginDrag(PointerEventData pointerEventData)
    {
        if (Input.touchCount == 1)
        {
            scroll.GetComponent<ScrollRect>().vertical = false;
        }

        originalPosition = card.transform.position;
    }

    public void OnEndDrag(PointerEventData pointerEventData)
    {
        StartCoroutine(ResetDrag());
    }


    //coroutine
    IEnumerator ResetDrag()
    {
        card.transform.position = originalPosition;
        yield return new WaitForSeconds(0.1f);
        scroll.GetComponent<ScrollRect>().vertical = true;
    }
}
