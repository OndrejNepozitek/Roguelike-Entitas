﻿namespace Assets.Sources.Helpers.Networking.ControlMessages
{
	using ProtoBuf;

	[ProtoContract]
	public class StartGameMessage : IControlMessage
	{
		[ProtoMember(1)] public int Seed;
	}
}