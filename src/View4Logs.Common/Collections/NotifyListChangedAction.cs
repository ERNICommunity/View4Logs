namespace View4Logs.Common.Collections
{
    /// <summary>
    /// Describes the action that caused a <see cref="INotifyListChanged{T}.ListChanged"/> event.
    /// </summary>
    public enum NotifyListChangedAction
    {
        /// <summary>
        /// One or more items has been appended to the list.
        /// </summary>
        Add,

        /// <summary>
        /// Content of the list was changed.
        /// Use this for any modification which is not <see cref="Add"/> action.
        /// </summary>
        Reset
    }
}
