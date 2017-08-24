//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public Assets.Sources.Features.Actions.Components.ActionProgressComponent actionProgress { get { return (Assets.Sources.Features.Actions.Components.ActionProgressComponent)GetComponent(GameComponentsLookup.ActionProgress); } }
    public bool hasActionProgress { get { return HasComponent(GameComponentsLookup.ActionProgress); } }

    public void AddActionProgress(float newProgress) {
        var index = GameComponentsLookup.ActionProgress;
        var component = CreateComponent<Assets.Sources.Features.Actions.Components.ActionProgressComponent>(index);
        component.Progress = newProgress;
        AddComponent(index, component);
    }

    public void ReplaceActionProgress(float newProgress) {
        var index = GameComponentsLookup.ActionProgress;
        var component = CreateComponent<Assets.Sources.Features.Actions.Components.ActionProgressComponent>(index);
        component.Progress = newProgress;
        ReplaceComponent(index, component);
    }

    public void RemoveActionProgress() {
        RemoveComponent(GameComponentsLookup.ActionProgress);
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

    static Entitas.IMatcher<GameEntity> _matcherActionProgress;

    public static Entitas.IMatcher<GameEntity> ActionProgress {
        get {
            if (_matcherActionProgress == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.ActionProgress);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherActionProgress = matcher;
            }

            return _matcherActionProgress;
        }
    }
}
