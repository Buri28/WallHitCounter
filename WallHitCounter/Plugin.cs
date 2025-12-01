using System;
using System.Runtime.CompilerServices;
using IPA;
using IPA.Config;
using IPA.Config.Stores;
using SiraUtil.Zenject;
using Zenject;
using WallHitCounter.Installers;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]

namespace WallHitCounter
{
    /// <summary>
    /// プラグインのエントリポイント。初期化処理と Zenject インストーラーの登録を行う。
    /// </summary>
    [Plugin(RuntimeOptions.DynamicInit)]
    public class Plugin
    {
        internal static Plugin Instance { get; private set; }
        public static IPA.Logging.Logger Log { get; private set; }

        [Init]
        public void Init(IPA.Logging.Logger logger, IPA.Config.Config config, Zenjector zenjector)
        {
            Instance = this;
            Log = logger;
            PluginConfig.Instance = config.Generated<PluginConfig>();
            // WallHitCounter用 Zenject インストーラー登録
            zenjector.Install<WallHitCounterInstaller>(Location.Player);
        }

        [OnStart]
        public void OnApplicationStart() { Log.Info("WallHitCounter started"); }
    }
}
