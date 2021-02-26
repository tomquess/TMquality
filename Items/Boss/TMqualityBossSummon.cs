using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using System;

namespace TMquality.Items.Boss
{
	public class TMqualityBossSummon : ModItem
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Eye from another world"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("Developer is watching you from another dimension");
		}

		public override void SetDefaults() 
		{
			
			item.width = 30;
			item.height = 20;
			item.useTime = 45;
			item.useAnimation = 45;
			item.useStyle = 4;
			item.rare = 4;
			item.consumable = true;
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

        public override void AddRecipes() 
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Obsidian, 10);
			recipe.AddIngredient(ItemID.LunarBar, 1);
			recipe.AddTile(TileID.DemonAltar);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}