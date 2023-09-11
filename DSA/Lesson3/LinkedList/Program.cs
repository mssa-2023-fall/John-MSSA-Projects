using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedList
{
    public interface ILinkedList<T>
    {
        int Count { get; }
        void AddFirst(INode<T> value);
        void InsertAfterNodeIndex(INode<T> value, int position);
        void AddLast(INode<T> value);
        void Clear();
        void RemoveFirst();
        void RemoveAt(int IndexPosition);
        void RemoveLast();
        INode<T>? FindFirst(T value);
        INode<T>[] FindAll(T value);
        INode<T> Head { get; }
        INode<T> Tail { get; }

        IEnumerable<INode<T>> Nodes { get; }
    }
    public interface INode<T>
    {
        T Content { get; set; }
        INode<T>? Next();
    }

}