
using UnityEngine;


public class TowerBullet : MonoBehaviour

{
    [SerializeField] private float speed = 8f;
    [SerializeField] private float maxFlyDistance = 15f;

    private Transform target;
    private float bulletDamage;
    private Vector3 startPosition;

    //몬스터가 겹쳐있을때 데미지가 두번 들어가는것을 방지하기 위해
    private bool isReturned = false;

    public void SetTarget(Transform target, float damage)
    {
        this.target = target;
        bulletDamage = damage;
        startPosition = transform.position;

        isReturned = false;
    }

    private void Update()
    {
        if (isReturned) return;
        if (target == null || !target.gameObject.activeSelf)
        {
            ReturnToPool();
            return;
        }
        
        //총알이 타깃을 맞춰야하는데 이상한곳으로 날아가는것을 방지하기위해
        if (Vector3.Distance(startPosition, target.position) > maxFlyDistance)
        {
            ReturnToPool();
            return;
        }

        Vector3 dir = (target.position - transform.position).normalized;

        transform.position += dir * speed * Time.deltaTime;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturned) return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(bulletDamage);
            }
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        if (isReturned) return;
        isReturned = true;

        target = null;
        ObjectPoolManager.instance.ReturnObject(gameObject.name, gameObject);
    }
}