using UnityEngine;

//이 스크립트를 넣으면 자동으로 Enemy도 같이 추가
[RequireComponent (typeof(Enemy))]
public class EnemyMovement : MonoBehaviour
{
    public Transform[] wayPoints;
    private int currentWayPoint = 0;
    private Enemy enemyBase;

    private void Awake()
    {
        enemyBase = GetComponent<Enemy>();
    }

    //오브젝트 폴링으로 재사용할때 초기화
    private void OnEnable()
    {
        currentWayPoint = 0;
    }

    private void Update()
    {
        if (wayPoints == null || wayPoints.Length == 0) return;

        Transform target = wayPoints[currentWayPoint];
        Vector3 dir = (target.position - transform.position).normalized;

        transform.position += dir * enemyBase.MoveSpeed* Time.deltaTime;

        if(Vector3.Distance(transform.position, target.position) < 0.01f)
        {
            currentWayPoint++;

            if (currentWayPoint >= wayPoints.Length)
            {
                enemyBase.MoveEndPoint();
            }
        }
    }
}
