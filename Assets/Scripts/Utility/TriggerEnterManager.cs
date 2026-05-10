using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace BoatGame
{
    public class TriggerEnterManager<T> where T : MonoBehaviour
    {
        private readonly Dictionary<T, int> triggerCounts = new();
        public List<T> All { get; set; } = new();
        
        /// <summary>
        /// Returns true if this entity is entering the trigger for the first time.
        /// </summary>
        public bool OnTriggerEntered(T entity)
        {
            if (triggerCounts.TryGetValue(entity, out var count))
            {
                count++;
                triggerCounts[entity] = count;
                return false;
            }
            
            triggerCounts.Add(entity, 1);
            All.Add(entity);
            return true;
        }

        /// <summary>
        /// Returns true if the entity has exited all tracked triggers.
        /// </summary>
        public bool OnTriggerExited(T entity)
        {
            if (triggerCounts.TryGetValue(entity, out var count))
            {
                count--;

                if (count == 0)
                {
                    triggerCounts.Remove(entity);
                    All.RemoveSwapBack(entity);
                    return true;
                }

                if (count < 0)
                    Debug.LogError($"Entity exited a trigger and its count was below zero at {count}", entity);
                else triggerCounts[entity] = count;
                
                return false;
            }
            
            Debug.LogError("Entity exited a trigger but was not tracked in the trigger count dictionary", entity);
            return false;
        }
    }
}
