using System.Collections.Generic;
using UnityEngine;

public enum PoolType
{
    None,
    Archer_T1, Archer_T2, Archer_T3,
    Wizard_T1, Wizard_T2, Wizard_T3,
    Arrow,
    WizardBullet,
    NormalEnemy,
    SpecialEnemy,
    SlimeKing,
    GameOverMonster,


    //Canvas 전용
    EnemyHPBar
}

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager instance;

    [System.Serializable]
    public struct CanvasPoolItem
    {
        public PoolType type;
        public GameObject prefab;
        public Transform targetCanvas;
        public int poolSize;
    }


    [System.Serializable]
    public struct ObjectPoolItem
    {
        public PoolType type;
        public GameObject prefab;
        public int poolSize;
    }

    //기본 오브젝트
    //[SerializeField] private List<GameObject> objList = new List<GameObject>();
    [SerializeField] private List<ObjectPoolItem> objList = new List<ObjectPoolItem>();
    //캔버스 전용 오브젝트 풀 적 HPUI
    [SerializeField] private List<CanvasPoolItem> canvasPools = new List<CanvasPoolItem>();

    private Dictionary<PoolType, Queue<GameObject>> pools = new Dictionary<PoolType, Queue<GameObject>>();
    private Dictionary<PoolType, Transform> poolParents = new Dictionary<PoolType, Transform>();
    private Dictionary<PoolType, GameObject> prefabDict = new Dictionary<PoolType, GameObject>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        foreach (var item in objList)
        {
            if(item.type==PoolType.None || item.prefab==null) continue;

            prefabDict[item.type] = item.prefab;

            pools[item.type] = new Queue<GameObject>();

            GameObject parentPool = new GameObject($"{item.type}_Pool");
            parentPool.transform.SetParent(this.transform);

            SetupPool(item.type, item.prefab, parentPool.transform, item.poolSize);
        }

        foreach (var item in canvasPools)
        {
            if (item.type == PoolType.None || item.prefab == null) continue;

            prefabDict[item.type] = item.prefab;
            pools[item.type] = new Queue<GameObject>();

            SetupPool(item.type, item.prefab, item.targetCanvas, item.poolSize);
        }
    }

    private void SetupPool(PoolType type, GameObject prefab, Transform parent, int size)
    {
        poolParents[type] = parent;

        for (int i = 0; i < size; i++)
        {
            GameObject go = Instantiate(prefab, parent);
            go.name = type.ToString();
            go.SetActive(false);
            pools[type].Enqueue(go);
        }
    }

    public GameObject GetObject(PoolType type)
    {
        if (!pools.ContainsKey(type))
        {
            return null;
        }

        if (pools[type].Count > 0)
        {
            GameObject go = pools[type].Dequeue();
            go.SetActive(true);
            return go;
        }
        else
        {
            GameObject prefab = GetPrefabFromList(type);
            if (prefab != null) return null;

            GameObject go = Instantiate(prefab, poolParents[type]);
            go.name = type.ToString();
            go.SetActive(true);
            return go;
        }
    }

    private GameObject GetPrefabFromList(PoolType type)
    {
        //// 일반 리스트에서 찾기
        //// 
        //ObjectPoolItem op = objList.Find(x => x.prefab.name == name);
        //if (op.prefab != null) return op.prefab;

        //// 캔버스 리스트에서 찾기
        //CanvasPoolItem item = canvasPools.Find(x => x.prefab.name == name);
        //if (item.prefab != null) return item.prefab;

        if (prefabDict.TryGetValue(type, out GameObject prefab))
        {
            return prefab;
        }

        return null;
    }

    public void ReturnObject(PoolType type, GameObject go)
    {
        if (!pools.ContainsKey(type))
        {
            Destroy(go);
            return;
        }
        go.SetActive(false);
        pools[type].Enqueue(go);
    }

}