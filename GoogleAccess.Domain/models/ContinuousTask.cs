using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Web;


namespace GoogleAccess.Domain.Models
{
    /// <summary>
    /// A generic class that continuously fetches data from a service at specified intervals.
    /// </summary>
    /// <typeparam name="T">The type of the fetcher service that implements IFetcherAsync</typeparam>
    /// <typeparam name="TResult">The type of the data being fetched</typeparam>
    public class ContinuousTask<T, TResult> : IDisposable where T : IFetcherAsync<TResult>
    {
        private readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);
        private readonly T _service;
        private readonly System.Timers.Timer _timer;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private TResult _result;
        private bool _isDisposed;

        /// <summary>
        /// Initializes a new instance of the ContinuousTask class.
        /// </summary>
        /// <param name="service">The service that implements IFetcherAsync to fetch data from</param>
        /// <param name="intervalMilliseconds">The interval in milliseconds between fetches</param>
        /// <exception cref="ArgumentNullException">Thrown when service is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when intervalMilliseconds is less than or equal to 0</exception>
        public ContinuousTask(T service, int intervalMilliseconds)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            if (intervalMilliseconds <= 0)
                throw new ArgumentOutOfRangeException(nameof(intervalMilliseconds), "Interval must be greater than 0");

            _cancellationTokenSource = new CancellationTokenSource();
            _timer = new System.Timers.Timer(intervalMilliseconds);
            _timer.Elapsed += async (sender, e) => await ExecuteAsync();
            _timer.Start();
        }

        /// <summary>
        /// Gets the current result of the last successful fetch operation.
        /// </summary>
        public TResult Result => _result;

        /// <summary>
        /// Gets whether the task is currently running.
        /// </summary>
        public bool IsRunning => _timer.Enabled;

        /// <summary>
        /// Stops the continuous task.
        /// </summary>
        public void Stop()
        {
            _timer.Stop();
            _cancellationTokenSource.Cancel();
        }

        /// <summary>
        /// Starts the continuous task.
        /// </summary>
        public void Start()
        {
            _cancellationTokenSource.TryReset();
            _timer.Start();
        }

        /// <summary>
        /// Forces an immediate fetch of data.
        /// </summary>
        /// <returns>A task representing the fetch operation</returns>
        public async Task<TResult> FetchNowAsync()
        {
            return await ExecuteAsync();
        }

        private async Task<TResult> ExecuteAsync()
        {
            if (_cancellationTokenSource.Token.IsCancellationRequested)
                return _result;

            try
            {
                await _lock.WaitAsync(_cancellationTokenSource.Token);
                var newResult = await _service.FetchDataAsync();
                _result = newResult;
                return newResult;
            }
            catch (OperationCanceledException)
            {
                return _result;
            }
            catch (Exception ex)
            {
                // Log the error but don't throw - we want to keep the last successful result
                // You might want to add proper logging here
                return _result;
            }
            finally
            {
                if (!_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    _lock.Release();
                }
            }
        }

        /// <summary>
        /// Releases all resources used by the ContinuousTask.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the ContinuousTask and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed) return;

            if (disposing)
            {
                _timer.Stop();
                _timer.Dispose();
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
                _lock.Dispose();
            }

            _isDisposed = true;
        }

        ~ContinuousTask()
        {
            Dispose(false);
        }
    }
}
