using Monocle;
using Microsoft.Xna.Framework;
using Celeste.Mod.Entities;
using System.Collections;

namespace Celeste.Mod.RandomStuffHelper;

[CustomEntity("RandomStuffHelper/PlixFakeHeart")]
public class PlixFakeHeart : HeartGem {
    private HoldableCollider hCollider;
    private PlayerCollider pCollider;
    private readonly string poemID;

    public PlixFakeHeart(EntityData data, Vector2 levelOffset) : base(data.Position + levelOffset) {
        IsFake = IsGhost = false;
        poemID = data.Attr("poem", "RWC2023_Plixona_heartpoem");
    }

    public new void OnHoldable(Holdable h) {
        Player entity = base.Scene.Tracker.GetEntity<Player>();
        if(!collected && entity != null && h.Dangerous(hCollider)) {
            Collect(entity);
        }
    }

    public new void OnPlayer(Player player) {
        if(collected || SceneAs<Level>().Frozen) {
            return;
        }
        if(player.DashAttacking) {
            Collect(player);
            return;
        }
        if(bounceSfxDelay <= 0f) {
            if(IsFake) {
                Audio.Play("event:/new_content/game/10_farewell/fakeheart_bounce", Position);
            } else {
                Audio.Play("event:/game/general/crystalheart_bounce", Position);
            }
            bounceSfxDelay = 0.1f;
        }
        player.PointBounce(base.Center);
        moveWiggler.Start();
        ScaleWiggler.Start();
        moveWiggleDir = (base.Center - player.Center).SafeNormalize(Vector2.UnitY);
        Input.Rumble(RumbleStrength.Medium, RumbleLength.Medium);
    }

    private new void Collect(Player player) {
        base.Scene.Tracker.GetEntity<AngryOshiro>()?.StopControllingTime();
        Coroutine coroutine = new(CollectRoutine(player)) {
            UseRawDeltaTime = true
        };
        Add(coroutine);
        collected = true;
    }

    private new IEnumerator CollectRoutine(Player player) {
        Level level = Scene as Level;
        AreaKey area = level.Session.Area;
        const string text = "event:/game/general/crystalheart_blue_get";
        SoundEmitter sfx = SoundEmitter.Play(text, this);
        Add(new LevelEndingHook(delegate {
            sfx.Source.Stop();
        }));
        Depth = -2000000;
        yield return null;
        Celeste.Freeze(0.2f);
        yield return null;
        Engine.TimeRate = 0.5f;
        player.Depth = -2000000;
        for(int i = 0; i < 10; i++) {
            Scene.Add(new AbsorbOrb(Position));
        }
        level.Shake();
        Input.Rumble(RumbleStrength.Strong, RumbleLength.Medium);
        level.Flash(Color.White);
        level.FormationBackdrop.Display = true;
        level.FormationBackdrop.Alpha = 1f;
        light.Alpha = bloom.Alpha = 0f;
        Visible = false;
        for(float t3 = 0f; t3 < 2f; t3 += Engine.RawDeltaTime) {
            Engine.TimeRate = Calc.Approach(Engine.TimeRate, 0f, Engine.RawDeltaTime * 0.25f);
            yield return null;
        }
        yield return null;
        if(player.Dead) {
            yield return 100f;
        }
        Engine.TimeRate = 1f;
        Tag = Tags.FrozenUpdate;
        level.Frozen = true;
        string text2 = null;
        if(!string.IsNullOrEmpty(poemID)) {
            text2 = Dialog.Clean("poem_" + poemID);
        }
        Poem poem = new(text2, (int) area.Mode, 0.6f) {
            Alpha = 0f
        };
        this.poem = poem;
        Scene.Add(poem);
        for(float t3 = 0f; t3 < 1f; t3 += Engine.RawDeltaTime) {
            poem.Alpha = Ease.CubeOut(t3);
            yield return null;
        }
        while(!Input.MenuConfirm.Pressed && !Input.MenuCancel.Pressed) {
            yield return null;
        }
        sfx.Source.Param("end", 1f);
        level.FormationBackdrop.Display = false;
        for(float t3 = 0f; t3 < 1f; t3 += Engine.RawDeltaTime * 2f) {
            poem.Alpha = Ease.CubeIn(1f - t3);
            yield return null;
        }
        player.Depth = 0;
        EndCutscene();
    }

    public override void Awake(Scene scene) {
        base.Awake(scene);
        sprite.Color = new Color(167, 5, 254);
        hCollider = Get<HoldableCollider>();
        pCollider = Get<PlayerCollider>();
        hCollider.OnCollide = OnHoldable;
        pCollider.OnCollide = OnPlayer;
    }
}