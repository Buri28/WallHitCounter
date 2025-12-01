using TMPro;
using UnityEngine;
using CountersPlus.Custom;
using CountersPlus.Utils;
using CountersPlus.Counters.Custom;
using CountersPlus.Counters.Interfaces;
using Zenject;

namespace WallHitCounter.UI
{
    /// <summary>
    /// Counters+ 用のカスタムカウンター実装。
    /// WallHitService のカウントを受け取りテキストで表示する。
    /// </summary>
    public class WallHitCounterUI : BasicCustomCounter, INoteEventHandler, IEventHandler
    {
        private TMP_Text counterText;
        private RectTransform counterRect;
        private static WallHitCounterUI ActiveInstance;

        [Inject] private WallHitCounter.Services.WallHitService service = null;
        [Inject] private CanvasUtility canvasUtility = null;

        /// <summary>
        /// カウンターの初期化。テキスト生成とサービスの購読登録を行う。
        /// </summary>
        public override void CounterInit()
        {
            // 依存性注入チェック
            if (service == null || canvasUtility == null || Settings == null) return;

            // テキスト作成（Counters+ の既定配置を使う）
            CreateOrUpdateText();

            // 登録（BSML 設定画面などから一括操作できるようにする）
            ActiveInstance = this;

            // 表示更新の購読と初回反映
            service.OnCountChanged += UpdateDisplay;
            UpdateDisplay();
        }

        /// <summary>
        /// カウンター破棄時のクリーンアップ処理。購読解除と生成テキストの破棄を行う。
        /// </summary>
        public override void CounterDestroy()
        {
            if (service != null) service.OnCountChanged -= UpdateDisplay;
            if (counterText != null)
            {
                UnityEngine.Object.Destroy(counterText.gameObject);
                counterText = null;
                counterRect = null;
            }
            if (ActiveInstance == this) ActiveInstance = null;
        }

        /// <summary>
        /// Counters+ の設定に基づいてテキストオブジェクトを生成または再生成する。
        /// 既存のテキストがある場合は破棄して再作成する。
        /// </summary>
        private void CreateOrUpdateText()
        {
            if (counterText != null)
            {
                UnityEngine.Object.Destroy(counterText.gameObject);
                counterText = null;
                counterRect = null;
            }

            // Counters+ の既知のオーバーロードを直接呼び出す（フォールバックを行わない）
            counterText = canvasUtility.CreateTextFromSettings(Settings, Vector3.zero);

            // 見た目の設定
            counterText.fontSize = 3f;
            counterText.color = Color.white;
            counterText.alignment = TextAlignmentOptions.Center;
            counterText.enableWordWrapping = false;
            counterText.lineSpacing = -50;

            counterRect = counterText.GetComponent<RectTransform>();
            if (counterRect != null)
            {
                counterRect.sizeDelta = new Vector2(160f, 20f);
                // ユーザー設定のオフセットをデルタで適用
                counterRect.anchoredPosition
                    += new Vector2(PluginConfig.Instance.CounterXOffset,
                                   PluginConfig.Instance.CounterYOffset);
            }
        }
        
        /// <summary>
        /// ノートカットイベント。爆弾ノートのヒットを検出してカウントを増やす。
        /// </summary>
        public void OnNoteCut(NoteData data, NoteCutInfo info)
        {
            if (data.gameplayType == NoteData.GameplayType.Bomb)
                service?.IncrementBombHit();
        }

        /// <summary>
        /// ノートミスイベント（現状未使用）。
        /// </summary>
        public void OnNoteMiss(NoteData data) { /* 未使用 */ }

        /// <summary>
        /// 表示テキストを最新のカウント値に更新する。
        /// </summary>
        private void UpdateDisplay()
        {
            if (counterText == null || service == null) return;
            counterText.text = $"Walls: {service.WallHitCount}\nBombs: {service.BombHitCount}";
        }

        /// <summary>
        /// シーン内のすべての WallHitCounterUI にオフセットを適用します（BSML から呼ばれます）。
        /// 単一の ActiveInstance に対して再生成を実行する。
        /// </summary>
        public static void ApplyOffsetsToAll()
        {
            ActiveInstance?.CreateOrUpdateText();
        }
    }
}
