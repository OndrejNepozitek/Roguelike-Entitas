//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ContextMatcherGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class ActionsMatcher {

    public static Entitas.IAllOfMatcher<ActionsEntity> AllOf(params int[] indices) {
        return Entitas.Matcher<ActionsEntity>.AllOf(indices);
    }

    public static Entitas.IAllOfMatcher<ActionsEntity> AllOf(params Entitas.IMatcher<ActionsEntity>[] matchers) {
          return Entitas.Matcher<ActionsEntity>.AllOf(matchers);
    }

    public static Entitas.IAnyOfMatcher<ActionsEntity> AnyOf(params int[] indices) {
          return Entitas.Matcher<ActionsEntity>.AnyOf(indices);
    }

    public static Entitas.IAnyOfMatcher<ActionsEntity> AnyOf(params Entitas.IMatcher<ActionsEntity>[] matchers) {
          return Entitas.Matcher<ActionsEntity>.AnyOf(matchers);
    }
}