using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coursera
{
	public class Heap<T> where T : IComparable<T>  
	{
		private readonly List<T> _items;
		private int _actualSize = 0;
		private bool _isMaxHeap;

		public Heap(bool isMaxHeap = false, int capacity = 100)
		{
			_items = new List<T>(capacity);
			_isMaxHeap = isMaxHeap;
		}		

		private int GetLeftChildIndex(int parentIndex) => 2 * parentIndex + 1;

		private int GetRightChildIndex(int parentIndex) => 2 * parentIndex + 2;

		private int GetParentIndex(int childIndex) => (childIndex - 1) / 2;

		private bool HasLeftChild(int index) => GetLeftChildIndex(index) < _actualSize;

		private bool HasRightChild(int index) => GetRightChildIndex(index) < _actualSize;

		private bool HasParent(int index) => GetParentIndex(index) >= 0;

		private T GetLeftChild(int index) => _items[GetLeftChildIndex(index)];

		private T GetRightChild(int index) => _items[GetRightChildIndex(index)];

		private T GetParent(int index) => _items[GetParentIndex(index)];

		private bool CompareForHeapifyUp(T item1, T item2) => _isMaxHeap ? item1.CompareTo(item2) < 0 : item1.CompareTo(item2) > 0;

		private bool CompareForHeapifyDown(T item1, T item2) => _isMaxHeap ? item1.CompareTo(item2) > 0 : item1.CompareTo(item2) < 0;

		public int Size => _actualSize;

		public IEnumerable<T> Items => _items.Where(i => i != null);

		private void Swap(int index1, int index2)
		{
			var temp = _items[index2];
			_items[index2] = _items[index1];
			_items[index1] = temp;
		}

		public T Peek()
		{
			if (_actualSize == 0)
			{
				throw new InvalidOperationException("The heap is empty");
			}

			return _items[0];
		}

		public T Poll()
		{
			if (_actualSize == 0)
			{
				throw new InvalidOperationException("The heap is empty");
			}

			var item = _items[0];
			_items[0] = _items[_actualSize - 1];
			_items[_actualSize - 1] = default(T);
			_actualSize--;
			HeapifyDown();
			return item;
		}

		public void Add(T item)
		{
			if (_items.Count == _actualSize)
			{
				_items.Add(item);
			}
			else
			{
				_items[_actualSize] = item;
			}
			_actualSize++;
			HeapifyUp();
		}

		public void AddRange(IEnumerable<T> items)
		{
			foreach(var item in items)
			{
				Add(item);
			}
		}

		public void Update(T original, T updated)
		{
			var index = _items.IndexOf(original);
			if (index == -1)
			{
				throw new Exception("Item not found");
			}

			_items[index] = updated;
			HeapifyUp(index);
			HeapifyDown(index);
		}

		public void Delete(T item)
		{
			var index = _items.IndexOf(item);
			if(index == -1)
			{
				throw new Exception("Item not found");
			}

			Swap(index, _actualSize - 1);
			_actualSize--;

			HeapifyUp(index);
			HeapifyDown(index);
		}

		private void HeapifyUp(int index = -1)
		{
			if (index == -1)
			{
				index = _actualSize - 1;
			}

			while (HasParent(index) && CompareForHeapifyUp(GetParent(index), _items[index]))
			{
				Swap(GetParentIndex(index), index);
				index = GetParentIndex(index);
			}
		}

		private int GetSmallestChildIndex(int index)
		{
			if(!HasLeftChild(index))
			{
				return -1;
			}

			var smallerChildIndex = GetLeftChildIndex(index);
			if (HasRightChild(index) && CompareForHeapifyDown(GetRightChild(index), GetLeftChild(index)))
			{
				smallerChildIndex = GetRightChildIndex(index);
			}

			return smallerChildIndex;
		}

		private void HeapifyDown(int index = 0)
		{
			while (HasLeftChild(index))
			{
				var smallerChildIndex = GetSmallestChildIndex(index);

				if (CompareForHeapifyDown(_items[index], _items[smallerChildIndex]))
				{
					return;
				}

				Swap(index, smallerChildIndex);
				index = smallerChildIndex;
			}
		}
	}
}
