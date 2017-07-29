public sealed class MoveArgs : IEventArgs
{
    public GameEntity source;
}

public sealed class NothingArgs : IEventArgs
{
    public GameEntity source;
}

public sealed class AttackArgs : IEventArgs
{
    public GameEntity source;
    public GameEntity target;
    public float amount;
}