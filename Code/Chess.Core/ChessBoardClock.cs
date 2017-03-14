using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Core
{
    public class ChessBoardClock:IDisposable
    {
        public event EventHandler<ChessClockEventArgs> OnClockStart;
        public event EventHandler<ChessClockEventArgs> OnClockStop;
        public event EventHandler<ChessClockEventArgs> OnTimeFinish;

        #region FIELDS
        bool disposed = false;
        private TimeSpan _spentTime;
        private TimeSpan _leftTime;
        #endregion

        #region THREAD SAFE PROPERTIES
        /// <summary>
        /// Represent the total time available for the player for the complete game. [Thread Safe]        
        /// </summary>
        public TimeSpan SpentTime
        {
            get
            {
                lock (ClockTickMonitor)
                {
                    return _spentTime;
                }
            }
            private set
            {
                lock (ClockTickMonitor)
                {
                    _spentTime = value;
                }
            }
        }

        
        /// <summary>
        /// Represent the total remaining time until the clock reach zero. [Thread Safe]        
        /// </summary>
        public TimeSpan LeftTime
        {
            get
            {
                lock (ClockTickMonitor)
                {
                    return _leftTime;
                }
            }
            private set
            {
                lock (ClockTickMonitor)
                {
                    _leftTime = value;
                }
            }
        }


        #endregion

        #region NON THREAD SAFE PROPERTIES
        /// <summary>
        /// Indicates when was the last time when the Start method was called
        /// No ThreadSafe, updated only in start method, that is noth thread safe and we don't need thread safe to start a clock.
        /// 
        /// </summary>
        public DateTime? LastTurnStartAt { get; set; }               
        public TimeSpan TotalTime { get; set; }
        public ChessPlayer Player { get; set; }
        #endregion


        /// <summary>
        /// Simple Clock that Count the seconds that the clock was active
        /// </summary>
        System.Threading.Timer myClock;

        #region MONITORS LOCKERS
        /// <summary>
        /// Reentrant Locker: Using a reentrant locker alogn the entire class, 
        /// Used to provide a thread safe operation updating the total time.
        /// </summary>
        private static readonly object ClockTickMonitor = new object();        
        #endregion


        private ClockState _state;
        public ClockState State {
            get {
                lock (ClockTickMonitor)
                {
                    return _state;
                }
            }
            private set {
                lock (ClockTickMonitor)
                {
                    _state = value;
                }
            }
                
        }

        public void InitClock(TimeSpan totalTime)
        {
            TotalTime = totalTime;
        }

        public void Start()
        {
            if (myClock == null)
            {
                LastTurnStartAt = DateTime.UtcNow;
                myClock = new System.Threading.Timer(ClockTick, null, 0, 1000);
                OnClockStart?.Invoke(this, new ChessClockEventArgs(Player));
            }
        }

        public void Stop()
        {
            lock (ClockTickMonitor)
            {
                myClock.Dispose();
                myClock = null;
                LastTurnStartAt = null;
                OnClockStop?.Invoke(this, new ChessClockEventArgs(Player));
            }
        }

        public void ClockTick(object obj)
        {
            lock(ClockTickMonitor)
            {
                if(LastTurnStartAt.HasValue)
                {
                    SpentTime = DateTime.UtcNow.Subtract(LastTurnStartAt.Value);
                    LeftTime = TotalTime - SpentTime;
                    if (LeftTime.TotalSeconds <= 0)
                    {
                        Stop();
                        this.State = ClockState.Finished;
                        OnTimeFinish?.Invoke(this, new ChessClockEventArgs(Player)); //WARNING: Event Raised in a background thread.
                    }
                }                
            }
        }

        // Public implementation of Dispose pattern callable by consumers.
        // https://msdn.microsoft.com/en-us/library/system.idisposable(v=vs.110).aspx
        //
        // A base class with subclasses that should be disposable must implement 
        // IDisposable as follows. You should use this pattern whenever you implement
        // IDisposable on any type that isn't sealed 
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                this.myClock.Dispose();
                // Free any other managed objects here.
                //
            }
            this.myClock = null;
            // Free any unmanaged objects here.
            //
            disposed = true;
        }
    }

    public class ChessClockEventArgs : EventArgs
    {
        public ChessPlayer Player { get; set; }
        public ChessClockEventArgs(ChessPlayer player)
        {
            Player = player;
        }
    }

}
