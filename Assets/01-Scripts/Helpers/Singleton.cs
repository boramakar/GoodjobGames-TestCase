using UnityEngine;

namespace HappyTroll
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance == null)
                    {
                        var obj = new GameObject();
                        _instance = obj.AddComponent<T>();
                        DontDestroyOnLoad(obj);
                    }
                }

                return _instance;
            }
        }
    }
}