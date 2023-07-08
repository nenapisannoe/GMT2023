using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game {
    public class ChestContriller :HitableObject
    {
        public override void attakMe(Damage attackDamage){
            Debug.Log("I Exploded");
        }

    }
}
