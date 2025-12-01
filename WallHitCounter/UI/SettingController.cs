using System;
using BeatSaberMarkupLanguage.Attributes;

namespace WallHitCounter.UI
{
    /// <summary>
    /// BSML から参照される設定コントローラー。
    /// PluginConfig を更新し、変更をカウンター表示へ即時反映する。
    /// </summary>
    [ViewDefinition("WallHitCounter.UI.SettingsUI.bsml")]
    public class SettingController
    {
        public SettingController()
        {
            Plugin.Log.Info("SettingController constructed");
        }

        /// <summary>
        /// カウンターの X オフセット（UI バインディング用）
        /// </summary>
        [UIValue("counter-x-offset")]
        public float CounterXOffset
        {
            get => PluginConfig.Instance.CounterXOffset;
            set
            {
                PluginConfig.Instance.CounterXOffset = value;
                // apply live to any active counters
                WallHitCounterUI.ApplyOffsetsToAll();
            }
        }

        /// <summary>
        /// カウンターの Y オフセット（UI バインディング用）
        /// </summary>
        [UIValue("counter-y-offset")]
        public float CounterYOffset
        {
            get => PluginConfig.Instance.CounterYOffset;
            set
            {
                PluginConfig.Instance.CounterYOffset = value;
                // apply live to any active counters
                WallHitCounterUI.ApplyOffsetsToAll();
            }
        }

        /// <summary>
        /// 小数点1桁でフォーマットした表示文字列を返すヘルパー
        /// </summary>
        [UIValue("FormatOneDecimal")]
        public string FormatOneDecimal(float value)
        {
            return value.ToString("F1"); // 小数点1桁で表示

        }
    }
}
