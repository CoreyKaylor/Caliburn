﻿namespace Caliburn.ShellFramework.Resources
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Windows.Markup;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Extensions methods related to resources.
    /// </summary>
    public static class ResourceExtensions
    {
        internal static Assembly DefaultResourceAssembly = Assembly.GetExecutingAssembly();

        internal static Func<string, string> DetermineIconPath =
            displayName => "Resources/Icons/" + displayName.Replace(" ", string.Empty) + ".png";

        /// <summary>
        /// Gets the name of the assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>The name.</returns>
        public static string GetAssemblyName(this Assembly assembly)
        {
            string name = assembly.FullName;
            return name.Substring(0, name.IndexOf(','));
        }

        /// <summary>
        /// Gets the resource stream using the default resource assembly.
        /// </summary>
        /// <param name="resourceManager">The resource manager.</param>
        /// <param name="relativeUri">The relative URI.</param>
        /// <returns>The stream, or null if not found.</returns>
        public static Stream GetStream(this IResourceManager resourceManager, string relativeUri)
        {
            return resourceManager.GetStream(relativeUri, DefaultResourceAssembly.GetAssemblyName());
        }

        /// <summary>
        /// Gets the bitmap resource.
        /// </summary>
        /// <param name="resourceManager">The resource manager.</param>
        /// <param name="relativeUri">The relative URI.</param>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <returns>The bitmap, or null if not found.</returns>
        public static BitmapImage GetBitmap(this IResourceManager resourceManager, string relativeUri, string assemblyName)
        {
            var stream = resourceManager.GetStream(relativeUri, assemblyName);
            if (stream == null) return null;

            using (stream)
            {
                var bmp = new BitmapImage();

                bmp.BeginInit();
                bmp.StreamSource = stream;
                bmp.EndInit();
                bmp.Freeze();

                return bmp;
            }
        }

        /// <summary>
        /// Gets the bitmap resource from the default resource assembly.
        /// </summary>
        /// <param name="resourceManager">The resource manager.</param>
        /// <param name="relativeUri">The relative URI.</param>
        /// <returns>The bitmap, or null if not found.</returns>
        public static BitmapImage GetBitmap(this IResourceManager resourceManager, string relativeUri)
        {
            return resourceManager.GetBitmap(relativeUri, DefaultResourceAssembly.GetAssemblyName());
        }

        /// <summary>
        /// Gets the resource string.
        /// </summary>
        /// <param name="resourceManager">The resource manager.</param>
        /// <param name="relativeUri">The relative URI.</param>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <returns>The string, or null if not found.</returns>
        public static string GetString(this IResourceManager resourceManager, string relativeUri, string assemblyName)
        {
            var stream = resourceManager.GetStream(relativeUri, assemblyName);
            if (stream == null) return null;

            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Gets the resource string from the default resource assmebly.
        /// </summary>
        /// <param name="resourceManager">The resource manager.</param>
        /// <param name="relativeUri">The relative URI.</param>
        /// <returns>The string, or null if not found.</returns>
        public static string GetString(this IResourceManager resourceManager, string relativeUri)
        {
            return resourceManager.GetString(relativeUri, DefaultResourceAssembly.GetAssemblyName());
        }

        /// <summary>
        /// Gets the xaml object resource.
        /// </summary>
        /// <param name="resourceManager">The resource manager.</param>
        /// <param name="relativeUri">The relative URI.</param>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <returns>The xaml object, or null if not found.</returns>
        public static object GetXamlObject(this IResourceManager resourceManager, string relativeUri, string assemblyName)
        {
            var stream = resourceManager.GetStream(relativeUri, assemblyName);
            if (stream == null) return null;

            using(stream)
            {
                return XamlReader.Load(stream);
            }
        }

        /// <summary>
        /// Gets the xaml object resource from the default resource assembly.
        /// </summary>
        /// <param name="resourceManager">The resource manager.</param>
        /// <param name="relativeUri">The relative URI.</param>
        /// <returns>The xaml object, or null if not found.</returns>
        public static object GetXamlObject(this IResourceManager resourceManager, string relativeUri)
        {
            return resourceManager.GetXamlObject(relativeUri, DefaultResourceAssembly.GetAssemblyName());
        }
    }
}
