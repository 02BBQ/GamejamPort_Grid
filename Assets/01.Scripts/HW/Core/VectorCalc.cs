using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class VectorCalc
{
    public static Transform[] GetClosestTransforms(Transform[] transforms, Transform targetTransform, int count)
    {
        count = Mathf.Min(count, transforms.Length);

        var maxHeap = new MaxHeap(count);
        foreach (var transform in transforms)
        {
            float distance = Vector3.Distance(targetTransform.position, transform.position);
            maxHeap.Add(transform, distance);
        }

        return maxHeap.GetClosestTransforms();
    }
    public static Transform[] GetClosestTransforms(List<Enemy> transforms, Transform targetTransform, int count)
    {
        count = Mathf.Min(count, transforms.Count);

        var maxHeap = new MaxHeap(count);
        foreach (var transform in transforms)
        {
            float distance = Vector3.Distance(targetTransform.position, transform.transform.position);
            maxHeap.Add(transform.transform, distance);
        }

        return maxHeap.GetClosestTransforms();
    }
}

public class MaxHeap
{
    private List<(Transform transform, float distance)> heap;
    private int maxSize;

    public MaxHeap(int maxSize)
    {
        this.maxSize = maxSize;
        heap = new List<(Transform transform, float distance)>(maxSize);
    }

    public void Add(Transform transform, float distance)
    {
        if (heap.Count < maxSize)
        {
            heap.Add((transform, distance));
            if (heap.Count == maxSize)
            {
                for (int i = maxSize / 2 - 1; i >= 0; i--)
                {
                    Heapify(i);
                }
            }
        }
        else if (distance < heap[0].distance)
        {
            heap[0] = (transform, distance);
            Heapify(0);
        }
    }

    public Transform[] GetClosestTransforms()
    {
        Transform[] closestTransforms = new Transform[heap.Count];
        for (int i = 0; i < heap.Count; i++)
        {
            closestTransforms[i] = heap[i].transform;
        }
        return closestTransforms;
    }

    private void Heapify(int i)
    {
        int largest = i;
        int left = 2 * i + 1;
        int right = 2 * i + 2;

        if (left < heap.Count && heap[left].distance > heap[largest].distance)
            largest = left;

        if (right < heap.Count && heap[right].distance > heap[largest].distance)
            largest = right;

        if (largest != i)
        {
            (heap[i], heap[largest]) = (heap[largest], heap[i]);
            Heapify(largest);
        }
    }
}
