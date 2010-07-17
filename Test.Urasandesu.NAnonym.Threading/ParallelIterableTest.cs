using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System.Threading;
using Urasandesu.NAnonym;
using Urasandesu.NAnonym.Linq;
using System.Diagnostics;
using Moq;

namespace Test.Urasandesu.NAnonym.Threading
{
    [TestFixture]
    public class ParallelIterableTest
    {
        [Test]
        public void Hoge()
        {
            DateTime startTime;
            DateTime endTime;
            var threads = new List<Thread>();

            var monitorCounter = new MonitorCounter();
            threads.Clear();
            startTime = DateTime.Now;
            for (int i = 0; i < 10; i++)
            {
                var thread = new Thread(() => Enumerable.Range(0, 100000).ForEach(_ => monitorCounter.Increace()));
                thread.Start();
                threads.Add(thread);
            }
            threads.ForEach(thread => thread.Join());
            endTime = DateTime.Now;
            Console.WriteLine("Result: {0}, Time: {1} ms", monitorCounter.Value, (endTime.Ticks - startTime.Ticks) * 100d / 1000d / 1000d);


            var spinlockCounter = new SpinlockCounter();
            threads.Clear();
            startTime = DateTime.Now;
            for (int i = 0; i < 10; i++)
            {
                var thread = new Thread(() => Enumerable.Range(0, 100000).ForEach(_ => spinlockCounter.Increace()));
                thread.Start();
                threads.Add(thread);
            }
            threads.ForEach(thread => thread.Join());
            endTime = DateTime.Now;
            Console.WriteLine("Result: {0}, Time: {1} ms", spinlockCounter.Value, (endTime.Ticks - startTime.Ticks) * 100d / 1000d / 1000d);


            var casCounter = new CASCounter();
            threads.Clear();
            startTime = DateTime.Now;
            for (int i = 0; i < 10; i++)
            {
                var thread = new Thread(() => Enumerable.Range(0, 100000).ForEach(_ => casCounter.Increace()));
                thread.Start();
                threads.Add(thread);
            }
            threads.ForEach(thread => thread.Join());
            endTime = DateTime.Now;
            Console.WriteLine("Result: {0}, Time: {1} ms", casCounter.Value, (endTime.Ticks - startTime.Ticks) * 100d / 1000d / 1000d);
            
        }

        sealed class MonitorCounter
        {
            private object syncObject = new object();
            private int value;
            public int Value { get { return value; } }

            public void Increace()
            {
                // Reentrant 可能なのはこちらだけっぽいね。
                lock (syncObject)
                {
                    lock (syncObject)
                    {
                        value++;
                    }
                }
            }
        }

        sealed class SpinlockCounter
        {
            private SpinLock spinLock;
            private int value;
            public int Value { get { return value; } }

            public void Increace()
            {
                spinLock.Enter();
                try
                {
                    spinLock.Enter();
                    try
                    {
                        value++;
                    }
                    finally
                    {
                        spinLock.Exit();
                    }
                }
                finally
                {
                    spinLock.Exit();
                }
            }
        }

        sealed class CASCounter
        {
            private int value;
            public int Value { get { return value; } }

            public void Increace()
            {
                Interlocked.Increment(ref value);
            }
        }

        [Test]
        public void LockTest()
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            var @lock = default(Lock);
            var threads = new List<Thread>();
            int counter = 0;

            
            @lock = new TASLock();
            counter = 0;
            threads.Clear();
            stopwatch.Reset();
            stopwatch.Start();
            for (int i = 0; i < Environment.ProcessorCount; i++)
            {
                var thread = new Thread(() => Enumerable.Range(0, 100000).ForEach(_ => { @lock.Lock(); counter++; @lock.Unlock(); }));
                thread.Start();
                threads.Add(thread);
            }
            threads.ForEach(thread => thread.Join());
            stopwatch.Stop();
            Console.WriteLine("Counter: {0}, Time: {1} ms", counter, stopwatch.ElapsedMilliseconds);


