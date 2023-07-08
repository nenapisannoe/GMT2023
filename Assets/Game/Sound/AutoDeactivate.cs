using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDeactivate : MonoBehaviour
{

    public float sec = 4f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LateCall());
    }


    IEnumerator LateCall()
    {

        yield return new WaitForSeconds(sec);
        Destroy(gameObject);
    }

}
