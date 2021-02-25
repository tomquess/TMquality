using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.Localization;
using System.Threading;
using System.Collections.Generic;
using Terraria.ModLoader.IO;
using System.IO;

namespace TMqualityNpcs
{
	public class TMqualityNpcsWorld : ModWorld
	{
		public static bool DownedTMqualityBoss = false;

		public override void Initialize()
		{
			DownedTMqualityBoss = false;
		}

		public override TagCompound Save()
		{
			var Downed = new List<string>();
			if (DownedTMqualityBoss) Downed.Add("tmqualityBoss");

			return new TagCompound
			{
				{
					"Version", 0
				},
				{
					"Downed", Downed
				}
			};
		}


		public override void Load(TagCompound tag)
		{
			var Downed = tag.GetList<string>("Downed");
			DownedTMqualityBoss = Downed.Contains("tmqualityBoss");
		}


		public override void LoadLegacy(BinaryReader reader)
		{
			int loadVersion = reader.ReadByte();
			if (loadVersion == 0)
			{
				BitsByte flags = reader.ReadByte();
				DownedTMqualityBoss = flags[0];
			}
		}


		public override void NetSend(BinaryWriter writer)
		{
			BitsByte flags = new BitsByte();
			flags[0] = DownedTMqualityBoss;

			writer.Write(flags);
		}


		public override void NetReceive(BinaryReader reader)
		{
			BitsByte flags = reader.ReadByte();
			DownedTMqualityBoss = flags[0];
		}

	}
}


