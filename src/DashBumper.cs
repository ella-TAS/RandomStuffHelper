using Celeste.Mod.Entities;
using Monocle;
using Microsoft.Xna.Framework;

namespace Celeste.Mod.RandomStuffHelper;

[CustomEntity("RandomStuffHelper/DashBumper")]
public class DashBumper : Bumper {
    public DashBumper(EntityData data, Vector2 levelOffset) : base(data, levelOffset) {
        PlayerCollider p = Get<PlayerCollider>();
        System.Action<Player> orig = p.OnCollide;
        p.OnCollide = delegate (Player p) {
            if(!p.DashAttacking && respawnTimer <= 0f) {
                fireMode = true;
                orig(p);
                Get<Wiggler>().Start();
                fireMode = false;
            } else {
                orig(p);
            }
        };
        spriteEvil = sprite;
    }

    public static void OnUpdatePosition(On.Celeste.Bumper.orig_UpdatePosition orig, Bumper b) {
        if(b is DashBumper) return;
        orig(b);
    }
}