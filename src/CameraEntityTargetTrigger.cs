using Celeste.Mod.Entities;
using Celeste.Mod.GameHelper.Entities.Wrappers;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.RandomStuffHelper.Triggers;

[CustomEntity("RandomStuffHelper/CameraEntityTargetTrigger")]
public class CameraEntityTargetTrigger : Wrapper {
    private readonly string onlyType, flag;
    private readonly float lerp;

    private Entity target;

    public CameraEntityTargetTrigger(EntityData data, Vector2 levelOffset) : base(data.Position + levelOffset) {
        onlyType = data.Attr("onlyType");
        flag = data.Attr("flag");
        lerp = data.Float("lerp");
        Depth = -20;
    }

    public override void Update() {
        base.Update();
        Player p = SceneAs<Level>().Tracker.GetEntity<Player>();
        if(target == null || p == null) {
            RemoveSelf();
            return;
        }
        if(flag.Length == 0 || SceneAs<Level>().Session.GetFlag(flag)) {
            p.CameraAnchor = target.Center - 0.5f * new Vector2(317, 184);
            p.CameraAnchorLerp = Vector2.One * lerp;
        }
    }

    public override void Awake(Scene scene) {
        base.Awake(scene);
        target = FindNearest(Position, onlyType);
    }
}