using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;



public class PerkSelection : MonoBehaviour
{

    public Perk[] perks;
    public int perkPerLvl = 2;
    public gray_out[] skillButton;
    public TextMeshProUGUI skillName;
    public TextMeshProUGUI skillDescr1;
    public TextMeshProUGUI skillDescr2;
    public Image skillIcon;
    public Image progressBar;
    public Sprite defIcon;
    public float currentExp = 0.25f;
    public int lvl = 1;
    public float[] expThreshold;
    public string[] lvlDescr;
    public TextMeshProUGUI lvlName;
    public int lastPressed = -1;
    public float t;

    public Image levelInd;
    public Sprite[] LevelIndSpr;

    // Start is called before the first frame update
    private void Start()
    {
        foreach (Perk perk in perks)
        {
            if (perk.skillObj!=null)
            {
                perk.skillButton = perk.skillObj.GetComponent<gray_out>();
                perk.sprite = perk.skillObj.GetComponent<Image>().sprite;
                
            }
        }
    }

    

    void OnEnable()
    {
        skillIcon.sprite = defIcon;
        skillName.text = "";
        skillDescr1.text = "";
        skillDescr2.text = "";
        t = 0.666f;
        
    }



    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        if (t>=0.66f)
        {

            t = 0;
            if (lastPressed>-1)
            {
                //perks[lastPressed].effectObject.RefreshDescr();
                skillName.text = perks[lastPressed].title;
                skillDescr1.text = perks[lastPressed].descr1;
                skillDescr2.text = perks[lastPressed].descr2;
            }
        }
    }

    public void ExpChange(float inp)
    {
        currentExp += inp;
        CalcLevel();
    }

    private void CalcLevel()
    {
        int i = 0;
        while (i < expThreshold.Length && expThreshold[i] <= currentExp)
        {
            i++;
        }
        lvl = i;
        levelInd.sprite = LevelIndSpr[lvl];
        progressBar.fillAmount = currentExp;
        for (i = 0; i < lvl * perkPerLvl; i++)
        {
            if (!perks[i].isEmpty)
            {
                perks[i].skillButton.GrayOutGentle();
            }
        }

        // dark out all rest
        for (i = lvl * perkPerLvl; i < perks.Length; i++)
        {
            if (!perks[i].isEmpty)
            {
                perks[i].skillButton.DarkOutGentle();
            }
        }
        //mb name goes here also
        lvlName.text = lvlDescr[lvl];
    }

    public void ButtonClicked(int inp)
    {
        if (inp < lvl * perkPerLvl)
        {

            if (lastPressed >=0)
            {
                perks[lastPressed].skillButton.UnSelect();
            }

            lastPressed = inp;

            // mb do this only once


            perks[inp].isActive = true;

            int btm = (inp / perkPerLvl) * perkPerLvl;
            int top = btm + perkPerLvl - 1;

            for (int i = btm; i<=top; i++)
            {
                if (i != inp)
                {
                    if (!perks[i].isEmpty)
                    {
                        perks[i].skillButton.GrayOut();
                        perks[i].isActive = false;
                    }
                }
            }
            perks[inp].skillButton.WhiteOut();





            //skillIcon.sprite = perks[inp].sprite;
            skillName.text = perks[inp].title;
            skillDescr1.text = perks[inp].descr1;
            //skillDescr2.text = perks[inp].descr2;


        }
    }

    public void MainButtonClicked(int inp)
    {
        ButtonClicked(inp);

    }

    //public void 

}



[System.Serializable]
public class Perk
{
    public string title;
    public string descr1;
    public string descr2;
    public GameObject skillObj;

    public bool isActive = false;
    public bool isEmpty = false;
    public Sprite sprite;
    public gray_out skillButton;



}