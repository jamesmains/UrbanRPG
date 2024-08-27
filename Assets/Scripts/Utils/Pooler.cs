using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class Pooler : MonoBehaviour {
    public static Pooler Instance;
    
    [ListDrawerSettings(ShowIndexLabels = true, ListElementLabelName = "pooledObject")] [SerializeField] [ReadOnly]
    private List<PooledObject> pooledObjects;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else {
            Debug.LogError("Found duplicate Pooler");
            Destroy(this.gameObject);
        }
        
        foreach (var pObj in pooledObjects)
            StartCoroutine(SpawnObjects(pObj, true));
    }

    public static GameObject SpawnObject(GameObject targetObject, Vector3 spawnLocation) {
        return Instance.DoSpawnObject(targetObject, spawnLocation);
    }
    
    private GameObject DoSpawnObject(GameObject targetObject, Vector3 spawnLocation) {
        
        if (targetObject == null) {
            print($"NOTE: Tried to spawn a non-existent object! Oops!");
            return null;
        }
        
        var pool = pooledObjects.FirstOrDefault(o => o.pooledObject.name == targetObject.name);
        var obj = pool?.spawnedObjects.FirstOrDefault(o => o.activeSelf == false);
        
        if (pool == null) {
            
            var newPool = new PooledObject {
                poolContent = Instantiate(new GameObject(), this.transform).transform,
                pooledObject = targetObject,
                initialAmount = 3,
                increaseBoundsAmount = 3
            };
            newPool.poolContent.name = targetObject.name;
            print($"NOTE: Had to create a new pool for {targetObject.name}");
            pooledObjects.Add(newPool);
            StartCoroutine(SpawnObjects(newPool));
            return SpawnObject(targetObject, spawnLocation);
        }

        if (obj == null) {
            if (pool.increaseBoundsAmount <= 0) return null;
            StartCoroutine(SpawnObjects(pool));
            return SpawnObject(targetObject, spawnLocation);
        }
        
        obj.transform.position = spawnLocation;
        obj.SetActive(true);
        return obj;
    }

    private IEnumerator SpawnObjects(PooledObject poolObj, bool init = false) {
        var amountToSpawn = init ? poolObj.initialAmount : poolObj.increaseBoundsAmount;
        while (amountToSpawn > 0) {
            var obj = Instantiate(poolObj.GetPooledObject(), poolObj.poolContent);
            obj.name = poolObj.GetPooledObject().name;
            poolObj.spawnedObjects.Add(obj);
            obj.SetActive(false);
            amountToSpawn--;
        }

        yield return new WaitForEndOfFrame();
    }
#if UNITY_EDITOR
    [Button]
    private void SortPooledObjects() {
        pooledObjects = pooledObjects.OrderBy(o => o.pooledObject.name).ToList();
    }
#endif
}

[Serializable]
public class PooledObject {
    public Transform poolContent;
    public GameObject pooledObject;
    public int initialAmount;
    public int increaseBoundsAmount;
    public List<GameObject> spawnedObjects = new();

    public GameObject GetPooledObject() {
        pooledObject.SetActive(false);
        return pooledObject;
    }
}