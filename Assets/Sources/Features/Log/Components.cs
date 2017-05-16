using Entitas;
using System.Collections.Generic;

public sealed class LogComponent : IComponent
{
    public Queue<string> queue;
    public int maxSize;
}