using Hazel;
using NEXT.Features.Managers;
using Reactor.Networking.Attributes;
using Reactor.Networking.Rpc;

namespace NEXT.Networking.Color;

[RegisterCustomRpc((uint)LaunchpadRpc.CustomSetColor)]
public class CustomRpcSetColor(NEXTPlugin plugin, uint id) : PlayerCustomRpc<NEXTPlugin, CustomColorData>(plugin, id)
{
    public override RpcLocalHandling LocalHandling => RpcLocalHandling.Before;

    public override void Write(MessageWriter writer, CustomColorData data)
    {
        writer.Write(data.ColorId);
        writer.Write(data.GradientId);
    }

    public override CustomColorData Read(MessageReader reader)
    {
        return new CustomColorData(reader.ReadByte(), reader.ReadByte());
    }

    public override void Handle(PlayerControl playerControl, CustomColorData data)
    {
        playerControl.SetColor(data.ColorId);
        playerControl.SetGradient(data.GradientId);
    }
}