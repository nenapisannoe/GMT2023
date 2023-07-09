using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game {
    public class ExplosiveBarrelController :HitableObject
    {
        public AttackBase explosionPerfab;
        public int explosionDamge;
        public override void attakMe(Damage attackDamage){
            Debug.Log(attackDamage.Type);
            if (attackDamage.Type is DamageType.Fire){
                Debug.Log("I Exploded");
                var attack = Instantiate(explosionPerfab);
                attack.transform.position = new Vector2(transform.position.x, transform.position.y);
			    attack.InitAttack(new Damage {
				    Type = DamageType.Fire,
				    Value = explosionDamge
			    });
                Destroy(gameObject, 0.5f);
            }
            
        }
    }
}
