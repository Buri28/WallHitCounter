namespace WallHitCounter
{
    /// <summary>
    /// プラグイン設定クラス
    /// </summary>
    public class PluginConfig
    {
        public static PluginConfig Instance { get; internal set; }
        public virtual float CounterXOffset { get; set; } = 0.0f;
        public virtual float CounterYOffset { get; set; } = 0.0f;
    }
}
