using UnityEngine;

public class HPBarFollower : MonoBehaviour
{
    [SerializeField] private Vector3 offset = new Vector3(0, 1.5f, 0);
    private Transform target;

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    private void LateUpdate()
    {
        if (target != null && target.gameObject.activeSelf)
        {
            transform.position = target.position + offset;
        }
    }
}
