using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HappyTroll
{
    public class ObjectPool : MonoBehaviour
    {
        [SerializeField] private GameObject poolObject;
        [SerializeField] private Transform unusedParent;

        private List<GameObject> _activeObjects;
        private List<GameObject> _inactiveObjects;
        private int _poolSize;

        private void Awake()
        {
            _poolSize = 0;
        }

        public void Initialize(int targetSize)
        {
            if (_activeObjects != null && _inactiveObjects != null) return;

            _activeObjects = new List<GameObject>();
            _inactiveObjects = new List<GameObject>();

            for (var i = 0; i < targetSize; i++)
            {
                var obj = Instantiate(poolObject, unusedParent);
                obj.name = $"{poolObject.name} - {_poolSize}";
                obj.GetComponent<IPoolObject>().Disable();
                _inactiveObjects.Add(obj);
                _poolSize++;
            }
        }

        public GameObject Get()
        {
            GameObject obj;
            if (_inactiveObjects.Count > 0)
            {
                obj = _inactiveObjects[0];
                _inactiveObjects.Remove(obj);
            }
            else
            {
                obj = Instantiate(poolObject);
                obj.name = $"{poolObject.name} - {_poolSize}";
                _poolSize++;
            }

            _activeObjects.Add(obj);
            obj.GetComponent<IPoolObject>().Enable();
            return obj;
        }

        public void Release(GameObject poolElement)
        {
            if (_activeObjects.Contains(poolElement))
            {
                _activeObjects.Remove(poolElement);
                _inactiveObjects.Add(poolElement);
                poolElement.transform.parent = unusedParent;
                poolElement.transform.localPosition = Vector3.zero;
            }
            else
            {
                Debug.LogError($"Attempting to release an object that doesn't belong to the pool. PoolObject: {poolObject.name} - Attempted: {poolElement.name}");
            }
        }
    }
}
