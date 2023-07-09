using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game {
    public class ExplosiveBarrelController :HitableObject
    {
        public AttackBase explosionPerfab;
        public int explosionDamge;

        public void Awake(){
            explosionPerfab.damage = explosionDamge;
        }

        private void Start() {
            BarrelsStorage.active_barrels.Add(this);
        }

        public void onEnanle(){
            
        }


        public override void attakMe(Damage attackDamage){
            if (attackDamage.Type is DamageType.BossAbility2 or DamageType.HeroAbility2){
                Debug.Log("I Exploded");
                AttackManager.Instance.MakeAttack(this, explosionPerfab, this.transform.position);
                gameObject.SetActive(false);
                
                BarrelsStorage.active_barrels.Remove(this);
            }
            
        }
    }
}
