using Game;
using UnityEngine;
using Microlight.MicroBar;
using System.Collections.Generic;

public class Character : HitableObject {
    
    protected static readonly int IsWalking = Animator.StringToHash("IsWalking");
    protected static readonly int Vertical = Animator.StringToHash("Vertical");
    
    [SerializeField] private CharacterAnimator m_CharacterAnimator;
    [SerializeField] private Animator m_SpriteAnimator;
    
    [SerializeField] public bool isInWater;
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float currentHealth;
    [SerializeField] protected float attack;
    [SerializeField] MicroBar _hpBar;


    private List<Damage> attackHistory = new List<Damage>();

    private void Start()
    {
        if (_hpBar != null) _hpBar.Initialize(maxHealth);
    }

    protected void SetVelocity(Vector2 velocity) {
        m_Rigidbody.velocity = velocity;
        m_SpriteAnimator.SetBool(IsWalking, velocity != Vector2.zero);
        m_SpriteAnimator.SetFloat(Vertical, velocity.y);
        transform.localScale = velocity.x < 0 ? new Vector3(-1f, 1f, 1f) : new Vector3(1f, 1f, 1f);
    }
    
    protected void MakeBasicAttack(AttackBase attackPrefab, Vector2 target) {
        if (actionsLocked) {
            return;
        }
        actionsLocked = true;
        var handle = AttackManager.Instance.MakeAttack(this, attackPrefab, target);
        handle.OnComplete += MakeBasicAttackComplete;
        handle.OnRemoveLock += UnlockActions;
    }

    private void UnlockActions() {
        actionsLocked = false;
    }

    private void MakeBasicAttackComplete(AttackHandle handle) {
        handle.OnRemoveLock -= UnlockActions;
        handle.OnComplete -= MakeBasicAttackComplete;
    }
    
    public void Hit(Damage damage) {
        m_CharacterAnimator.PlayHit();

        currentHealth -= damage.Value;
        if (currentHealth < 0) currentHealth = 0;

        if (_hpBar != null) _hpBar.UpdateHealthBar(currentHealth);

        attackHistory.Add(damage);

        if (currentHealth <= 0)
        {
            Die();
        }

    }

    private void Die()
    {
        foreach (Damage damage in attackHistory)
        {
            Debug.Log($"Attack: {damage.Type}, Damage: {damage.Value}");
        }
        Debug.Log($"{this} is now dead");
    }

    public override void attakMe(Damage attackDamage){
        Hit(attackDamage);
    }


    
}
