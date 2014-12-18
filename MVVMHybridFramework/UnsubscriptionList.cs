using System;
using System.Collections.Generic;

namespace MvvmHybridFramework
{
    public class UnsubscriptionList<T>
        where T : class
    {
        private readonly Dictionary<T, List<IDisposable>> list;
        private readonly object lockObject;

        public UnsubscriptionList()
        {
            this.list = new Dictionary<T, List<IDisposable>>();
            this.lockObject = new object();
        }

        public void AddSubscriber(T item, IDisposable subscription)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item", "item is null.");
            }

            if (subscription == null)
            {
                throw new ArgumentNullException("subscription", "subscription is null.");
            }

            lock (this.lockObject)
            {
                if (this.list.ContainsKey(item))
                {
                    this.list[item].Add(subscription);
                }
                else
                {
                    this.list.Add(item, new List<IDisposable>() { subscription });
                }
            }
        }

        public void Unsubscribe(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item", "item is null.");
            }

            lock (this.lockObject)
            {
                if (this.list.ContainsKey(item))
                {
                    List<IDisposable> unscriberList = this.list[item];
                    this.list.Remove(item);

                    foreach (IDisposable disposable in unscriberList)
                    {
                        disposable.Dispose();
                    }
                }
            }
        }

        public void UnsubscribeAll()
        {
            lock (this.lockObject)
            {
                foreach (T item in this.list.Keys)
                {
                    List<IDisposable> unsubscriberList = this.list[item];

                    foreach (IDisposable disposable in unsubscriberList)
                    {
                        disposable.Dispose();
                    }
                }

                this.list.Clear();
            }
        }
    }
}