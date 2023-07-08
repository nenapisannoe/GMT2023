using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game {
    public abstract class HitableObject: MonoBehaviour
    {
        public abstract void attakMe(Damage attackDamage); // Envoke when object is attaked
    }
}
