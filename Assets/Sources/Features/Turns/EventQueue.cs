using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class EventQueue<T> where T : class
{
    QueueNode firstNode = null;

    public int Count { get; private set; }

    public EventQueue()
    {
        Clear();
    }

    public void Clear()
    {
        firstNode = null;
        Count = 0;
    }

    public void Remove(T item)
    {
        if (firstNode == null)
        {
            return;
        }

        if (firstNode.Item == item)
        {
            firstNode = firstNode.NextNode;
            Count--;
            return;
        }

        foreach (var node in GetAllNodes())
        {
            if (node.NextNode != null && node.NextNode.Item == item)
            {
                Count--;
                if (node.NextNode.NextNode == null)
                {
                    node.NextNode = null;
                } else
                {
                    node.NextNode = node.NextNode.NextNode;
                }
            }
        }
    }

    public void Enqueue(T item, int priority)
    {
        QueueNode newNode = new QueueNode();
        newNode.Item = item;
        newNode.Priority = priority;

        Count++;

        if (firstNode == null)
        {
            firstNode = newNode;
            return;
        }

        if (newNode.Priority <= firstNode.Priority)
        {
            newNode.NextNode = firstNode;
            firstNode = newNode;
            return;
        }

        foreach (var node in GetAllNodes())
        {
            if (node.NextNode == null || (node.Priority <= newNode.Priority && node.NextNode.Priority > newNode.Priority))
            {
                newNode.NextNode = node.NextNode;
                node.NextNode = newNode;
                break;
            }
        }
    }

    public T Peek()
    {
        return firstNode != null ? firstNode.Item : null;
    }

    public T Dequeue()
    {
        if (firstNode == null)
            return null;

        var nodeToReturn = firstNode;

        firstNode = nodeToReturn.NextNode;
        if (nodeToReturn.Priority != 0)
        {
            foreach (var node in GetAllNodes())
            {
                node.Priority -= nodeToReturn.Priority;
            }
        }

        Count--;

        return nodeToReturn.Item;
    }

    private IEnumerable<QueueNode> GetAllNodes()
    {
        if (firstNode == null)
            yield break;

        var currentNode = firstNode;

        while (currentNode != null)
        {
            yield return currentNode;
            currentNode = currentNode.NextNode;
        }
    }

    public IEnumerable<T> GetEnumerator()
    {
        foreach (var node in GetAllNodes())
        {
            yield return node.Item;
        }
    }

    class QueueNode
    {
        public QueueNode NextNode = null;
        public T Item;
        public int Priority;
    }
}