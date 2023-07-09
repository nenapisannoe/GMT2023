using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gray_out : MonoBehaviour {

    public int test;
    public GameObject border_off = null;
    public GameObject border_on = null;
    public GameObject border_close = null;
    public GameObject border_selected = null;
    public int status = -1; // 0 dark; 1 gray; 2 white


    // Use this for initialization
    void Start () {
        //border_on?.SetActive(false);
        border_off?.SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Set_gray(float inp)
    {
        GetComponent<Image>().color = new Vector4(inp, inp, inp,1);
    }


    public void GrayOut()
    {
        //Debug.Log("Gray Out");
        Set_gray(0.5f);
        border_off?.SetActive(true);
        //border_on?.SetActive(false);
        //border_close?.SetActive(false);

        status = 1;
    }

    public void GrayOutGentle()
    {
        if (status == 2) return;
        GrayOut();
    }

    public void DarkOutGentle()
    {
        if (status >= 1) return;
        DarkOut();
    }

    public void DarkOut()
    {
        Set_gray(0.2f);
        border_off?.SetActive(false);
        //border_on?.SetActive(false);
        //border_close?.SetActive(true);
        status = 0;
    }

    public void WhiteOut()
    {
        Set_gray(1);
        border_off?.SetActive(false);
        //border_on?.SetActive(true);
        //border_close?.SetActive(false);
        //border_selected?.SetActive(true);
        status = 2;
    }

    public void UnSelect()
    {
        //border_selected?.SetActive(false);
    }


}
