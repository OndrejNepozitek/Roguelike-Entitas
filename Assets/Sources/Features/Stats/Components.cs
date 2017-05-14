using Entitas;

public sealed class StatsComponent : IComponent
{
    public int attack;
    public int attackSpeed;
    public int defense;
    public int movementSpeed;
}

public sealed class Health : IComponent
{
    public int value;
}

public sealed class Dead : IComponent
{

}