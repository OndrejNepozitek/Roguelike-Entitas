//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    static readonly Assets.Sources.Features.Init.InitComponent initComponent = new Assets.Sources.Features.Init.InitComponent();

    public bool isInit {
        get { return HasComponent(GameComponentsLookup.Init); }
        set {
            if (value != isInit) {
                if (value) {
                    AddComponent(GameComponentsLookup.Init, initComponent);
                } else {
                    RemoveComponent(GameComponentsLookup.Init);
                }
            }
        }
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

    static Entitas.IMatcher<GameEntity> _matcherInit;

    public static Entitas.IMatcher<GameEntity> Init {
        get {
            if (_matcherInit == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.Init);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherInit = matcher;
            }

            return _matcherInit;
        }
    }
}
