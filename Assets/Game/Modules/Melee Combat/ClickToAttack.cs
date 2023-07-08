using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToAttack : MonoBehaviour
{
    [SerializeField] private GameObject character;
    [SerializeField] private GameObject attackPrefab;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 v3Pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            v3Pos = Camera.main.ScreenToWorldPoint(v3Pos);

            var characterPos = character.transform.localPosition;
            //var posX = characterPos.x;
            v3Pos -= characterPos;

           /*  if (v3Pos.x > 0.0f)
                 posX += 3f;
             else
                 posX -= 3f;*/
            
             Instantiate(attackPrefab, new Vector3(0f,0f,0f)/*new Vector3(Mathf.Clamp(v3Pos.x, characterPos.x - 3f, characterPos.x + 3f), v3Pos.y, 0f)*/, Quaternion.identity); 
            Debug.Log(v3Pos);


        }
    }
    
}
