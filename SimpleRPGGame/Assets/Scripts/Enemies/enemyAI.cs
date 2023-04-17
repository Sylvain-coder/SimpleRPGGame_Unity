using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAI : MonoBehaviour
{
    // Distance entre le joueur et l'ennemi
    private float Distance;

    // Cible de l'ennemi
    public Transform Target;

    // Distance de déclenchement de l'animation IdleBattle
    public float idlebattleRange;

    // Distance de poursuite
    public float chaseRange;

    // Portée des attaques
    public float attackRange;

    // Cooldown des attaques
    public float attackRepeatTime;
    private float attackTime;

    // Montant des dégâts infligés
    public float DamageDealt;

    // Agent de navigation
    private UnityEngine.AI.NavMeshAgent agent;

    // Animator de l'ennemi
    private Animator animator;

    // Vie de l'ennemi
    public float enemyHealth;

    // L'ennemi est-il mort ?
    private bool isDead = false;


    // Start is called before the first frame update
    void Start()
    {
        agent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = gameObject.GetComponent<Animator>();
        attackTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            // On cherche le joueur en permanence
            Target = GameObject.Find("Player").transform;

            // On calcule la distance entre le joueur et l'ennemi, 
            // en fonction de cette distance, on effectue diverses actions
            Distance = Vector3.Distance(Target.position, transform.position);

            // Quand l'ennemi est loin = idlenormal
            if (Distance > idlebattleRange) {
                IdleNormal();
            }

            // Quand l'ennemi est plutôt proche et qu'il ne poursuit pas le joueur
            if (Distance < idlebattleRange && Distance > chaseRange) {
                IdleBattle();
            }

            // Quand l'ennemi est proche mais pas assez pour attaquer
            if (Distance < chaseRange && Distance > attackRange)
            {
                Chase();
            }

            // Quand l'ennemi est assez proche pour attaquer
            if (Distance < attackRange)
            {
                Attack();
            }
        }
    }

    // Attente
    void IdleNormal()
    {
        animator.SetBool("IsIdlingBattle", false);
    }

    // Prêt à se battre
    void IdleBattle()
    {
        animator.SetBool("IsIdlingBattle", true);
        animator.SetBool("IsChasing", false);
    }

    // Poursuite
    void Chase()
    {
        animator.SetBool("IsChasing", true);
        animator.SetBool("IsAttacking", false);
        agent.destination = Target.position;
    }

    // Combat
    void Attack()
    {
        // Empêche l'ennemi de traverser le joueur
        agent.destination = transform.position;

        // Si pas de cooldown
        if (Time.time > attackTime) {
            animator.SetBool("IsAttacking", true);
            Target.GetComponent<PlayerInventory>().ApplyDamage(DamageDealt);
            Debug.Log(gameObject.name + " a infligé " + DamageDealt + " points de dégâts au joueur.");
            attackTime = Time.time + attackRepeatTime;
        }
    }    

    public void ApplyDamage(float DamageTaken)
    {
        if (!isDead)
        {
            enemyHealth = enemyHealth - DamageTaken;
            print(gameObject.name + " a subi " + DamageTaken + " points de dégâts par le joueur.");

            if (enemyHealth <= 0)
            {
                Dead();
            }
        }
    }

    public void Dead()
    {
        isDead = true;
        animator.SetBool("IsDying", true);
        Destroy(transform.gameObject, 7);
    }
}
