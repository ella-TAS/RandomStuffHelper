using Celeste.Mod.RandomStuffHelper.Entities;
using System;

namespace Celeste.Mod.RandomStuffHelper;

public class RandomStuffHelperModule : EverestModule {
    public static RandomStuffHelperModule Instance;

    public RandomStuffHelperModule() {
        Instance = this;
        Logger.SetLogLevel("RandomStuffHelper", LogLevel.Verbose);
    }

    public override void Load() {
        On.Celeste.Bumper.UpdatePosition += DashBumper.OnUpdatePosition;
    }

    public override void Unload() {
        On.Celeste.Bumper.UpdatePosition -= DashBumper.OnUpdatePosition;
    }
}