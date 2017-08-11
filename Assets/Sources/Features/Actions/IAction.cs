using ProtoBuf;

[ProtoContract]
[ProtoInclude(1, typeof(BasicMoveAction))]
public interface IAction
{
	bool Validate(GameContext context);
}