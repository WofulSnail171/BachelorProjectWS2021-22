using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEnablerManager : MonoBehaviour
{
    public enum ListReference
    {
        Footer,
        Header,
        Centered,
        PopUp
    }


    #region vars
    public static UIEnablerManager Instance;

    //dictionaries
    Dictionary<string,GameObject> FooterElements = new Dictionary<string, GameObject>();
    Dictionary<string, GameObject> HeaderElements = new Dictionary<string, GameObject>();
    Dictionary<string, GameObject> CenterElements = new Dictionary<string, GameObject>();
    Dictionary<string, GameObject> PopUpElements = new Dictionary<string, GameObject>();
    #endregion


    //Singleton
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        else
            Destroy(this);
    }


    //public funcs to be used 
    //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //init
    public void AssignElementToList(ListReference reference, GameObject UIelement)
    {
        string name = UIelement.name;
        int index = name.LastIndexOf("_"); // Character to remove "_"
        if (index > 0)
            name = name.Substring(0, index);

        switch (reference)
        {
            case ListReference.Header:
                HeaderElements.Add(name,UIelement);
                break;
            case ListReference.Footer:
                FooterElements.Add(name,UIelement);
                break;
            case ListReference.Centered:
                CenterElements.Add(name,UIelement);
                break;
            case ListReference.PopUp:
                PopUpElements.Add(name,UIelement);
                break;
            default:
                Debug.Log("trying to assign none existant element");
                break;
        }
    }


    //during runtime to be called
    public void EnableElement(string element)
    {
        bool IsSomewhere = false;

        //differemt enable logics for dictionary type
        //find if in Dictionaries there is something to be enabled
        //only do if not already enabled

        if (FooterElements.ContainsKey(element))
            IsSomewhere = EnableFooter(FooterElements[element]);

        if (HeaderElements.ContainsKey(element))
            IsSomewhere = EnableHeader(HeaderElements[element]);

        if (CenterElements.ContainsKey(element))
            IsSomewhere = EnableCentered(CenterElements[element]);

        if (PopUpElements.ContainsKey(element))
            IsSomewhere = EnablePopUp(PopUpElements[element]);

        if (!IsSomewhere)
            Debug.Log("trying to enable: " + element + ", but this does not exist in any dict as key");
    }

    public void DisableElement(string element)
    {
        bool IsSomewhere = false;//catch bool

        //differemt enable logics for dictionary type
        //find if in Dictionaries there is something to be enabled
        //only do if not already disabled

        if (FooterElements.ContainsKey(element))
            IsSomewhere = DisableFooter(FooterElements[element]);

        if (HeaderElements.ContainsKey(element))
            IsSomewhere = DisableHeader(HeaderElements[element]);

        if (CenterElements.ContainsKey(element))
            IsSomewhere = DisableCentered(CenterElements[element]);

        if (PopUpElements.ContainsKey(element))
            IsSomewhere = DisablePopUp(PopUpElements[element]);

        if (!IsSomewhere)
            Debug.Log("trying to disable: " + element + ", but this does not exist in any dict as key");
    }

    public void SwitchElements(string oldElement,string newElement)
    {
        //mix of enable and disable
        //wait for disable before do enable (at least for certain objects) or do smooth transition
    }



    //concrete logics (maybe these should be coroutines)
    //------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //enables

    #region enabler funcs
    private bool EnableFooter(GameObject element)
    {
        return true;
    }

    private bool EnableHeader(GameObject element)
    {
        return true;
    }

    private bool EnableCentered(GameObject element)
    {
        return true;
    }

    private bool EnablePopUp(GameObject element)
    {
        return true;
    }
    #endregion


    //disables

    #region disabler funcs
    private bool DisableFooter(GameObject element)
    {
        element.SetActive(false);
        return true;
    }

    private bool DisableHeader(GameObject element)
    {
        element.SetActive(false);
        return true;
    }

    private bool DisableCentered(GameObject element)
    {
        element.SetActive(false);
        return true;
    }

    private bool DisablePopUp(GameObject element)
    {
        element.SetActive(false);
        return true;
    }
    #endregion
}
