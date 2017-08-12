using System.Collections.Generic;
using Assets.Sources.Helpers.Monsters;

public class MonsterDatabase
{
	private Dictionary<MonsterType, MonsterConfig> monsters = new Dictionary<MonsterType, MonsterConfig>();

	private static MonsterDatabase _instance;
	public static MonsterDatabase Instance
	{
		get { return _instance ?? (_instance = new MonsterDatabase()); }
	}

	public void Reset()
	{
		monsters = new Dictionary<MonsterType, MonsterConfig>();
	}

	public MonsterConfig GetMonster(MonsterType type)
	{
		return monsters[type];
	}

	public void RegisterMonster(MonsterType type, MonsterConfig config)
	{
		monsters.Add(type, config);
	}
}