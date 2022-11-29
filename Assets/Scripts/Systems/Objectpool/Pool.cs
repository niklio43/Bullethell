using UnityEngine;
using System;
using System.Collections.Generic;

namespace BulletHell.ObjectPool
{
    public class Pool<T> where T : IPoolable
    {
        List<T> members = new List<T>();
        List<T> unavailable = new List<T>();

        Func<T> CreateFunction;
        int maxAmount;

        public Pool(Func<T> CreateFunction, int maxAmount = 10)
        {
            this.CreateFunction = CreateFunction;
            this.maxAmount = maxAmount;
        }

        public void Populate(int amount)
        {
            if(amount <= 0) { return; }
            for (int i = 0; i < amount; i++) {
                Create();
            }
        }

        T Create()
        {
            T newItem = CreateFunction();
            members.Add(newItem);

            return newItem;
        }

        public T Get()
        {
            for (int i = 0; i < members.Count; i++) {
                if(unavailable.Contains(members[i])) { continue; }
                unavailable.Add(members[i]);
                return members[i];
            }

            if(members.Count < maxAmount) {
                T newItem = Create();
                unavailable.Add(newItem);
                return newItem;
            }

            T oldestItem = unavailable[0];
            oldestItem.ResetObject();
            unavailable.Add(oldestItem);

            return oldestItem;
        }

        public void Release(T item)
        {
            if(!unavailable.Contains(item)) { return; }
            unavailable.Remove(item);
        }
    }
}