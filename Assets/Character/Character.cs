using Game;
using UnityEngine;

public class Character : MonoBehaviour {
    
    [SerializeField] private CharacterAnimator m_CharacterAnimator;
    
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float currentHealth;
    [SerializeField] protected float attack;
    
    public void Hit() {
        m_CharacterAnimator.PlayHit();
    }
    
}
