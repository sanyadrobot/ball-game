using System;
using UnityEngine;

namespace Mono
{
    public class TriggerController : MonoBehaviour
    {
        public event Action OnTrigger;

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                OnTrigger?.Invoke();
            }
        }
    }
}