            @lock = new TTASLock();
            counter = 0;
            threads.Clear();
            stopwatch.Reset();
            stopwatch.Start();
            for (int i = 0; i < Environment.ProcessorCount; i++)
            {
                var thread = new Thread(() => Enumerable.Range(0, 100000).ForEach(_ => { @lock.Lock(); counter++; @lock.Unlock(); }));
                thread.Start();
                threads.Add(thread);
            }
            threads.ForEach(thread => thread.Join());
            stopwatch.Stop();
            Console.WriteLine("Counter: {0}, Time: {1} ms", counter, stopwatch.ElapsedMilliseconds);


            @lock = new BackoffLock();
            counter = 0;
            threads.Clear();
            stopwatch.Reset();
            stopwatch.Start();
            for (int i = 0; i < Environment.ProcessorCount; i++)
            {
                var thread = new Thread(() => Enumerable.Range(0, 100000).ForEach(_ => { @lock.Lock(); counter++; @lock.Unlock(); }));
                thread.Start();
                threads.Add(thread);
            }
            threads.ForEach(thread => thread.Join());
            stopwatch.Stop();
            Console.WriteLine("Counter: {0}, Time: {1} ms", counter, stopwatch.ElapsedMilliseconds);


            @lock = new ALock(Environment.ProcessorCount);
            counter = 0;
            threads.Clear();
            stopwatch.Reset();
            stopwatch.Start();
            for (int i = 0; i < Environment.ProcessorCount; i++)
            {
                var thread = new Thread(() => Enumerable.Range(0, 100000).ForEach(_ => { @lock.Lock(); counter++; @lock.Unlock(); }));
                thread.Start();
                threads.Add(thread);
            }
            threads.ForEach(thread => thread.Join());
            stopwatch.Stop();
            Console.WriteLine("Counter: {0}, Time: {1} ms", counter, stopwatch.ElapsedMilliseconds);


        }


        [Test]
        public void LinkListTest()
        {
            var contains = new List<int>();
            var add = new List<int>();
            var remove = new List<int>();
            var random = new Random();

            // contains が 90%、add が 9%、remove が 1% か・・・
            var mock = new Mock<SetTestable<int>>();
            mock.Setup(target => target.Next()).Returns(() => random.Next(100));
            mock.Setup(target => target.InvokesToContains(It.Is<int>(next => 0 <= next && next < 90))).Returns(true);
            mock.Setup(target => target.Contains(It.IsAny<int>())).Callback(() => contains.Add('0')).Returns(true);
            mock.Setup(target => target.InvokesToAdd(It.Is<int>(next => 90 <= next && next < 99))).Returns(true);
            mock.Setup(target => target.Add(It.IsAny<int>())).Callback(() => add.Add('0')).Returns(true);
            mock.Setup(target => target.InvokesToRemove(It.Is<int>(next => next == 99))).Returns(true);
            mock.Setup(target => target.Remove(It.IsAny<int>())).Callback(() => remove.Add('0')).Returns(true);

            var tester = new SetTester<int>(mock.Object);
            Console.WriteLine("Time    : {0} ms", tester.Time());
            Console.WriteLine("contains: {0}", new string(contains.Select(x => Convert.ToChar(x)).ToArray()));
            Console.WriteLine("add     : {0}", new string(add.Select(x => Convert.ToChar(x)).ToArray()));
            Console.WriteLine("remove  : {0}", new string(remove.Select(x => Convert.ToChar(x)).ToArray()));

            // interface を Java の inner クラス風に実装。まさにこんなイメージ！
            // 元になっているのは Castle Project が開発している DynamicProxy。
            // License は Apache License 2.0 だった。プロダクトに使うのも問題なし。
            // ただ、あっこの習慣的に OSS の組み込むのは難しいはず。自分で組むよ。
            
        }

