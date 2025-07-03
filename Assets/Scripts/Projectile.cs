using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    private float direction;
    private bool hit;
    private float lifetime;

    private Animator anim;
    private BoxCollider2D boxCollider;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    private void Update()
    {
        if (hit) return; // при поподании выходим из метода
        float movementSpeed = speed * Time.deltaTime * direction; // расчет скорости
        transform.Translate(movementSpeed, 0, 0); // перемещение объекта

        lifetime += Time.deltaTime; // увеличение счетчик таймера жизни
        if (lifetime > 5) gameObject.SetActive(false); // отключение через 5 секунд
    }
    private void OnTriggerEnter2D(Collider2D collision) // срабатывает при попдании по другому колайдеру
    {
        hit = true;
        boxCollider.enabled = false;
        anim.SetTrigger("explode");
    }
    public void SetDirection(float _direction) //?
    {
        lifetime = 0; // обнуление счетчика
        direction = _direction; //?
        gameObject.SetActive(true); // активирует снаряд
        hit = false; // сбрасывает флаг попадания
        boxCollider.enabled = true; // включает колайдер

        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != _direction) // ? определяет текучение положение спрайта
            localScaleX = -localScaleX; // инвертирует если не совпадает с направлением движения

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }
    private void Deactivate()
    {
        gameObject.SetActive(false); // деактивация фаербола
    }
}