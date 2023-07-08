using Game;
using UnityEngine;

public class Character : MonoBehaviour {
    
    protected static readonly int IsWalking = Animator.StringToHash("IsWalking");
    protected static readonly int Vertical = Animator.StringToHash("Vertical");
    
    [SerializeField] private CharacterAnimator m_CharacterAnimator;
    [SerializeField] private Rigidbody2D m_Rigidbody;
    [SerializeField] private Animator m_SpriteAnimator;
    
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float currentHealth;
    [SerializeField] protected float attack;
    
    protected bool actionsLocked;
    
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
        var handle = AttackManager.Instance.MakeAttack(attackPrefab, target, transform.position);
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
        Debug.LogWarning($"Got hit: {damage.Value}");
    }
    
}
