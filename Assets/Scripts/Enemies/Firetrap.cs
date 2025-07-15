using UnityEngine;
using System.Collections;

public class Firetrap : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float damageInterval = 0.5f; // Интервал между уроном

    [Header("Firetrap Timers")]
    [SerializeField] private float activationDelay;
    [SerializeField] private float activeTime;

    private Animator anim;
    private SpriteRenderer spriteRend;
    private bool triggered;
    private bool active;
    private Health playerHealth; // Кэшируем здоровье игрока

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerHealth = collision.GetComponent<Health>(); // Запоминаем игрока
            if (!triggered)
                StartCoroutine(ActivateFiretrap());
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (active && collision.CompareTag("Player"))
        {
            StartCoroutine(ApplyDamageOverTime());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerHealth = null; // Очищаем ссылку при выходе
        }
    }

    private IEnumerator ApplyDamageOverTime()
    {
        while (active && playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
            yield return new WaitForSeconds(damageInterval);
        }
    }

    private IEnumerator ActivateFiretrap()
    {
        triggered = true;
        spriteRend.color = Color.red;

        yield return new WaitForSeconds(activationDelay);
        spriteRend.color = Color.white;
        active = true;
        anim.SetBool("activated", true);

        yield return new WaitForSeconds(activeTime);
        active = false;
        triggered = false;
        anim.SetBool("activated", false);
    }
}