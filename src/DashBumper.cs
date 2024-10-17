using Celeste.Mod.Entities;
using Monocle;
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
            if(!p.DashAttacking && dData.Get<float>("respawnTimer") <= 0f) {
                dData.Set("fireMode", true);
                orig(p);
                Get<Wiggler>().Start();
                dData.Set("fireMode", false);
            } else {
                orig(p);
            }
        };
        dData.Set("spriteEvil", dData.Get("sprite"));
    }

    public static void OnUpdatePosition(On.Celeste.Bumper.orig_UpdatePosition orig, Bumper b) {
        if(b is DashBumper) return;
        orig(b);
    }
}