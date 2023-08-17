using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryTracker
{
    private List<long> memoryHistory = new List<long>();

    public void RecordMemoryUsage()
    {
        long memory = GC.GetTotalMemory(false);
        memoryHistory.Add(memory);
    }

    public void PrintMemoryHistory()
    {
        foreach (var memory in memoryHistory)
        {
            Console.WriteLine($"Bellek kullanımı: {memory} bytes");
        }
    }
}
