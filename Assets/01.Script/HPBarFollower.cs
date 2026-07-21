using UnityEngine;
using UnityEngine.UI;

public class HPBarFollower : MonoBehaviour
{
    [SerializeField] private Vector3 offset = new Vector3(0, 1.5f, 0);
    private Transform target;

    private Slider slider;

    private void OnEnable()
    {
        slider = GetComponent<Slider>();
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public Slider GetSlider()
    {
        return slider;
    }

    private void LateUpdate()
    {
        if (target != null && target.gameObject.activeSelf)
        {
            transform.position = target.position + offset;
        }
    }
}
