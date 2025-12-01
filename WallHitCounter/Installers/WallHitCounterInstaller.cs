using Zenject;
using WallHitCounter.Services;

namespace WallHitCounter.Installers
{
    /// <summary>
    /// Zenject に WallHitService をバインドするインストーラー。
    /// プレイヤー・スコープでサービスを登録する。
    /// </summary>
    internal class WallHitCounterInstaller : Installer
    {
        public override void InstallBindings()
        {
            // プレイヤー・スコープでサービスをシングルトン登録
            Container.BindInterfacesAndSelfTo<WallHitService>().AsSingle();
        }
    }
}
