
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    private Animator anim;
    private PlayerMovement playerMovement;
    private float cooldownTimer=Mathf.Infinity;  //cooldownTimer ,infinity it makes available initially otherwise value is 0

    private void Awake()
    {
        anim= GetComponent<Animator>();
        playerMovement= GetComponent<PlayerMovement>();
    }
    private void Update()
    {
        //read by mouse left button and cooldownTimer is greater
        if (Input.GetMouseButton(0) && cooldownTimer > attackCooldown && playerMovement.canAttack())
            Attack();

        cooldownTimer += Time.deltaTime;
    }

    private void Attack()
    {
        anim.SetTrigger("attack");
        cooldownTimer = 0;
    }
}
