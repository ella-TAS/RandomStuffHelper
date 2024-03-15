using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using MonoMod.Utils;

namespace Celeste.Mod.RandomStuffHelper.Entities;

[CustomEntity("RandomStuffHelper/DashBumper")]
public class DashBumper : Bumper {
    public DashBumper(EntityData data, Vector2 levelOffset) : base(data, levelOffset) {
        DynamicData dData = DynamicData.For(this);
        PlayerCollider p = Get<PlayerCollider>();
        System.Action<Player> orig = p.OnCollide;
        p.OnCollide = delegate (Player p) {
            if (!p.DashAttacking && dData.Get<float>("respawnTimer") <= 0f) {
                dData.Set("fireMode", true);
                orig(p);
                dData.Set("fireMode", false);
            } else {
                orig(p);
            }
        };
    }
}