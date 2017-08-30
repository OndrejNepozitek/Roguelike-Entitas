using UnityEngine;

namespace Assets.Scripts
{
	using System.Collections.Generic;
	using Sources.Features.Items;
	using Sources.Features.Stats.Components;
	using UnityEngine.UI;

	public class InventoryController : MonoBehaviour
	{
		public GameObject Canvas;

		public Text HelmetText;
		public Text BodyArmorText;
		public Text WeaponText;
		public Text ShieldText;

		public GameObject HelmetImage;
		public GameObject BodyArmorImage;
		public GameObject WeaponImage;
		public GameObject ShieldImage;

		public Text HealthText;
		public Text DefenseText;
		public Text DamageText;

		public Text MainHealthText;
		public Text MainDamageText;

		private Dictionary<InventorySlot, InventoryItem> newestItems;
		private StatsComponent newestStats; // TODO: is this safe?
		private int newestHealth;

		public bool IsOpened;

		private void Start()
		{
			ResetInventory();
		}

		public void SetInventory(Dictionary<InventorySlot, InventoryItem> items)
		{
			ResetInventory();
			newestItems = items;

			foreach (var pair in items)
			{
				var description = pair.Value.Item.ToInventoryString();
				GameObject imageHolder = null;

				switch (pair.Key)
				{
					case InventorySlot.Weapon:
						WeaponText.text = description;
						imageHolder = WeaponImage;
						break;

					case InventorySlot.Shield:
						ShieldText.text = description;
						imageHolder = ShieldImage;
						break;

					case InventorySlot.Body:
						BodyArmorText.text = description;
						imageHolder = BodyArmorImage;
						break;

					case InventorySlot.Head:
						HelmetText.text = description;
						imageHolder = HelmetImage;
						break;
				}

				if (imageHolder != null)
				{
					var asset = Resources.Load<GameObject>(pair.Value.Item.Prefab);
					var image = imageHolder.GetComponent<Image>();
					image.sprite = asset.GetComponent<SpriteRenderer>().sprite;
					imageHolder.SetActive(true);
				}
			}
		}

		public void SetStats(StatsComponent stats)
		{
			newestStats = stats;

			HealthText.text = string.Format("Health: {0}/{1}", newestHealth, stats.MaxHealth);
			DefenseText.text = string.Format("Defense: {0}", stats.Defense);
			DamageText.text = string.Format("Damage: {0}", stats.Attack);

			MainDamageText.text = stats.Attack.ToString();
			MainHealthText.text = string.Format("{0}/{1}", newestHealth, stats.MaxHealth);
		}

		public void SetHealth(int health)
		{
			newestHealth = health;

			SetStats(newestStats);
		}

		private void ResetInventory()
		{
			HelmetText.text = "";
			BodyArmorText.text = "";
			WeaponText.text = "";
			ShieldText.text = "";

			HelmetImage.SetActive(false);
			BodyArmorImage.SetActive(false);
			WeaponImage.SetActive(false);
			ShieldImage.SetActive(false);
		}

		public void Open()
		{
			Canvas.SetActive(true);
			IsOpened = true;
		}

		public void Close()
		{
			Canvas.SetActive(false);
			IsOpened = false;
		}
	}
}
