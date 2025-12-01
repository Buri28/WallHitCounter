using System;
using UnityEngine;
using Zenject;

namespace WallHitCounter.Services
{
    /// <summary>
    /// 壁ヒットと爆弾ヒットのカウントを管理するサービス。
    /// Zenject によってプレイヤー・スコープで提供される。
    /// </summary>
    public class WallHitService : IInitializable, IDisposable
    {
        private int wallHitCount = 0;
        private int bombHitCount = 0;
        private ObstacleMonitor obstacleMonitor;

        public int WallHitCount => wallHitCount;
        public int BombHitCount => bombHitCount;

        public event Action OnCountChanged;

        [Inject] private DiContainer container = null;
        [Inject(Optional = true)] private PlayerHeadAndObstacleInteraction playerHeadAndObstacleInteraction = null;

        /// <summary>
        /// サービスの初期化処理。監視コンポーネントのセットアップを行う。
        /// </summary>
        public void Initialize()
        {
            SetupMonitors();
        }

        /// <summary>
        /// 障害物監視コンポーネントをセットアップします。
        /// </summary>
        private void SetupMonitors()
        {
            if (obstacleMonitor != null) return;
            if (container == null || playerHeadAndObstacleInteraction == null) return;
            
            // モニター作成と初期化
            obstacleMonitor = 
                container.InstantiateComponentOnNewGameObject<ObstacleMonitor>("WallHitCounter_ObstacleMonitor");
            obstacleMonitor.Initialize(this, playerHeadAndObstacleInteraction);
        }

        /// <summary>
        /// 障害物ヒットカウントをインクリメントします。
        /// </summary>
        public void IncrementWallHit()
        {
            wallHitCount++;
            OnCountChanged?.Invoke();
        }

        /// <summary>
        /// 爆弾ヒットカウントをインクリメントします。
        /// </summary>
        public void IncrementBombHit()
        {
            bombHitCount++;
            OnCountChanged?.Invoke();
        }

        /// <summary>
        /// サービスを破棄します。
        /// </summary>
        public void Dispose()
        {
            if (obstacleMonitor != null)
            {
                UnityEngine.Object.Destroy(obstacleMonitor.gameObject);
                obstacleMonitor = null;
            }
        }
    }
}
