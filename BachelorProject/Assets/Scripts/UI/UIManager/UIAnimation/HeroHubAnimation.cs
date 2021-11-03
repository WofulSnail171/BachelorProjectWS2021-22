using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroHubAnimation : MonoBehaviour
{
    [SerializeField] float animSpeed;

    IEnumerator hide()
    {
        yield return null;
    }

    public void Show()
    {
        float original = gameObject.transform.position.y;

        gameObject.transform.position = new Vector3(gameObject.transform.position.x, -Screen.height , gameObject.transform.position.z);
        gameObject.SetActive(true);

        LeanTween.moveY(gameObject, original, animSpeed).setEase(LeanTweenType.easeInOutQuad);
        //yield return null;
    }
}