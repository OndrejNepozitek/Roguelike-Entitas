using ProtoBuf;

[ProtoContract]
public class EntityReference
{
	[ProtoMember(1)]
	public int Id;

	public EntityReference()
	{
		
	}

	public EntityReference(int id)
	{
		Id = id;
	}

	public GameEntity GetEntity()
	{
		return EntityDatabase.Instance.GetEntity(Id);
	}
}