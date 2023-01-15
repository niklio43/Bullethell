using System;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell
{
    //Class responsible for pooling and managing objects.
    public class Pool<T> where T : class, IPoolable
    {
        public List<T> Members = new List<T>();
        public List<int> Active = new List<int>();

        protected Func<T> CreateFunction;

        //Max size of the pool.
        protected readonly int _maxAmount;

        public int GetCount() => Members.Count;


        public Pool(Func<T> CreateFunction, int maxAmount = 10)
        {
            if (CreateFunction == null) throw new ArgumentNullException("createFunction");

            this.CreateFunction = CreateFunction;
            this._maxAmount = maxAmount;
        }

        //Returns an available object, if none exist and there's room in the pool then the function creates a new instance.
        public virtual T Get()
        {
            T item = null;
            int index = 0;

            for (int i = 0; i < Members.Count; i++) {
                if (Active.Contains(i)) { continue; }
                item = Members[i];
                index = i;
            }

            if (item == null) {
                if (Members.Count < _maxAmount) {
                    item = Create();
                    index = Members.Count - 1;
                }
                else {
                    item = Members[Active[0]];
                    index = Active[0];
                    item.ResetObject();
                }
            }

            Active.Add(index);
            return item;
        }

        //Returns the given object to the pool.
        public virtual void Release(T item)
        {
            int index = Members.IndexOf(item);

            if (!Active.Contains(index)) { return; }
            Active.Remove(index);
        }

        //Destroys the pool.
        public virtual void Dispose()
        {
            Members.Clear();
            Active.Clear();
        }

        //Creates new instaces of the pooled object using the given "CreateFunction".
        protected virtual T Create()
        {
            T newItem = CreateFunction();
            Members.Add(newItem);

            return newItem;
        }

    }

    //Class responsible for pooling and managing objects deriving from MonoBehaviour.
    public class ObjectPool<T> : Pool<T> where T : Behaviour, IPoolable
    {
        Transform _poolHolder;

        public ObjectPool(Func<T> CreateFunction, int maxAmount = 10, string poolName = "", Transform parent = null) : base(CreateFunction, maxAmount)
        {
            _poolHolder = new GameObject($"{poolName} (ObjectPool)").transform;
            if(parent != null) {
                _poolHolder.transform.parent = parent;
            }
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

            foreach (T item in Members) {
                if(item != null && item.gameObject != null)
                UnityEngine.Object.Destroy(item.gameObject);
            }

            base.Dispose();

            try {
                GameObject.Destroy(_poolHolder.gameObject);
            } catch(Exception e) { }
        }

        //Creates new instaces of the pooled object using the given "CreateFunction".
        protected override T Create()
        {
            T newItem = CreateFunction();
            Members.Add(newItem);
            newItem.transform.parent = _poolHolder;
            newItem.name += " (Pooled)";

            return newItem;
        }
    }
}