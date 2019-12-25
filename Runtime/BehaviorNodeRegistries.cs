using EntitiesBT.Nodes;

namespace EntitiesBT
{
    public static class BehaviorNodeRegistries
    {
        public static void RegisterCommonNodes(this BehaviorNodeFactory factory)
        {
            factory.Register<SequenceNode>();
            factory.Register<SelectorNode>();
            factory.Register<ParallelNode>();
            
            factory.Register<ResetChildrenNode>();
            factory.Register<ResetDescendantsNode>();
            
            factory.Register<SuccessNode>();
            factory.Register<FailureNode>();
            factory.Register<RunningNode>();
        }
    }
}
