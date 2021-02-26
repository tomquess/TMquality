using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using System;

namespace TMquality.Items.Boss
{
	public class TMqualityBossTreasureBag : ModItem
	{

        public override int BossBagNPC => mod.NPCType("TMqualityBoss"); 

        public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Bulging bag full of coins"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("<right> to open");
		}

		public override void SetDefaults() 
		{
			
			item.width = 32;
			item.height = 32;
			item.maxStack = 999;
			item.rare = 9;
			item.consumable = true;
			item.expert = true;
		}

        public override bool CanUseItem(Player player)
        {
			return !NPC.AnyNPCs(mod.NPCType("TMqualityBoss"));
        }

        public override bool UseItem(Player player)
        {
			Main.PlaySound(SoundID.Roar, player.position);
			if(Main.netMode != 1)
            {
				NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("TMqualityBoss"));
            }
			return true;
        }

        public override void OpenBossBag(Player player)
        {
			player.QuickSpawnItem(ItemID.PlatinumCoin, 150);
        }
    }
}