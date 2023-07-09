using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game {
    
    public abstract class HitableObject: MonoBehaviour {
        
        [SerializeField] protected Rigidbody2D m_Rigidbody;
        protected bool actionsLocked;
        protected bool isStunned;
        
        public abstract void attakMe(Damage attackDamage); // Envoke when object is attaked

        public async void Knockback(Transform from) {
            actionsLocked = true;
            m_Rigidbody.velocity = (transform.position - from.position).normalized * 10f;
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            actionsLocked = false;
        }
        
    }
    
}
