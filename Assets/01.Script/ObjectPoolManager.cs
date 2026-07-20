using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager instance;

    [System.Serializable]
    public struct CanvasPoolItem
    {
        public GameObject prefab;
        public Transform targetCanvas;
    }
    //기본 오브젝트
    [SerializeField] private List<GameObject> objList = new List<GameObject>();
    //캔버스 전용 오브젝트 풀 적 HPUI
    [SerializeField] private List<CanvasPoolItem> canvasPools = new List<CanvasPoolItem>();
    Dictionary<string, Queue<GameObject>> pools = new Dictionary<string, Queue<GameObject>>();

    Dictionary<string, Transform> poolParents = new Dictionary<string, Transform>();

    int poolSize;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        poolSize = 5;

        foreach (GameObject obj in objList)
        {
            pools[obj.name] = new Queue<GameObject>();

            GameObject parentPool = new GameObject($"{obj.name}_Pool");
            parentPool.transform.SetParent(this.transform);

            SetupPool(obj.name, obj, parentPool.transform, poolSize);
        }

        foreach(var item in canvasPools)
        {
            SetupPool(item.prefab.name, item.prefab, item.targetCanvas, poolSize);
        }
    }

    private void SetupPool(string name, GameObject prefab, Transform parent, int size)
    {
        pools[name] = new Queue<GameObject>();
        poolParents[name] = parent;

        for (int i = 0; i < poolSize; i++)
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
            go.name = prefab.name;
            go.SetActive(true);
            return go;
        }
    }

    private GameObject GetPrefabFromList(string name)
    {
        // 일반 리스트에서 찾기
        GameObject obj = objList.Find(x => x.name == name);
        if (obj != null) return obj;

        // 캔버스 리스트에서 찾기
        CanvasPoolItem item = canvasPools.Find(x => x.prefab.name == name);
        return item.prefab;
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