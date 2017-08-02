using Entitas;

public sealed class StatsComponent : IComponent
{
    public int Attack;
    public int AttackSpeed;
    public int Defense;
    public int MovementSpeed;
}

public sealed class Health : IComponent
{
    public int Value;
}

public sealed class Dead : IComponent
{

}