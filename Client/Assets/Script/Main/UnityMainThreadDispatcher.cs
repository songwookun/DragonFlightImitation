using System;
using System.Collections.Generic;
using UnityEngine;

public class UnityMainThreadDispatcher : MonoBehaviour
{
    private static readonly Queue<Action> _executionQueue = new Queue<Action>();

    public static UnityMainThreadDispatcher Instance()
    {
        if (_instance == null)
        {
            var obj = new GameObject("MainThreadDispatcher");
            _instance = obj.AddComponent<UnityMainThreadDispatcher>();
            DontDestroyOnLoad(obj);
        }
        return _instance;
    }

    private static UnityMainThreadDispatcher _instance;

    public void Enqueue(Action action)
    {
        lock (_executionQueue)
        {
            _executionQueue.Enqueue(action);
        }
    }

    void Update()
    {
        if (_executionQueue.Count > 0)
            Debug.Log("[Dispatcher] Update 실행됨. 큐 처리 중...");

        while (_executionQueue.Count > 0)
        {
            _executionQueue.Dequeue()?.Invoke();
        }
    }
}
