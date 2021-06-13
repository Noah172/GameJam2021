using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    public Button Action;
    public Button Character;
    public Button Stats;
    public Button Options;
    public Button Skip;

    public Canvas Option;
    public Canvas Actions;
    public Canvas Info;
    public Canvas Stat;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void clickAct()
    {
        Actions.gameObject.SetActive(true);
    }

    public void clickChar()
    {
        Info.gameObject.SetActive(true);
    }

    public void clickStat()
    {
        Stat.gameObject.SetActive(true);
    }

    public void clickOpt()
    {
        Option.gameObject.SetActive(true);
    }

    public void clickSkip()
    {

    }
}
