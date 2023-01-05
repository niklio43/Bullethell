using System;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell
{
    //Class responsible for pooling and managing objects.
    public class Pool<T> where T : class, IPoolable
    {
        public List<T> members = new List<T>();
        public List<int> active = new List<int>();

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
            int index = 0;

            for (int i = 0; i < members.Count; i++) {
                if (active.Contains(i)) { continue; }
                item = members[i];
                index = i;
            }

            if (item == null) {
                if (members.Count < maxAmount) {
                    item = Create();
                    index = members.Count - 1;
                }
                else {
                    item = members[active[0]];
                    index = active[0];
                    item.ResetObject();
                }
            }

            active.Add(index);
            return item;
        }

        //Returns the given object to the pool.
        public virtual void Release(T item)
        {
            int index = members.IndexOf(item);

            if (!active.Contains(index)) { return; }
            active.Remove(index);
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
                if(item != null && item.gameObject != null)
                UnityEngine.Object.Destroy(item.gameObject);
            }

            base.Dispose();

            GameObject.Destroy(_poolHolder.gameObject);
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