using UnityEngine;
using System;
using System.Collections.Generic;

namespace BulletHell
{
    public class Pool<T> where T : class, IPoolable
    {
        public List<T> members = new List<T>();
        //TODO: Turn this into a list holding indices instead.
        public List<T> active = new List<T>();

        Func<T> CreateFunction;
        readonly int maxAmount;

        public Pool(Func<T> CreateFunction, int maxAmount = 10)
        {
            if (CreateFunction == null) throw new ArgumentNullException("createFunction");

            this.CreateFunction = CreateFunction;
            this.maxAmount = maxAmount;
        }

        public T Get()
        {
            T item = null;

            for (int i = 0; i < members.Count; i++) {
                if(active.Contains(members[i])) { continue; }
                item = members[i];
            }

            if(item == null) {
                if (members.Count < maxAmount) {
                    item = Create();
                }
                else {
                    item = active[0];
                    item.ResetObject();
                }
            }

            active.Add(item);
            return item;
        }
        public void Release(T item)
        {
            if(!active.Contains(item)) { return; }
            active.Remove(item);
        }
        public void Dispose()
        {
            foreach (T item in members) {
                item.Dispose();
            }

            members.Clear();
            active.Clear();
        }
        private T Create()
        {
            T newItem = CreateFunction();
            members.Add(newItem);

            return newItem;
        }
    }
}