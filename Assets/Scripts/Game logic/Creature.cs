using UnityEngine;

public class Creature : MonoBehaviour
{
    public bool isAlive = true;
    public bool canAttack = true;

    public int maxHealth;
    public int curHealth;

    public int attackDamage;
    public float attackRange;
    public float attackSpeed;
    public float animatorSpeed = 1;

    public SpriteRenderer model;
    public Animator animator;
    public ParticleSystem hurtParticle;

    public virtual void TakeDamage(int damage)
    {
        if (!isAlive) return;

        animator.Play("Hurt", 1);

        hurtParticle.Clear();
        hurtParticle.Play();

        curHealth -= damage;

        if (curHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        isAlive = false;

        animator.Play("Death");
    }
}