        public interface Set<T>
        {
            bool Contains(T x);
            bool Add(T x);
            bool Remove(T x);
        }

        public interface SetTestable<T> : Set<T>
        {
            T Next();
            bool InvokesToContains(T next);
            bool InvokesToAdd(T next);
            bool InvokesToRemove(T next);
        }

        public class SetTester<T>
        {
            private SetTestable<T> target;
            private static readonly Stopwatch stopwatch = new Stopwatch();
            public SetTester(SetTestable<T> target)
            {
                this.target = target;
            }

            public long Time()
            {
                stopwatch.Reset();
                stopwatch.Start();
                for (int i = 0; i < 1000; i++)
                {
                    var next = target.Next();
                    if (target.InvokesToContains(next))
                    {
                        target.Contains(next);
                    }
                    else if (target.InvokesToAdd(next))
                    {
                        target.Add(next);
                    }
                    else if (target.InvokesToRemove(next))
                    {
                        target.Remove(next);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
                stopwatch.Stop();
                return stopwatch.ElapsedMilliseconds;
            }
        }
    }

    #region for LockTest
    interface Lock
    {
        void Lock();
        void Unlock();
    }

    class TASLock : Lock
    {
        private int state;

        #region Lock Member

        public void Lock()
        {
            while (Interlocked.CompareExchange(ref state, 1, 0) != 0) ;
        }

        public void Unlock()
        {
            Interlocked.Exchange(ref state, 0);
        }

        #endregion
    }

    class TTASLock : Lock
    {
        private int state;

        #region Lock Member

        public void Lock()
        {
            while (true)
            {
                while (state == 1) ;
                if (Interlocked.CompareExchange(ref state, 1, 0) == 0)
                    return;
            }
        }

        public void Unlock()
        {
            Interlocked.Exchange(ref state, 0);
        }

        #endregion
    }

    class BackoffLock : Lock
    {
        struct Backoff
        {
            readonly int minDelay;
            readonly int maxDelay;
            int limit;
            readonly Random random;

            public Backoff(int min, int max)
            {
                minDelay = min;
                maxDelay = max;
                limit = minDelay;
                random = new Random();
            }

            public void Wait()
            {
                int delay = random.Next(limit);
                limit = Math.Min(maxDelay, 2 * limit);
                Thread.Sleep(delay);
            }
        }

        private int state;
        private const int MinDelay = 2;
        private const int MaxDelay = 10;

        #region Lock Member

        public void Lock()
        {
            var backoff = new Backoff(MinDelay, MaxDelay);
            while (true)
            {
                while (state == 1) ;
                if (Interlocked.CompareExchange(ref state, 1, 0) == 0)
                    return;
                else
                    backoff.Wait();
            }
        }

        public void Unlock()
        {
            Interlocked.Exchange(ref state, 0);
        }

        #endregion
    }

    class ALock : Lock
    {
        [ThreadStatic]
        static int mySlotIndex;
        int tail;
        bool[] flag;
        readonly int size;

        public ALock(int capacity)
        {
            size = capacity;
            flag = new bool[capacity];
            flag[0] = true;
        }

        #region Lock Member

        public void Lock()
        {
            int slot = (4 * (Interlocked.Increment(ref tail) - 1)) % size;
            mySlotIndex = slot;
            while (!flag[slot]) Thread.SpinWait(1);
        }

        public void Unlock()
        {
            int slot = mySlotIndex;
            flag[slot] = false;
            flag[(slot + 4) % size] = true;
        }

        #endregion
    }
    #endregion

    public class CoarseList<T>
    {
        private class Node
        {
            public Node(int key)
            {
                this.key = key;
            }

            public T item;
            public int key;
            public Node next;
        }

        private Node head;
        private object @lock = new object();

        public CoarseList()
        {
            head = new Node(int.MinValue);
            head.next = new Node(int.MaxValue);
        }
    }
}
