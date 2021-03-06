﻿namespace Caliburn.PresentationFramework.Conventions
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using Core;
    using ViewModels;
    using Views;

    /// <summary>
    /// Hosts extension methods related to conventions.
    /// </summary>
    public static class ConventionExtensions
    {
        internal static Func<IConventionManager, DependencyObject, IEnumerable<ElementDescription>> SelectElementsToInspect = DefaultSelectElementsToInspectImplementation;

        /// <summary>
        /// Determines if the provided dependency property has a binding on the dependency object.
        /// </summary>
        /// <param name="dependencyObject">The object instance.</param>
        /// <param name="dependencyProperty">The property to check.</param>
        /// <returns>true if a binding exists; false otherwise</returns>
        public static bool HasBinding(this DependencyObject dependencyObject, DependencyProperty dependencyProperty)
        {
            return BindingOperations.GetBindingExpression(dependencyObject, dependencyProperty) != null;
        }

        /// <summary>
        /// Determines the conventions for the specified view and view model description.
        /// </summary>
        /// <param name="conventionManager">The convention manager.</param>
        /// <param name="viewModelDescription">The view model description.</param>
        /// <param name="view">The view.</param>
        /// <returns>The applicalble conventions.</returns>
        public static IEnumerable<IViewApplicable> DetermineConventions(this IConventionManager conventionManager, IViewModelDescription viewModelDescription, DependencyObject view)
        {
            return conventionManager.DetermineConventions(viewModelDescription, SelectElementsToInspect(conventionManager, view));
        }

#if !SILVERLIGHT
        private static IEnumerable<ElementDescription> DefaultSelectElementsToInspectImplementation(IConventionManager conventionManager, DependencyObject root)
        {
            var queue = new Queue<DependencyObject>();
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                var currentName = current.GetName();

                if (!string.IsNullOrEmpty(currentName))
                {
                    var currentType = current.GetType();
                    var currentConvention = conventionManager.GetElementConvention(currentType);

                    if (currentConvention != null)
                        yield return new ElementDescription { Type = currentType, Name = currentName, Convention = currentConvention };
                }

                foreach (object child in LogicalTreeHelper.GetChildren(current))
                {
                    var childDo = child as DependencyObject;

                    if (childDo == null || childDo is UserControl)
                        continue;

                    queue.Enqueue(childDo);
                }
            }
        }
#else
        private static IEnumerable<ElementDescription> DefaultSelectElementsToInspectImplementation(IConventionManager conventionManager, DependencyObject root)
        {
            var queue = new Queue<DependencyObject>();
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                var currentName = current.GetName();

                if (!string.IsNullOrEmpty(currentName))
                {
                    var currentType = current.GetType();
                    var currentConvention = conventionManager.GetElementConvention(currentType);

                    if (currentConvention != null)
                        yield return new ElementDescription { Type = currentType, Name = currentName, Convention = currentConvention };
                }

                var childCount = VisualTreeHelper.GetChildrenCount(current);
                if (childCount > 0)
                {
                    for(var i = 0; i < childCount; i++)
                    {
                        var childDo = VisualTreeHelper.GetChild(current, i);

                        if(childDo is UserControl)
                            continue;

                        queue.Enqueue(childDo);
                    }
                }
                else
                {
                    var contentControl = current as ContentControl;
                    if (contentControl != null)
                    {
                        if (contentControl.Content != null
                            && contentControl.Content is DependencyObject
                            && !(contentControl.Content is UserControl))
                            queue.Enqueue(contentControl.Content as DependencyObject);
                    }
                    else
                    {
                        var itemsControl = current as ItemsControl;
                        if(itemsControl != null)
                        {
                            itemsControl.Items.OfType<DependencyObject>()
                                .Where(item => !(item is UserControl))
                                .Apply(queue.Enqueue);
                        }
                    }
                }
            }
        }
#endif

        /// <summary>
        /// Finds the interaction defaults or fail.
        /// </summary>
        /// <param name="conventionManager">The convention manager.</param>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public static IElementConvention FindElementConventionOrFail(this IConventionManager conventionManager, object element)
        {
            var type = element.GetType();
            var defaults = conventionManager.GetElementConvention(type);

            if (defaults == null)
                throw new CaliburnException(
                    string.Format("Could not locate an IElementConvention for {0}.  Please register one with the IConventionManager.", type.Name)
                    );

            return defaults;
        }
    }
}
