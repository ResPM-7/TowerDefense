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
    GameOverMonster
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

    private Dictionary<string, Queue<GameObject>> pools = new Dictionary<string, Queue<GameObject>>();
    private Dictionary<string, Transform> poolParents = new Dictionary<string, Transform>();
    private Dictionary<string, GameObject> prefabDict = new Dictionary<string, GameObject>();

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
            prefabDict[item.prefab.name] = item.prefab;

            pools[item.prefab.name] = new Queue<GameObject>();

            GameObject parentPool = new GameObject($"{item.prefab.name}_Pool");
            parentPool.transform.SetParent(this.transform);

            SetupPool(item.prefab.name, item.prefab, parentPool.transform, item.poolSize);
        }

        foreach (var item in canvasPools)
        {
            prefabDict[item.prefab.name] = item.prefab;

            SetupPool(item.prefab.name, item.prefab, item.targetCanvas, item.poolSize);
        }
    }

    private void SetupPool(string name, GameObject prefab, Transform parent, int size)
    {
        pools[name] = new Queue<GameObject>();
        poolParents[name] = parent;

        for (int i = 0; i < size; i++)
        {
            GameObject go = Instantiate(prefab, parent);
            go.name = name;
            go.SetActive(false);
            pools[name].Enqueue(go);
        }
    }

    public GameObject GetObject(string name)
    {
        if (!pools.ContainsKey(name))
        {
            return null;
        }

        if (pools[name].Count > 0)
        {
            GameObject go = pools[name].Dequeue();
            go.SetActive(true);
            return go;
        }
        else
        {
            GameObject prefab = GetPrefabFromList(name);
            GameObject go = Instantiate(prefab, poolParents[name]);
            go.name = name;
            go.SetActive(true);
            return go;
        }
    }

    private GameObject GetPrefabFromList(string name)
    {
        //// 일반 리스트에서 찾기
        //// 
        //ObjectPoolItem op = objList.Find(x => x.prefab.name == name);
        //if (op.prefab != null) return op.prefab;

        //// 캔버스 리스트에서 찾기
        //CanvasPoolItem item = canvasPools.Find(x => x.prefab.name == name);
        //if (item.prefab != null) return item.prefab;

        if (prefabDict.TryGetValue(name, out GameObject prefab))
        {
            return prefab;
        }

        return null;
    }

    public void ReturnObject(string name, GameObject go)
    {
        if (!pools.ContainsKey(name))
        {
            Destroy(go);
            return;
        }
        go.SetActive(false);
        pools[name].Enqueue(go);
    }

}