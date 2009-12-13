﻿#if !SILVERLIGHT

namespace Caliburn.PresentationFramework.ApplicationModel
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Windows;
    using Configuration;
    using Core;
    using Core.Configuration;
    using Core.IoC;
    using Microsoft.Practices.ServiceLocation;

    /// <summary>
    /// A base class for applications based on Caliburn.
    /// </summary>
    public class CaliburnApplication : Application
    {
        private readonly IServiceLocator _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="CaliburnApplication"/> class.
        /// </summary>
        /// <exception cref="T:System.InvalidOperationException">
        /// More than one instance of the <see cref="T:System.Windows.Application"/> class is created per <see cref="T:System.AppDomain"/>.
        /// </exception>
        public CaliburnApplication()
        {
			BeforeConfiguration();

            _container = CreateContainer();

            var builder = CaliburnFramework
                .Configure(_container, Register);

            builder.With.Assemblies(SelectAssemblies());

            ConfigureCore(builder.With.Core());
            ConfigurePresentationFramework(builder.With.PresentationFramework());

            builder.Start();
        }

        /// <summary>
        /// Gets the container.
        /// </summary>
        /// <value>The container.</value>
        public IServiceLocator Container
        {
            get { return _container; }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Startup"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.StartupEventArgs"/> that contains the event data.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var rootModel = CreateRootModel();
            if (rootModel == null) return;

            ShowMainWindow(rootModel);
        }

        /// <summary>
        /// Shows the main window based on the provided model.
        /// </summary>
        /// <param name="rootModel">The root model.</param>
        protected virtual void ShowMainWindow(object rootModel)
        {
            Container.GetInstance<IWindowManager>()
                .Show(rootModel, null, ExecuteShutdownModel);
        }

        /// <summary>
        /// Executes the shutdown model.
        /// </summary>
        /// <param name="subordinate">The subordinate.</param>
        /// <param name="completed">The completed.</param>
        protected virtual void ExecuteShutdownModel(ISubordinate subordinate, Action completed)
        {
            completed();
        }

        /// <summary>
        /// Creates the root application model.
        /// </summary>
        /// <returns></returns>
        protected virtual object CreateRootModel()
        {
            return null;
        }

        /// <summary>
        /// Creates the container.
        /// </summary>
        /// <returns></returns>
        protected virtual IServiceLocator CreateContainer()
        {
            return new SimpleContainer();
        }

        /// <summary>
        /// Configures Caliburn's components.
        /// </summary>
        /// <param name="registrations">The component registrations.</param>
        protected virtual void Register(IEnumerable<IComponentRegistration> registrations)
        {
            var registry = _container as IRegistry;

            if (registry == null)
                throw new CaliburnException(
                    "Cannot configure Caliburn. Override Register or provide an IServiceLocator that also implements IRegistry."
                    );

            registry.Register(registrations);
        }

        /// <summary>
        /// Selects the assemblies which Caliburn will be able to inspect for components, views, etc.
        /// </summary>
        /// <returns></returns>
        protected virtual Assembly[] SelectAssemblies()
        {
            return new[] {Assembly.GetEntryAssembly()};
        }

		/// <summary>
		/// Called before the Caliburn initialization phase. 
		/// </summary>
		protected virtual void BeforeConfiguration() { }

        /// <summary>
        /// Configures the presentation framework module.
        /// </summary>
        /// <param name="module">The module.</param>
        protected virtual void ConfigurePresentationFramework(PresentationFrameworkConfiguration module) {}

        /// <summary>
        /// Configures the core module.
        /// </summary>
        /// <param name="module">The module.</param>
        protected virtual void ConfigureCore(CoreConfiguration module) { }
    }
}

#endif