using System;
using System.Collections.Generic;

public class EntityDatabase
{
	private Dictionary<int, GameEntity> entities = new Dictionary<int, GameEntity>();
	private int id = 0;

	private static EntityDatabase _instance;
	public static EntityDatabase Instance
	{
		get { return _instance ?? (_instance = new EntityDatabase()); }
	}

	public void Reset()
	{
		entities = new Dictionary<int, GameEntity>();
		id = 0;
	}

	public GameEntity GetEntity(int id)
	{
		GameEntity entity;
		entities.TryGetValue(id, out entity);

		return entity;
	}

	public void AddEntity(int id, GameEntity entity)
	{
		if (entities.ContainsKey(id))
		{
			throw new ArgumentException("Entity is already in the database");
		}

		entities.Add(id, entity);

		if (id > this.id)
		{
			this.id = id;
		}
	}

	public int AddEntity(GameEntity entity)
	{
		id++;
		entities.Add(id, entity);
		return id;
	}

	public void RemoveEntity(int id)
	{
		entities.Remove(id);
	}

	public int GetNextId()
	{
		id++;
		return id;
	}
}