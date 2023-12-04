using System.Collections;
using UnityEngine;

public class Enemy : Creature
{
    public float attackInterval;

    public float moveSpeed;

    public float knockBack;

    protected Rigidbody body;

    private void Start()
    {
        body = GetComponent<Rigidbody>();

        // Flip and change forward if this enemy spawns on left
        if (transform.position.x < GameManagerScr.Instance.player.transform.position.x)
        {
            transform.right = -transform.right;
        }

        animator.speed = animatorSpeed;
    }

    private void FixedUpdate()
    {
        if (!isAlive) return;

        // Check distance for attack
        if (Vector2.Distance(GameManagerScr.Instance.player.transform.position, transform.position) > attackRange)
        {
            // move to player
            animator.SetBool("isMoving", true);

            //body.MovePosition((Vector2)transform.position + moveSpeed * Time.deltaTime * forward);
            body.velocity = moveSpeed * Time.deltaTime * -transform.right;
        }
        else
        {
            // stop and start attacking player
            animator.SetBool("isMoving", false);

            body.velocity = Vector2.zero;

            if (canAttack) StartCoroutine(AttackCour());
        }
    }

    private IEnumerator AttackCour()
    {
        canAttack = false;

        animator.Play("Attack");

        yield return new WaitForSeconds(attackSpeed);

        if (isAlive && 
            Vector2.Distance(GameManagerScr.Instance.player.transform.position, transform.position) <= attackRange)
        {
            GameManagerScr.Instance.player.TakeDamage(attackDamage);
        }

        yield return new WaitForSeconds(attackInterval);

        canAttack = true;
    }

    public override void TakeDamage(int damage)
    {
        if (!isAlive) return;

        base.TakeDamage(damage);

        body.AddForce(damage * knockBack * transform.right);
    }

    public override void Die()
    {
        base.Die();

        body.isKinematic = true;

        GameManagerScr.Instance.AddKill();

        GameManagerScr.Instance.allEnemies.Remove(this);

        Destroy(gameObject, 5F);
    }
}