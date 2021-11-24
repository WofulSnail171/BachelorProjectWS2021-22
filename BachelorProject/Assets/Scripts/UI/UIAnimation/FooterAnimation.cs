using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FooterAnimation : AbstractElementAnimation
{
    [SerializeField] float animSpeed = 2;
    [SerializeField] LeanTweenType animationType;

    private float original;

    public override float HideObject()
    {
        StopAllCoroutines();

        original = gameObject.GetComponent<RectTransform>().rect.y;
        float removed = -gameObject.GetComponent<RectTransform>().rect.height *2;


        LeanTween.moveY(gameObject, removed, animSpeed).setEase(animationType);

        StartCoroutine(Pause());

        return animSpeed;
    }

    public override void ShowObject()
    {
        StopAllCoroutines();

        float original = gameObject.GetComponent<RectTransform>().rect.y;

        gameObject.transform.position = new Vector3(gameObject.transform.position.x, - gameObject.GetComponent<RectTransform>().rect.height*2, gameObject.transform.position.z);
        gameObject.SetActive(true);

        LeanTween.moveY(gameObject, original, animSpeed).setEase(animationType);
    }

    IEnumerator Pause()
    {
        yield return new WaitForSeconds(animSpeed +1);


        gameObject.SetActive(false);
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, original, gameObject.transform.position.z);
    }
}
