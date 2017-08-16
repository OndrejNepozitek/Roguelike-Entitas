using Entitas;
using Entitas.CodeGeneration.Attributes;

[Unique]
public class ConfigComponent : IComponent
{
	public Config Value;
}