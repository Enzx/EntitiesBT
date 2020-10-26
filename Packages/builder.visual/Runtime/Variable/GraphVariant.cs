using System.Collections.Generic;
using EntitiesBT.Core;
using EntitiesBT.Entities;
using EntitiesBT.Variant;
using Runtime;
using Unity.Entities;
using UnityEngine.Scripting;
using static EntitiesBT.Core.Utilities;

namespace EntitiesBT.Builder.Visual
{
    public static class GraphVariant
    {
        public const string GUID = "7BCAE9F4-BE78-424D-84DD-5DA101A3F07F";

        public class Reader<T> : IVariantReader<T> where T : unmanaged
        {
            private readonly InputDataPort _port;
            public Reader(InputDataPort port) => _port = port;

            public void Allocate(ref BlobBuilder builder, ref BlobVariant blobVariant, INodeDataBuilder self, ITreeNode<INodeDataBuilder>[] tree)
            {
                blobVariant.VariantId = GuidHashCode(GUID);
                builder.Allocate(ref blobVariant, _port);
            }

            [Preserve, ReaderMethod(GUID)]
            private static unsafe T GetData<TNodeBlob, TBlackboard>(ref BlobVariant blobVariant, int index, ref TNodeBlob blob, ref TBlackboard bb)
                where TNodeBlob : struct, INodeBlob
                where TBlackboard : struct, IBlackboard
            {
                ref var port = ref blobVariant.Value<InputDataPort>();
                var behaviorTree = bb.GetData<CurrentBehaviorTreeComponent>().RefValue.BehaviorTree;
                // HACK: how to support multiple worlds?
                var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
                var graphInstance = entityManager.GetComponentObject<GraphInstanceComponent>(behaviorTree).Value;

                T data;
                void* ptr = &data;
                var value = graphInstance.ReadValue(port);
                Value.SetPtrToValue(ptr, value.Type, value);
                return data;
            }

            [Preserve, AccessorMethod(GUID)]
            IEnumerable<ComponentType> GetComponentAccess(ref BlobVariantReader<T> variant)
            {
                return ComponentType.ReadOnly<CurrentBehaviorTreeComponent>().Yield();
            }
        }
    }
}