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
    public float maxHealth;
    public float currentHealth;
    [SerializeField] protected float attack;
    [SerializeField] protected MicroBar _hpBar;


    public void Awake(){
        base.Awake();
    }

    private void Start()
    {
        if (_hpBar != null) _hpBar.Initialize(maxHealth);
    }

    public void OnEnable(){
        base.OnEnable();
        currentHealth = maxHealth;
        if (_hpBar != null) _hpBar.Initialize(maxHealth);            
    }

    public void SetVelocity(Vector2 velocity) {
        moveVector = velocity;
        m_SpriteAnimator.SetBool(IsWalking, velocity != Vector2.zero);
        m_SpriteAnimator.SetFloat(Vertical, velocity.y);
        transform.localScale = velocity.x < 0 ? new Vector3(-1f, 1f, 1f) : new Vector3(1f, 1f, 1f);
    }

    public void SetCharge(bool value) {
        gameObject.layer = value ? 9 : 8;
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

    private void MakeBasicAttackComplete(AttackHandle handle) {
        handle.OnRemoveLock -= UnlockActions;
        handle.OnComplete -= MakeBasicAttackComplete;
    }

    public void Heal(float healValue){
        currentHealth += healValue;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        if (_hpBar != null) _hpBar.UpdateHealthBar(currentHealth);
    }
    
    public virtual void Hit(Damage damage) {
        m_CharacterAnimator.PlayHit();

        currentHealth -= damage.Value;
        if (currentHealth < 0) currentHealth = 0;

        if (_hpBar != null) _hpBar.UpdateHealthBar(currentHealth);
        

        if (currentHealth <= 0)
        {
            Die();
        }

    }

    protected virtual void Die()
    {
        /*foreach (Damage damage in attackHistory)
        {
            Debug.Log($"Attack: {damage.Type}, Damage: {damage.Value}");
        }*/
        Main.instance.resetLevel(-1);	
        Debug.Log($"{this} is now dead");
    }

    public override void attakMe(Damage attackDamage){
        Hit(attackDamage);
    }


    
}
