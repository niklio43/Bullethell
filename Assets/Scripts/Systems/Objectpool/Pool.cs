using System;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell
{
    //Class responsible for pooling and managing objects.
    public class Pool<T> where T : class, IPoolable
    {
        public List<T> members = new List<T>();

        //TODO: Turn this into a list holding indices instead.
        public List<T> active = new List<T>();

        protected Func<T> CreateFunction;

        //Max size of the pool.
        protected readonly int maxAmount;

        public Pool(Func<T> CreateFunction, int maxAmount = 10)
        {
            if (CreateFunction == null) throw new ArgumentNullException("createFunction");

            this.CreateFunction = CreateFunction;
            this.maxAmount = maxAmount;
        }

        //Returns an available object, if none exist and there's room in the pool then the function creates a new instance.
        public virtual T Get()
        {
            T item = null;

            for (int i = 0; i < members.Count; i++) {
                if (active.Contains(members[i])) { continue; }
                item = members[i];
            }

            if (item == null) {
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

        //Returns the given object to the pool.
        public virtual void Release(T item)
        {
            if (!active.Contains(item)) { return; }
            active.Remove(item);
        }

        //Destroys the pool.
        public virtual void Dispose()
        {
            members.Clear();
            active.Clear();
        }

        //Creates new instaces of the pooled object using the given "CreateFunction".
        protected virtual T Create()
        {
            T newItem = CreateFunction();
            members.Add(newItem);

            return newItem;
        }
    }


    //Class responsible for pooling and managing objects deriving from MonoBehaviour.
    public class ObjectPool<T> : Pool<T> where T : MonoBehaviour, IPoolable
    {
        Transform _poolHolder;

        public ObjectPool(Func<T> CreateFunction, int maxAmount = 10, string poolName = "") : base(CreateFunction, maxAmount)
        {
            _poolHolder = new GameObject($"{poolName} (ObjectPool)").transform;
        }
        
        //Returns the given object to the pool.
        public override void Release(T item)
        {
            base.Release(item);
            item.transform.parent = _poolHolder;
        }

        //Destroys the pool and all elements contained within.
        public override void Dispose()
        {
            foreach (T item in members) {
                if(item.gameObject != null)
                UnityEngine.Object.Destroy(item.gameObject);
            }

            base.Dispose();
        }

        //Creates new instaces of the pooled object using the given "CreateFunction".
        protected override T Create()
        {
            T newItem = CreateFunction();
            members.Add(newItem);
            newItem.transform.parent = _poolHolder;
            newItem.name += " (Pooled)";

            return newItem;
        }
    }
}