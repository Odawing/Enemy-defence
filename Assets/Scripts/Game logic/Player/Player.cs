using System.Collections;
using System.Linq;
using UnityEngine;

public class Player : Creature
{
    public int ultimateCharge = 0;
    public int maxUltimateCharge = 10;

    private bool xFliped = false;
    private int curAttackStreak;

    private void Start()
    {
        animator.speed = animatorSpeed;
    }

    public void PerformAttack(bool xFlip)
    {
        if (!canAttack) return;

        var animToPlay = GetAttackAnimationName(xFlip);

        if (xFliped != xFlip)
        {
            transform.right = -transform.right;
        }

        xFliped = xFlip;

        animator.Play(animToPlay);

        StartCoroutine(AttackCour());
    }

    private IEnumerator AttackCour()
    {
        canAttack = false;

        yield return new WaitForSeconds(attackSpeed);

        // Raycast to check near enemies to hit
        var hitColls = new RaycastHit[5];
        var hits = Physics.RaycastNonAlloc(new Ray(transform.position, transform.right), hitColls, attackRange);

        for (int i = 0; i < hits; i++)
        {
            var enemy = hitColls[i].collider.GetComponent<Enemy>();
            if (enemy)
            {
                // Multiply damage by streak
                var finalDamage = attackDamage;
                if (curAttackStreak == 2) finalDamage *= 2;
                if (curAttackStreak == 4) finalDamage *= 3;

                enemy.TakeDamage(finalDamage);
            }
        }

        canAttack = true;
    }

    private string GetAttackAnimationName(bool xFlip)
    {
        // Check if attacking one side in streak
        if (xFliped == xFlip)
        {
            curAttackStreak++;
        }
        else curAttackStreak = 0;

        // Return anim to play
        string animToPlay;
        if (curAttackStreak == 2)
        {
            animToPlay = "AttackInStreak1";
        }
        else if (curAttackStreak == 4)
        {
            animToPlay = "AttackInStreak2";
            curAttackStreak = 0;
        }
        else
        {
            animToPlay = "Attack";
        }

        return animToPlay;
    }

    public void UseUltimate()
    {
        if (!canAttack || ultimateCharge != maxUltimateCharge) return;

        canAttack = false;

        animator.Play("Ult");

        StartCoroutine(UltimateCour());
    }

    private IEnumerator UltimateCour()
    {
        yield return new WaitForSeconds(0.25F);

        foreach (var enemy in GameManagerScr.Instance.allEnemies.ToList())
        {
            if (enemy && enemy.isAlive)
            {
                enemy.TakeDamage(25);
            }
        }

        ultimateCharge = 0;

        canAttack = true;

        GameInterface.Instance.RefreshUltimateBar();
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        GameInterface.Instance.RefreshHPBar();
    }

    public override void Die()
    {
        base.Die();

        Invoke(nameof(EndOfGame), 2F);
    }

    private void EndOfGame()
    {
        GameInterface.Instance.ShowEndGameMenu();
    }
}