using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// The <c>RecyclingObjectPool</c> class.
/// The main pool to manages all pooled objects
/// </summary>
public class RecyclingObjectPool : MonoBehaviour
{

    //Original objects
    private Dictionary<string, GameObject> _originalDict = new Dictionary<string, GameObject>();

    //Pool of generated objects
    private Dictionary<string, List<GameObject>> _pool = new Dictionary<string, List<GameObject>>();

    private void Awake()
    {
        //Load all prefabs
        List<GameObject> prefabList = new List<GameObject>(Resources.LoadAll<GameObject>(ResourcePath.PREFAB_RECYCLING_OBJECT));

        for (int i = 0; i < prefabList.Count; i++)
        {
            //Create originals
            GameObject original = Instantiate(prefabList[i]);
            original.transform.parent = transform;
            original.name = prefabList[i].name;
            original.transform.position = Vector3.zero;

            _originalDict[original.name] = original;
            _pool[original.name] = new List<GameObject>();

            //Always hide original first
            original.SetActive(false);
        }
    }

    //=================================================================================
    //Mapping Object
    //=================================================================================

    /// <summary>
    /// Get Object from Pool by name
    /// </summary>
    public GameObject Get(string objectName)
    {
        GameObject target = null;

        if (string.IsNullOrEmpty(objectName))
        {
            Debug.LogError("Object's name should not be null or empty");
        }
        else if (!_pool.ContainsKey(objectName))
        {
            Debug.LogError(objectName + "Can not find any object named :" + objectName + ".");
            return null;
        }

        //Recycling object if it exists
        if (_pool[objectName].Count > 0)
        {
            target = _pool[objectName][0];
            _pool[objectName].Remove(target);
        }
        else // create new
        {
            target = Instantiate(_originalDict[objectName]);
            target.name = objectName;
            target.transform.parent = transform;
        }

        //init object
        if (target.GetComponent<RecyclingObject>() != null)
        {
            target.GetComponent<RecyclingObject>().Init();
        }

        return target;
    }

    /// <summary>
    /// Release object
    /// </summary>
    public void Release(GameObject target)
    {
        //release
        if (target.GetComponent<RecyclingObject>() != null)
        {
            target.GetComponent<RecyclingObject>().Release();
        }

        //store it in pool
        target.transform.parent = transform;
        _pool[target.name].Add(target);
    }

}