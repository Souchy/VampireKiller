using Godot;

namespace vampirekiller.logia.net;

/// <summary>
/// Todo: put in a /util or /net package?
/// </summary>
public interface Net
{
    public Error Rpc(StringName method, params Variant[] args);
    public Error RpcId(long peerId, StringName method, params Variant[] args);
    public Error RpcServer(StringName method, params Variant[] args)
        => RpcId(1, method, args);
}