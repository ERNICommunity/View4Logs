using System;
using System.Windows;

namespace View4Logs.Theme
{
    /// <summary>
    /// Resource dictionary for theming support.
    /// </summary>
    /// <remarks>
    /// Use this as a base class for resources where there are multiple variants (themes).
    /// Do not load the resource dictionary directly.
    /// Instead group them by <see cref="Category"/> and dynamically add one to the Application resources.
    /// </remarks>
    public abstract class ThemeResourceDictionary : ResourceDictionary
    {
        public string Name { get; set; }

        public Type Category { get; set; }

        public bool IsDefault { get; set; }
    }
}
