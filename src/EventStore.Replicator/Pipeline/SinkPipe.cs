using EventStore.Replicator.Shared;
using GreenPipes;

namespace EventStore.Replicator.Pipeline {
    public class SinkContext : BasePipeContext, PipeContext {
        public SinkContext(ProposedEvent proposedEvent) => ProposedEvent = proposedEvent;

        public ProposedEvent ProposedEvent { get; }
    }

    public static class SinkPipelineExtensions {
        public static void UseEventWriter(this IPipeConfigurator<SinkContext> cfg, IEventWriter writer)
            => cfg.UseExecuteAsync(ctx => writer.WriteEvent(ctx.ProposedEvent, ctx.CancellationToken));

        public static void UseCheckpointStore(this IPipeConfigurator<SinkContext> cfg, ICheckpointStore checkpointStore)
            => cfg.UseExecuteAsync(async ctx => await checkpointStore.StoreCheckpoint(ctx.ProposedEvent.SourcePosition, ctx.CancellationToken));
    }
}
