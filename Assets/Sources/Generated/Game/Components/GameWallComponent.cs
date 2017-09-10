//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    static readonly Assets.Sources.Features.GameBoard.Components.WallComponent wallComponent = new Assets.Sources.Features.GameBoard.Components.WallComponent();

    public bool isWall {
        get { return HasComponent(GameComponentsLookup.Wall); }
        set {
            if (value != isWall) {
                if (value) {
                    AddComponent(GameComponentsLookup.Wall, wallComponent);
                } else {
                    RemoveComponent(GameComponentsLookup.Wall);
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

    static Entitas.IMatcher<GameEntity> _matcherWall;

    public static Entitas.IMatcher<GameEntity> Wall {
        get {
            if (_matcherWall == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.Wall);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherWall = matcher;
            }

            return _matcherWall;
        }
    }
}
