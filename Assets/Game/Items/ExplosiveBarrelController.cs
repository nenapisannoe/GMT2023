using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game {
    public class ExplosiveBarrelController :HitableObject
    {
        public AttackBase explosionPerfab;
        public int explosionDamge;

        public void Start(){
            explosionPerfab.damage = explosionDamge;
        }
        public override void attakMe(Damage attackDamage){
            if ((attackDamage.Type is DamageType.Fire) || (attackDamage.Type is DamageType.Contaminating)){
                Debug.Log("I Exploded");
                AttackManager.Instance.MakeAttack(this, explosionPerfab, this.transform.position);
                Destroy(gameObject, 0.5f);
            }
            
        }
    }
}
