using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.Graphics.Shaders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using System;

namespace TMquality.NPCs.Bosses
{

	[AutoloadBossHead]
	public class TMqualityBoss : ModNPC
	{
		//ai
		private int ai;
		private int attackTimer = 0;
		private bool fastSpeed = false;

		private bool stunned;
		private int stunnedTimer;

		//animation
		private int frame = 0;
		private string pocisk = "TMqualityBossProjectile";
		
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Grupa wpierdolu");
			Main.npcFrameCount[npc.type] = 5;
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax * 0.5f * bossLifeScale);
			npc.damage = (int)(npc.damage * 0.5f);
		}



		public override void SetDefaults() 
		{
			npc.width = 126;
			npc.height = 178;

			npc.boss = true;
			npc.aiStyle = -1;
			npc.npcSlots = 5f;

			npc.lifeMax = 10000000;
			npc.damage = 200;
			npc.defense = 400;
			npc.knockBackResist = 0f;

			npc.value = Item.buyPrice(gold: 10);
			npc.lavaImmune = true;
			npc.noTileCollide = true;
			npc.noGravity = true;

			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/TMqualityBossMusic");
			

		}


        public override void AI() 
		{
			npc.TargetClosest(true);
			Player player = Main.player[npc.target];
			Vector2 target = npc.HasPlayerTarget ? player.Center : Main.npc[npc.target].Center;

			npc.rotation = 0.0f;
			npc.netAlways = true;
			npc.TargetClosest(true);

			if (npc.life <= 8000000 && npc.life >= 6000000)
			{
				frame = 1;
				pocisk = "TMqualityBossProjectile1";
			} else if (npc.life <= 6000000 && npc.life >= 4000000)
			{
				frame = 2;
				pocisk = "TMqualityBossProjectile2";
			}
			else if (npc.life <= 4000000 && npc.life >= 2000000)
			{
				frame = 3;
				pocisk = "TMqualityBossProjectile3";
			}
			else if (npc.life <= 2000000)
			{
				frame = 4;
				pocisk = "TMqualityBossProjectile4";
			}
			else


			if (npc.target < 0 || npc.target == 255 || player.dead || !player.active)
			{
				npc.TargetClosest(false);
				npc.direction = 1;
				npc.velocity.Y = npc.velocity.Y - 0.1f;
				if (npc.timeLeft > 20)
				{
					npc.timeLeft = 20;
					return;

				}
			}

			

			if (stunned)
			{
				npc.velocity.X = 0.0f;
				npc.velocity.Y = 0.0f;

				stunnedTimer++;

				if (stunnedTimer >= 10)
				{
					stunned = false;
					stunnedTimer = 0;
				}
			}
			ai++;

			
			//movement
			npc.ai[0] = (float)ai * 1f;
			int distance = (int)Vector2.Distance(target, npc.Center);
			if((double)npc.ai[0] < 300)
            {
				MoveTowards(npc, target, (float)(distance > 300 ? 20f : 20f), 30f);
				npc.netUpdate = true;
            } else if((double)npc.ai[0] >= 300 && (double)npc.ai[0] <350.0)
            {
				stunned = true;
				npc.defense = 40;
				npc.damage = 300;
				MoveTowards(npc, target, (float)(distance > 300 ? 20f : 20f), 30f);
				npc.netUpdate = true;
            } else if ((double)npc.ai[0] >= 350.0)
            {
				stunned = false;
				npc.damage = 350;
				npc.defense = 200;
				if (!fastSpeed)
                {
					fastSpeed = true;
                }else
                {
					if((double)npc.ai[0] % 50 == 0)
                    {
						float speed = 40f;
						Vector2 vector = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
						float x = player.position.X + (float)(player.width / 2) - vector.X;
						float y = player.position.Y + (float)(player.height / 2) - vector.Y;
						float distance2 = (float)Math.Sqrt(x * x + y * y);
						float factor = speed / distance2;
						npc.velocity.X = x * factor;
						npc.velocity.Y = y * factor;

					}
				}
				npc.netUpdate = true;
			}
			// attack
			if((double)npc.ai[0] % (Main.expertMode ? 50 : 100) == 0 && !stunned && !fastSpeed)
            {
				attackTimer++;
				if (attackTimer <= 2)
                {
					npc.velocity.X = 0f;
					npc.velocity.Y = 0f;
					Vector2 shootPos = npc.Center;
					float accuracy = 12f * (npc.life / npc.lifeMax);
					Vector2 shootVel = target - shootPos + new Vector2(Main.rand.NextFloat(-accuracy, accuracy), Main.rand.NextFloat(-accuracy, accuracy));
					shootVel.Normalize();
					shootVel *= 1500f;
					for(int i=0; i < (Main.expertMode ? 5 : 1); i++)
                    {
						Projectile.NewProjectile(shootPos.X + (float)(-100 * npc.direction) + (float)Main.rand.Next(-200, 201), shootPos.Y - (float)Main.rand.Next(-200, 201), shootVel.X, shootVel.Y, mod.ProjectileType(pocisk), npc.damage / 3, 5f); // Projectile.NewProjectile(shootPos.X + (float)(-100 * npc.direction) + (float)Main.rand.Next(-40, 41), shootPos.Y - (float)Main.rand.Next(-50, 40), shootVel.X, shootVel.Y, mod.ProjectileType("TMqualityBossProjectile"), npc.damage / 3, 5f);

					}
               }else
                {
					attackTimer = 0;
                }
            }

			if ((double)npc.ai[0] >= 650.0)
            {
				ai = 0;
				npc.alpha = 0;
				npc.ai[2] = 0;
				fastSpeed = false;
			}
		}
		
		private void MoveTowards(NPC npc, Vector2 playerTarget, float speed, float turnResistance)
        {
			var move = playerTarget - npc.Center;
			float length = move.Length();
			if(length > speed)
            {
				move *= speed / length;
            }
			move = (npc.velocity * turnResistance + move) / (turnResistance + 1f);
			length = move.Length();
			if (length > speed)
			{
				move *= speed / length;
			}
			npc.velocity = move;
		}

		public override void FindFrame(int frameHeight)
        {
			


			if (frame == 0)
            {
				npc.frame.Y = 0;

            }else if(frame == 1)
			{
				npc.frame.Y = frameHeight;
			}else if (frame == 2)
			{
				npc.frame.Y = frameHeight * 2;
			}
			else if (frame == 3)
			{
				npc.frame.Y = frameHeight * 3;
			}
			else if (frame == 4)
			{
				npc.frame.Y = frameHeight * 4;
			}

		}

        public override void NPCLoot()
        {
			
			if(Main.expertMode)
            {
				npc.DropBossBags();
            } else
            {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.LifeCrystal, Main.rand.Next(1, 3));
				if(Main.rand.Next(7) == 0)
                {
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("TMqualityBossSummon"), 1);
				}
			}
        }

    }
}