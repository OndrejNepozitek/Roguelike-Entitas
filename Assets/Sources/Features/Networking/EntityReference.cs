using ProtoBuf;

[ProtoContract]
public class EntityReference
{
	[ProtoMember(1)]
	public int Id;

	public GameEntity GetEntity()
	{
		return EntityDatabase.Instance.GetEntity(Id);
	}
}