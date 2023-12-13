using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using UnityEngine;

namespace Utils
{
    public class UnityMainThreadDispatcher : MonoBehaviour
    {
        private readonly ConcurrentQueue<Action> _executionQueue = new ConcurrentQueue<Action>();
        public static event Action OnApplicationQuitEvent;
        public event Action OnFixedUpdate;
    
        public void FixedUpdate()
        {
            var messageCount = _executionQueue.Count;
            for (var i = 0; i < messageCount; i++)
            {
                if (_executionQueue.TryDequeue(out var action))
                    action.Invoke();
            }
            OnFixedUpdate?.Invoke();
        }


        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Adds the IEnumerator to the queue
        /// </summary>
        /// <param name="action">IEnumerator function that will be executed from the main thread.</param>
        public void Enqueue(IEnumerator action)
        {
            _executionQueue.Enqueue(() => { StartCoroutine(action); });
        }

        /// <summary>
        ///Adds the Action to the queue
        /// </summary>
        /// <param name="action">function that will be executed from the main thread.</param>
        public void Enqueue(Action action)
        {
            _executionQueue.Enqueue(action);
        }

        /// <summary>
        /// Adds the Action to the queue, returning a Task which is completed when the action completes
        /// </summary>
        /// <param name="action">function that will be executed from the main thread.</param>
        /// <returns>A Task that can be awaited until the action completes</returns>
        public Task EnqueueAsync(Action action)
        {
            var tcs = new TaskCompletionSource<bool>();

            void WrappedAction()
            {
                try
                {
                    action();
                    tcs.TrySetResult(true);
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            }

            Enqueue(ActionWrapper(WrappedAction));
            return tcs.Task;
        }


        IEnumerator ActionWrapper(Action a)
        {
            a();
            yield return null;
        }

        private void OnApplicationQuit()
        {
            OnApplicationQuitEvent?.Invoke();
        }
    }
}