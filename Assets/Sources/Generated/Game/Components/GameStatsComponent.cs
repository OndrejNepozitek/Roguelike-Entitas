//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public Assets.Sources.Features.Stats.Components.StatsComponent stats { get { return (Assets.Sources.Features.Stats.Components.StatsComponent)GetComponent(GameComponentsLookup.Stats); } }
    public bool hasStats { get { return HasComponent(GameComponentsLookup.Stats); } }

    public void AddStats(int newMaxHealth, int newAttack, int newAttackSpeed, int newDefense, int newMovementSpeed, int newCriticalChance) {
        var index = GameComponentsLookup.Stats;
        var component = CreateComponent<Assets.Sources.Features.Stats.Components.StatsComponent>(index);
        component.MaxHealth = newMaxHealth;
        component.Attack = newAttack;
        component.AttackSpeed = newAttackSpeed;
        component.Defense = newDefense;
        component.MovementSpeed = newMovementSpeed;
        component.CriticalChance = newCriticalChance;
        AddComponent(index, component);
    }

    public void ReplaceStats(int newMaxHealth, int newAttack, int newAttackSpeed, int newDefense, int newMovementSpeed, int newCriticalChance) {
        var index = GameComponentsLookup.Stats;
        var component = CreateComponent<Assets.Sources.Features.Stats.Components.StatsComponent>(index);
        component.MaxHealth = newMaxHealth;
        component.Attack = newAttack;
        component.AttackSpeed = newAttackSpeed;
        component.Defense = newDefense;
        component.MovementSpeed = newMovementSpeed;
        component.CriticalChance = newCriticalChance;
        ReplaceComponent(index, component);
    }

    public void RemoveStats() {
        RemoveComponent(GameComponentsLookup.Stats);
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class GameMatcher {

    static Entitas.IMatcher<GameEntity> _matcherStats;

    public static Entitas.IMatcher<GameEntity> Stats {
        get {
            if (_matcherStats == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.Stats);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherStats = matcher;
            }

            return _matcherStats;
        }
    }
}
