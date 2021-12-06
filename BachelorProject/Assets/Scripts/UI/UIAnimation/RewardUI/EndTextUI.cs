using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndTextUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI endText;
    [SerializeField] private GameObject continueButton;

    private void OnEnable()
    {
        DungeonEvent quest = DatabaseManager._instance.eventData.basicQuestDict[DatabaseManager._instance.dungeonData.currentRun.dungeon.questName];
        endText.text = quest.endText;

        endText.transform.position = new Vector3(endText.transform.position.x, -endText.preferredHeight, 0);

        continueButton.SetActive(false);

        StartCoroutine(AnimateText());
    }

    IEnumerator AnimateText()
    {
        float animspeed = endText.preferredHeight * 0.1f;

        LeanTween.value(endText.gameObject, -endText.preferredHeight,0,animspeed)
            .setOnUpdate(setY) ;

        yield return new WaitForSeconds(animspeed);

        continueButton.SetActive(true);
    }

    private void setY(float yVal)
    {
        endText.transform.position = new Vector3(endText.transform.position.x, yVal, 0);
    }
}
