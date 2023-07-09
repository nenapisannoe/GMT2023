using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game {
    public class ChestContriller :HitableObject
    {
        public void Start(){
            ChestsStorage.active_chests.Add(this);
        }

        public bool isDanger = false;

        public override void attakMe(Damage attackDamage){
            if (attackDamage.Type is DamageType.Contaminating){
                isDanger = true;
                gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
            }
        }

        public void open() {
            ChestsStorage.active_chests.Remove(this);
            Destroy(gameObject, 0.5f);
        }
    }
}
