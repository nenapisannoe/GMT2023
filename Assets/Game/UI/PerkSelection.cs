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

    public bool[] unlockedPerks = new bool[11];
    private int lastUnlocked = 10;


    // Start is called before the first frame update
    private void Start()
    {
        ShowPerk();
    }

    public void UnlockNewPerk(int inp)
    {
        unlockedPerks[inp]= true;
        lastUnlocked = inp;
        skillName.text = perks[inp].title;
        skillDescr1.text = perks[inp].descr1;


    }

    public void ShowPerk()
    {
        for (int i = 0; i < perks.Length; i++)
        {
            perks[i].skillButton.gameObject.SetActive(unlockedPerks[i]);
            perks[i].bigSkillButton.gameObject.SetActive(false);
        }
        perks[lastUnlocked].bigSkillButton.gameObject.SetActive(true);
        perks[lastUnlocked].bigSkillButton.WhiteOut();

        MainButtonClicked(lastUnlocked);

    }

    void OnEnable()
    {
        /*
        skillIcon.sprite = defIcon;
        skillName.text = "";
        skillDescr1.text = "";
        skillDescr2.text = "";
        t = 0.666f;
        */
    }



    // Update is called once per frame
    void Update()
    {

    }


    public void ButtonClicked(int inp)
    {
        //Debug.Log(inp);
        //Debug.Log($"Length: {perks.Length}");

        if (lastPressed >= 0)
        {
            perks[lastPressed].skillButton.UnSelect();
        }

        lastPressed = inp;

        for (int i = 0; i < perks.Length; i++)
        {
            //Debug.Log($"i: {i}");
            perks[i].skillButton.GrayOut();
            perks[i].bigSkillButton.GrayOut();

        }
        perks[inp].skillButton.WhiteOut();





        //skillIcon.sprite = perks[inp].sprite;
        skillName.text = perks[inp].title;
        skillDescr1.text = perks[inp].descr1;
        //skillDescr2.text = perks[inp].descr2;


    }

    public void MainButtonClicked(int inp)
    {
        ButtonClicked(inp);
        perks[inp].skillButton.GrayOut();
        perks[inp].bigSkillButton.WhiteOut();
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
    public gray_out bigSkillButton;



}