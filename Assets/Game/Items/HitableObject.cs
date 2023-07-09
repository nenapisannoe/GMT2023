using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game {
    
    public abstract class HitableObject: MonoBehaviour {
        
        [SerializeField] private Rigidbody2D m_Rigidbody;
        protected bool actionsLocked;

        protected Vector2 moveVector = Vector2.zero;
        protected Vector2 forceVector = Vector2.zero;

        protected bool isKnockable = true;
        
        public abstract void attakMe(Damage attackDamage); // Envoke when object is attaked
        
        protected void UnlockActions() {
            actionsLocked = false;
        }

        public async void Knockback(Transform from) {
            if (!isKnockable) {
                return;
            }
            actionsLocked = true;
            forceVector = (transform.position - from.position).normalized * 10f;
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            forceVector = Vector2.zero;
            UnlockActions();
        }

        private void FixedUpdate() {
            if (m_Rigidbody != null) {
                m_Rigidbody.velocity = forceVector != Vector2.zero ? forceVector : moveVector;
            }
        }

    }
    
}
