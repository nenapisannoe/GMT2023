using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game {
    public class ExplosiveBarrelController :HitableObject
    {
        public AttackBase explosionPerfab;
        public int explosionDamge;
        public override void attakMe(Damage attackDamage){
            if (attackDamage.Type is DamageType.BossAbility2){
                Debug.Log("I Exploded");
                var attack = Instantiate(explosionPerfab);
                attack.transform.position = new Vector2(transform.position.x, transform.position.y);
			    attack.InitAttack(new Damage {
				    Type = DamageType.BarrelExplosion,
				    Value = explosionDamge
			    });
                Destroy(gameObject, 0.5f);
            }
            
        }
    }
}
