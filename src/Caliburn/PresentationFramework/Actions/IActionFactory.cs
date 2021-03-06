namespace Caliburn.PresentationFramework.Actions
{
    /// <summary>
    /// A service responsible for creating an <see cref="IAction"/>.
    /// </summary>
    public interface IActionFactory
    {
        /// <summary>
        /// Creates an <see cref="IAction"/> using the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The <see cref="IAction"/>.</returns>
        IAction Create(ActionCreationContext context);
    }
}