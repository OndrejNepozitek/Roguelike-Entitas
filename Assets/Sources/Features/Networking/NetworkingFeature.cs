namespace Assets.Sources.Features.Networking
{
	using Systems;
	using Helpers.SystemDependencies;

	public class NetworkingFeature : Feature
	{
		public NetworkingFeature(Contexts contexts) : base("Networking feature")
		{
			// TODO: not good
			if (NetworkController.Instance.IsMultiplayer && !NetworkController.Instance.IsServer)
			{
				Add(new ClientSystem(contexts));
			}
			Add(new ServerSendSystem(contexts));
			Add(new ServerReceiveSystem(contexts));
			Add(new NetworkTrackingSystem(contexts));
			Add(new PlayerDisconnectSystem(contexts));
		}
	}
}
