using System;
using System.Collections.Generic;

namespace DevLib.PolyNavMesh
{
    /// <summary>
    /// Delegate를 통해서 Comparison을 직접 구현할 거라서 IComparable를 구현하지 않는다.
    /// Compare(a, b) 가 0보다 작으면 a가 b보다 높은 우선순위를 가지는 구조.
    /// </summary>
    public class MinHeap<T>
    {
        private readonly List<T> _heap = new List<T>();
        private readonly Comparison<T> _compare;
        
        public int Count => _heap.Count;

        public MinHeap(Comparison<T> compare) => _compare = compare;

        public void Push(T item)
        {
            _heap.Add(item);
            HeapifyUp(_heap.Count - 1);
        }

        public T Pop()
        {
            T top = _heap[0];
            int last = _heap.Count - 1;
            _heap[0] = _heap[last]; //마지막 원소를 맨 위로
            _heap.RemoveAt(last);
            if(_heap.Count > 0) HeapifyDown(0); //힙을 재정렬한다.
            return top;
        }

        public T Peek() => _heap[0];

        public T Find(Predicate<T> match)
        {
            int idx = _heap.FindIndex(match);
            return idx <0 ? default : _heap[idx];
        }

        /// <summary>
        /// F 값이 수정되었을 때 위로 올리면서 힙을 복구한다.
        /// </summary>
        public void DecreaseKey(T item)
        {
            int idx = _heap.IndexOf(item);
            if(idx >= 0) HeapifyUp(idx);
        }
        
        private void HeapifyUp(int idx)
        {
            while (idx > 0)
            {
                int parent = (idx - 1) / 2;
                if(_compare(_heap[idx], _heap[parent]) >= 0) break; //순서가 맞으니 더 안올라가도 된다.
                (_heap[idx], _heap[parent]) = (_heap[parent], _heap[idx]);
                idx = parent;
            }
        }
        
        private void HeapifyDown(int idx)
        {
            int count = _heap.Count;
            while (true)
            {
                int smallest = idx;
                int left = 2 * idx + 1;
                int right = 2 * idx + 2;
                if (left < count && _compare(_heap[left], _heap[smallest]) < 0) smallest = left;
                if (right < count && _compare(_heap[right], _heap[smallest]) < 0) smallest = right;
                
                if (smallest == idx) break;
                (_heap[idx], _heap[smallest]) = (_heap[smallest], _heap[idx]);
                idx = smallest;
            }
        }

    }
}