using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMotor : MonoBehaviour
{
    // Scripts playerInventory
    PlayerInventory playerInv;
    
    // Animations du perso
    private Animator animator;
    
    // Vitesse de déplacement
    public float walkSpeed;
    public float runSpeed;
    public float turnSpeed;

    // Variables concernant l'attaque 1
    public float attackCooldown1;
    private bool isAttacking1;
    private float currentCooldown1;

    // Variables concernant l'attaque 2
    public float attackCooldown2;
    private bool isAttacking2;
    private float currentCooldown2;

    public float attackRange;

    public GameObject rayHit;

    // Inputs
    public string inputFront;
    public string inputBack;
    public string inputLeft;
    public string inputRight;

    public Vector3 jumpSpeed;
    CapsuleCollider playerCollider;
    
    // Le personnage est-il mort ?
    public bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        playerCollider = gameObject.GetComponent<CapsuleCollider>();
        playerInv = gameObject.GetComponent<PlayerInventory>();
        rayHit = GameObject.Find("RayHit");
    }

    // On vérifie si le personnage est bien au sol
    bool IsGrounded()
    {
        // return Physics.CheckCapsule(playerCollider.bounds.center, new Vector3(playerCollider.bounds.center.x, playerCollider.bounds.min.y - 0.1f, playerCollider.bounds.center.z), 0.84f);
        Vector3 dwn = transform.TransformDirection(Vector3.down);

        return (Physics.Raycast(transform.position, dwn, 0.1f));
    }


    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            // Si le personnage avance
            if (Input.GetKey(inputFront) && !Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(inputBack))
            {
                transform.Translate(0, 0, walkSpeed * Time.deltaTime);
                animator.SetBool("IsWalking", true);
                animator.SetBool("IsRunning", false);
            }

            // Si le personnage court
            if (Input.GetKey(inputFront) && Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(inputBack))
            {
                transform.Translate(0, 0, runSpeed * Time.deltaTime);
                animator.SetBool("IsRunning", true);
            }

            // Si le personnage recule
            if (Input.GetKey(inputBack) && !Input.GetKey(inputFront))
            {
                transform.Translate(0, 0, -(walkSpeed / 2) * Time.deltaTime);
                animator.SetBool("IsWalking", true);
                animator.SetBool("IsRunning", false);
            }

            // Si le personnage tourne à gauche
            if (Input.GetKey(inputLeft))
            {
                transform.Rotate(0, -turnSpeed * Time.deltaTime, 0);
            }

            // Si le personnage tourne à droite
            if (Input.GetKey(inputRight))
            {
                transform.Rotate(0, turnSpeed * Time.deltaTime, 0);
            }

            // Si le personnage n'avance pas et ne recule pas non plus
            if (!Input.GetKey(inputFront) && !Input.GetKey(inputBack))
            {
                animator.SetBool("IsWalking", false);
                animator.SetBool("IsRunning", false);
            }

            // Si le personnage tente d'avancer et de reculer en même temps (appuyer sur inputFront et inputBack en même temps)
            if (Input.GetKey(inputFront) && Input.GetKey(inputBack))
            {
                animator.SetBool("IsWalking", false);
                animator.SetBool("IsRunning", false);
            }

            // Si on saute
            if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
            {
                // Préparation du saut
                Vector3 playerVelocity = gameObject.GetComponent<Rigidbody>().velocity;
                playerVelocity.y = jumpSpeed.y;

                // Saut
                gameObject.GetComponent<Rigidbody>().velocity = jumpSpeed;
            }

            // Si on attaque en étant à l'arrêt ou en marchant
            if (!(Input.GetKey(inputFront) && Input.GetKey(KeyCode.LeftShift)) && Input.GetKeyDown(KeyCode.Mouse0))
            {
                Attack1();
            }

            if (isAttacking1)
            {
                currentCooldown1 -= Time.deltaTime;
            }
            if (currentCooldown1 <= 0)
            {
                currentCooldown1 = attackCooldown1;
                isAttacking1 = false;
                animator.SetBool("IsAttacking1", false);
            }

            // Si on attaque en courant
            if (Input.GetKey(inputFront) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Mouse0))
            {
                Attack2();
            }

            if (isAttacking2)
            {
                currentCooldown2 -= Time.deltaTime;
            }
            if (currentCooldown2 <= 0)
            {
                currentCooldown2 = attackCooldown2;
                isAttacking2 = false;
                animator.SetBool("IsAttacking2", false);
            }
        }
    }

    // Fonction d'attaque 1
    public void Attack1()
    {
        if (!isAttacking1)
        {
            animator.SetBool("IsAttacking1", true);
            AttackManagement();
        }

        isAttacking1 = true;
    }

    // Fonction d'attaque 2
    public void Attack2()
    {
        if (!isAttacking2)
        {
            animator.SetBool("IsAttacking2", true);
            AttackManagement();
        }

        isAttacking2 = true;
    }

    // Fonction permettant de gérer l'attaque du personnage (portée de l'attaque, visualisation du raycast,
    // application de dommage si l'élément frappé est un ennemi)
    public void AttackManagement()
    {
        RaycastHit hit;

        if (Physics.Raycast(rayHit.transform.position, transform.TransformDirection(Vector3.forward), out hit, attackRange))
        {
            Debug.DrawLine(rayHit.transform.position, hit.point, Color.red);

            if (hit.transform.tag == "Enemy")
            {
                hit.transform.GetComponent<enemyAI>().ApplyDamage(playerInv.currentDamage);
            }
        }
    }
}
