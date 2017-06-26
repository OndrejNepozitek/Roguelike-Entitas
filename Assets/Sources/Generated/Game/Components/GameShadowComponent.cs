//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public ShadowComponent shadow { get { return (ShadowComponent)GetComponent(GameComponentsLookup.Shadow); } }
    public bool hasShadow { get { return HasComponent(GameComponentsLookup.Shadow); } }

    public void AddShadow(int newValue) {
        var index = GameComponentsLookup.Shadow;
        var component = CreateComponent<ShadowComponent>(index);
        component.value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceShadow(int newValue) {
        var index = GameComponentsLookup.Shadow;
        var component = CreateComponent<ShadowComponent>(index);
        component.value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveShadow() {
        RemoveComponent(GameComponentsLookup.Shadow);
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

    static Entitas.IMatcher<GameEntity> _matcherShadow;

    public static Entitas.IMatcher<GameEntity> Shadow {
        get {
            if (_matcherShadow == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.Shadow);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherShadow = matcher;
            }

            return _matcherShadow;
        }
    }
}