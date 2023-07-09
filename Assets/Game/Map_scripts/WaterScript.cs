using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterScript : MonoBehaviour
{
    protected void OnTriggerEnter2D(Collider2D other) {
        var target = other.gameObject.GetComponent<Character>();
        if (target != null){
            target.isInWater = true;
        }
	}

    protected void OnTriggerExit2D(Collider2D other) {
        var target = other.gameObject.GetComponent<Character>();
        if (target != null){
            target.isInWater = false;
        }	
	}
}
