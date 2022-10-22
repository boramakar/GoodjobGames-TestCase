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

        public void Initialize(int targetSize)
        {
            if (_activeObjects != null && _inactiveObjects != null) return;

            _activeObjects = new List<GameObject>();
            _inactiveObjects = new List<GameObject>();

            for (var i = 0; i < targetSize; i++)
            {
                var obj = Instantiate(poolObject, unusedParent);
                obj.GetComponent<IPoolObject>().Disable();
                _inactiveObjects.Add(obj);
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
