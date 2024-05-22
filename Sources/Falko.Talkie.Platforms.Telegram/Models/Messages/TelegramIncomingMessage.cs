using Talkie.Models.Entries;
using Talkie.Platforms;

namespace Talkie.Models.Messages;

public sealed record TelegramIncomingMessage : IIncomingMessage
{
    public required Identifier Id { get; init; }

    public required TelegramEntry Entry { get; init; }

    IEntry Message.IWithEntry.Entry => Entry;

    public required TelegramPlatform Platform { get; init; }

    IPlatform Message.IWithPlatform.Platform => Platform;

    public string? Content { get; init; }

    public TelegramIncomingMessage Mutate(Func<IIncomingMessageMutator, IIncomingMessageMutator> mutation)
    {
        return new TelegramIncomingMessageMutator(this).Mutate();
    }

    IIncomingMessage IIncomingMessage.Mutate(Func<IIncomingMessageMutator, IIncomingMessageMutator> mutation)
    {
        return Mutate(mutation);
    }
}
