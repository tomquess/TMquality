using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Localization;
using System.Threading;
using System.Collections.Generic;
using Terraria.ModLoader.IO;
using System.IO;

namespace TMquality.Projectiles.Bosses
{
    public class TMqualityBossProjectile2 : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("TMquality Boss Ball!");
        }

        public override void SetDefaults()
        {
            projectile.width = 60;
            projectile.height = 11;
            projectile.aiStyle = 1;
            aiType = ProjectileID.Bullet;
            projectile.friendly = false;
            projectile.ignoreWater = true;
            projectile.penetrate = 1;
            projectile.timeLeft = 1000;
            projectile.tileCollide = false;
            projectile.hostile = true;
            projectile.scale = 3f;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override Color ? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void AI()
        {
            projectile.velocity.Y += projectile.ai[0];
        }
    }
}


