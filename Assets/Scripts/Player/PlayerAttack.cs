using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireballs;

    private Animator anim;
    private PlayerMovement playerMovement;
    private float cooldownTimer = Mathf.Infinity;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && cooldownTimer > attackCooldown && playerMovement.canAttack()) //Проверка кулдауна и возможности атаки
            Attack();
        cooldownTimer += Time.deltaTime; // обновление таймера
    }

    private void Attack()
    {
        anim.SetTrigger("attack");
        cooldownTimer = 0;

        fireballs[FindFireball()].transform.position = firePoint.position; // Использует FindFireball для поиска свободного фаербола и устанавливает его на позицию?
        fireballs[FindFireball()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x)); //направление выстрела
    }
    private int FindFireball()
    {
        for (int i = 0; i < fireballs.Length; i++) // Length?(условие продолжения поиска)
        {
            if (!fireballs[i].activeInHierarchy) // activeInHierarchy - свойтсво GameObject которое возвращает false если объект выключен
                return i;
        }
        return 0;
    }

}