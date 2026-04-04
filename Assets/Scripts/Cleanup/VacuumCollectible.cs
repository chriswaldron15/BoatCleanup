using System;
using UnityEngine;

namespace BoatGame
{
    public class VacuumCollectible : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;
            
            gameObject.SetActive(false);
        }
    }
}
