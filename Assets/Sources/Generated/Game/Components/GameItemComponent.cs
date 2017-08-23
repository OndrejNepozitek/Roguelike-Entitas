//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public Assets.Sources.Features.Items.Components.ItemComponent item { get { return (Assets.Sources.Features.Items.Components.ItemComponent)GetComponent(GameComponentsLookup.Item); } }
    public bool hasItem { get { return HasComponent(GameComponentsLookup.Item); } }

    public void AddItem(IItem newItem) {
        var index = GameComponentsLookup.Item;
        var component = CreateComponent<Assets.Sources.Features.Items.Components.ItemComponent>(index);
        component.Item = newItem;
        AddComponent(index, component);
    }

    public void ReplaceItem(IItem newItem) {
        var index = GameComponentsLookup.Item;
        var component = CreateComponent<Assets.Sources.Features.Items.Components.ItemComponent>(index);
        component.Item = newItem;
        ReplaceComponent(index, component);
    }

    public void RemoveItem() {
        RemoveComponent(GameComponentsLookup.Item);
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

    static Entitas.IMatcher<GameEntity> _matcherItem;

    public static Entitas.IMatcher<GameEntity> Item {
        get {
            if (_matcherItem == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.Item);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherItem = matcher;
            }

            return _matcherItem;
        }
    }
}
