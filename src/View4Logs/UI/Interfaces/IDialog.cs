using System;

namespace View4Logs.UI.Interfaces
{
    /// <summary>
    /// User interface dialog.
    /// </summary>
    public interface IDialog : IDisposable
    {
        /// <summary>
        /// Observable sequence of dialog output values.        
        /// </summary>
        /// <remarks>
        /// Dialog will be closed when sequence completes.
        /// Dialog might return zero or more values.
        /// </remarks>
        IObservable<object> Result { get; }
    }


    /// <inheritdoc />
    public interface IDialog<out TResult> : IDialog
    {
        /// <summary>
        /// Observable sequence of dialog output values.        
        /// </summary>
        /// <remarks>
        /// Dialog will be closed when sequence completes.
        /// Dialog might return zero or more values.
        /// </remarks> 
        new IObservable<TResult> Result { get; }
    }
}
