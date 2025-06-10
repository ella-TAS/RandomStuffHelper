using Microsoft.Xna.Framework;
using Celeste.Mod.Entities;
using Monocle;

namespace Celeste.Mod.RandomStuffHelper;

[CustomEntity("RandomStuffHelper/WingBerryBarrier")]
public class WingBerryBarrier : SeekerBarrier {
    public WingBerryBarrier(EntityData data, Vector2 levelOffset) : base(data.Position + levelOffset, data.Width, data.Height) { }

    public override void Update() {
        base.Update();
        foreach(Strawberry s in SceneAs<Level>().Entities.FindAll<Strawberry>()) {
            if(Collider.Collide(s.Collider) && s.Winged && s.Follower.Leader == null) {
                Audio.Play("event:/game/general/seed_poof", Position);
                s.RemoveSelf();
            }
        }
    }
}