#if !SILVERLIGHT

namespace Caliburn.PresentationFramework.ApplicationModel
{
    using System;

    /// <summary>
    /// A service that manages windows.
    /// </summary>
    public interface IWindowManager
    {
        /// <summary>
        /// Shows a window for the specified model.
        /// </summary>
        /// <param name="rootModel">The root model.</param>
        /// <param name="context">The context.</param>
        /// <param name="handleShutdownModel">The handle shutdown model.</param>
        void Show(object rootModel, object context, Action<ISubordinate, Action> handleShutdownModel);
        
        /// <summary>
        /// Shows a modal dialog for the specified model.
        /// </summary>
        /// <param name="rootModel">The root model.</param>
        /// <param name="context">The context.</param>
        /// <param name="handleShutdownModel">The handle shutdown model.</param>
        /// <returns></returns>
        bool? ShowDialog(object rootModel, object context, Action<ISubordinate, Action> handleShutdownModel);
    }
}

#endif