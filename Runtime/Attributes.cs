using System;
using System.Linq;
using TechTalk.SpecFlow.Bindings;

namespace TechTalk.SpecFlow
{
    /// <summary>
    /// Marker attribute that specifies that this class may contain bindings (step definitions, hooks, etc.)
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class BindingAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public abstract class StepDefinitionBaseAttribute : Attribute
    {
        internal BindingType[] Types { get; private set; }
        public string Regex { get; set; }

        internal StepDefinitionBaseAttribute(string regex, BindingType type)
            : this(regex, new[] { type })
        {
        }

        protected StepDefinitionBaseAttribute(string regex, BindingType[] types)
        {
            if (types == null) throw new ArgumentNullException("types");
            if (types.Length == 0) throw new ArgumentException("List cannot be empty", "types");

            Regex = regex;
            Types = types;
        }
    }

    /// <summary>
    /// Specifies a 'Given' step definition that matches for the provided regular expression.
    /// </summary>
    public class GivenAttribute : StepDefinitionBaseAttribute
    {
        public GivenAttribute(string regex)
            : base(regex, BindingType.Given)
        {
        }
    }

    /// <summary>
    /// Specifies a 'When' step definition that matches for the provided regular expression.
    /// </summary>
    public class WhenAttribute : StepDefinitionBaseAttribute
    {
        public WhenAttribute(string regex)
            : base(regex, BindingType.When)
        {
        }
    }

    /// <summary>
    /// Specifies a 'Then' step definition that matches for the provided regular expression.
    /// </summary>
    public class ThenAttribute : StepDefinitionBaseAttribute
    {
        public ThenAttribute(string regex)
            : base(regex, BindingType.Then)
        {
        }
    }

    /// <summary>
    /// Specifies a step definition that matches for the provided regular expression and any step kinds (given, when, then).
    /// </summary>
    public class StepDefinitionAttribute : StepDefinitionBaseAttribute
    {
        public StepDefinitionAttribute(string regex) : base(regex, new[] { BindingType.Given, BindingType.When, BindingType.Then })
        {
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public abstract class HookAttribute : Attribute
    {
        internal BindingEvent Event { get; private set; }
        public string[] Tags { get; private set; }

        internal HookAttribute(BindingEvent bindingEvent, string[] tags)
        {
            Event = bindingEvent;
            Tags = tags;
        }
    }

    public class BeforeTestRunAttribute : HookAttribute
    {
        public BeforeTestRunAttribute() : base(BindingEvent.TestRunStart, null) {}
    }

    public class AfterTestRunAttribute : HookAttribute
    {
        public AfterTestRunAttribute() : base(BindingEvent.TestRunEnd, null) { }
    }

    public class BeforeFeatureAttribute : HookAttribute
    {
        public BeforeFeatureAttribute(params string[] tags) : base(BindingEvent.FeatureStart, tags) { }
    }

    public class AfterFeatureAttribute : HookAttribute
    {
        public AfterFeatureAttribute(params string[] tags) : base(BindingEvent.FeatureEnd, tags) { }
    }

    /// <summary>
    /// Specifies a hook to be executed before each scenario.
    /// </summary>
    public class BeforeScenarioAttribute : HookAttribute
    {
        public BeforeScenarioAttribute(params string[] tags) : base(BindingEvent.ScenarioStart, tags) { }
    }

    /// <summary>
    /// Specifies a hook to be executed before each scenario. This attribute is a synonym to <see cref="BeforeScenarioAttribute"/>.
    /// </summary>
    public class BeforeAttribute : BeforeScenarioAttribute
    {
        public BeforeAttribute(params string[] tags) : base(tags) { }
    }

    /// <summary>
    /// Specifies a hook to be executed after each scenario.
    /// </summary>
    public class AfterScenarioAttribute : HookAttribute
    {
        public AfterScenarioAttribute(params string[] tags) : base(BindingEvent.ScenarioEnd, tags) { }
    }

    /// <summary>
    /// Specifies a hook to be executed after each scenario. This attribute is a synonym to <see cref="AfterScenarioAttribute"/>.
    /// </summary>
    public class AfterAttribute : AfterScenarioAttribute
    {
        public AfterAttribute(params string[] tags) : base(tags) { }
    }

    public class BeforeScenarioBlockAttribute : HookAttribute
    {
        public BeforeScenarioBlockAttribute(params string[] tags) : base(BindingEvent.BlockStart, tags) { }
    }

    public class AfterScenarioBlockAttribute : HookAttribute
    {
        public AfterScenarioBlockAttribute(params string[] tags) : base(BindingEvent.BlockEnd, tags) { }
    }

    public class BeforeStepAttribute : HookAttribute
    {
        public BeforeStepAttribute(params string[] tags) : base(BindingEvent.StepStart, tags) { }
    }

    public class AfterStepAttribute : HookAttribute
    {
        public AfterStepAttribute(params string[] tags) : base(BindingEvent.StepEnd, tags) { }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class StepArgumentTransformationAttribute : Attribute
    {
        public string Regex { get; set; }

        public StepArgumentTransformationAttribute(string regex)
        {
            Regex = regex;
        }   
        
        public StepArgumentTransformationAttribute()
        {
            Regex = null;
        }
    }

    /// <summary>
    /// Restricts the binding attributes (step definition, hook, etc.) to be applied only in a specific scope.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class ScopeAttribute : Attribute
    {
        public string Tag { get; set; }
        public string Feature { get; set; }
        public string Scenario { get; set; }
    }

    [Obsolete("Use [Scope] attribute instead.")]
    public class StepScopeAttribute : ScopeAttribute
    {
    }
}