using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Sources.Features.Networking
{
	using Helpers.Entitas;
	using Helpers.Networking;

	public class NetworkingFeature : Feature
	{
		public NetworkingFeature(Contexts contexts) : base("Networking feature")
		{
			// TODO: not good
			if (NetworkController.Instance.IsMultiplayer && !(NetworkController.Instance.NetworkEntity is Server))
			{
				Add(new ClientSystem(contexts));
			}
			Add(new ServerSystem(contexts));
			Add(new NetworkTrackingSystem(contexts));
		}
	}
}
