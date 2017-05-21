//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public RevealAroundComponent revealAround { get { return (RevealAroundComponent)GetComponent(GameComponentsLookup.RevealAround); } }
    public bool hasRevealAround { get { return HasComponent(GameComponentsLookup.RevealAround); } }

    public void AddRevealAround(int newRadius) {
        var index = GameComponentsLookup.RevealAround;
        var component = CreateComponent<RevealAroundComponent>(index);
        component.radius = newRadius;
        AddComponent(index, component);
    }

    public void ReplaceRevealAround(int newRadius) {
        var index = GameComponentsLookup.RevealAround;
        var component = CreateComponent<RevealAroundComponent>(index);
        component.radius = newRadius;
        ReplaceComponent(index, component);
    }

    public void RemoveRevealAround() {
        RemoveComponent(GameComponentsLookup.RevealAround);
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

    static Entitas.IMatcher<GameEntity> _matcherRevealAround;

    public static Entitas.IMatcher<GameEntity> RevealAround {
        get {
            if (_matcherRevealAround == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.RevealAround);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherRevealAround = matcher;
            }

            return _matcherRevealAround;
        }
    }
}
