// Copyright 2017-2021 Elringus (Artyom Sovetnikov). All Rights Reserved.

using Naninovel.UI;
using UniRx.Async;

namespace Naninovel.Commands
{
    /// <summary>
    /// Shows a UI for general-purpose self-hiding popup notification (aka "toast")
    /// with the provided text and (optionally) appearance and duration.
    /// The UI is automatically hidden after the specified (or default) duration.
    /// </summary>
    /// <remarks>
    /// Appearance name is the name of a game object with `Toast Appearance`
    /// component inside the `ToastUI` UI prefab (case-insensitive).
    /// </remarks>
    [CommandAlias("toast")]
    public class ShowToastUI : Command
    {
        /// <summary>
        /// The text content to set for the toast.
        /// </summary>
        [ParameterAlias(NamelessParameterAlias)]
        public StringParameter Text;
        /// <summary>
        /// Appearance variant (game object name) of the toast.
        /// When not specified, will use default appearance set in Toast UI prefab.
        /// </summary>
        public StringParameter Appearance;
        /// <summary>
        /// Seconds to wait before hiding the toast.
        /// When not specified, will use duration set by default in Toast UI prefab.
        /// </summary>
        [ParameterAlias("time")]
        public DecimalParameter Duration;

        public override UniTask ExecuteAsync (CancellationToken cancellationToken = default)
        {
            var toastUI = Engine.GetService<IUIManager>().GetUI<IToastUI>();
            toastUI?.Show(Text, Appearance, Duration);
            return UniTask.CompletedTask;
        }
    }
}
