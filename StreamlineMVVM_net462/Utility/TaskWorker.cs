﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StreamlineMVVM
{
    // Wrapper class for TaskFactory wipped together to sort of simulate BackgroundWorker class but use the better Tasks technology.
    // Just mostly trying this out.
    public class TaskWorkerEventArgs
    {
        public object Parameters { get; set; }
        public object Results { get; set; }
        public bool CancellationRequested { get; set; } = false;
        public bool Cancelled { get; set; } = false;
        public bool Error { get; set; } = false;
    }

    public class TaskWorker : IDisposable
    {
        private Task task = null;
        private CancellationTokenSource source = null;
        private CancellationToken token;
        private TaskWorkerEventArgs taskWorkerEventArgs = new TaskWorkerEventArgs();

        public Action<object, TaskWorkerEventArgs> TaskAction { get; set; }
        public Action<object, TaskWorkerEventArgs> TaskComplete { get; set; }

        private readonly TimeSpan defaultTaskTimeout = TimeSpan.FromSeconds(0);
        public TimeSpan TaskTimeout { get; set; } = TimeSpan.FromSeconds(0);

        private async void setTaskTimeout(TimeSpan taskTimeout)
        {
            Task timeoutTask = null;
            try
            {
                timeoutTask = Task.Delay(taskTimeout);
                await timeoutTask;
            }
            catch (Exception Ex)
            {
                LogWriter.Exception("TaskWorker Error: Failed to run timeout task.", Ex);
            }
            finally
            {
                if (timeoutTask != null)
                {
                    timeoutTask.Dispose();
                    timeoutTask = null;
                }
            }

            CancelTask();
        }

        private bool _CancellationRequested = false;
        public bool CancellationRequested
        {
            get
            {
                return _CancellationRequested;
            }
        }

        private bool _IsBusy = false;
        public bool IsBusy
        {
            get
            {
                return _IsBusy;
            }
        }

        public void StartTask()
        {
            runTask(null, TaskTimeout);
        }

        public void StartTask(object paramaters)
        {
            runTask(paramaters, TaskTimeout);
        }

        public void StartTask(object paramaters, TimeSpan taskTimeout)
        {
            runTask(paramaters, taskTimeout);
        }

        private async void runTask(object paramaters, TimeSpan taskTimeout)
        {
            if (_IsBusy || TaskAction == null || TaskComplete == null)
            {
                // This should probrably throw or something else.
                return;
            }

            Dispose(); // If we are reusing an instance of this class, we want to clean it up first.

            if (TimeSpan.Compare(taskTimeout, defaultTaskTimeout) != 0)
            {
                source = new CancellationTokenSource(taskTimeout);
                setTaskTimeout(taskTimeout);
            }
            else
            {
                source = new CancellationTokenSource();
            }

            token = source.Token;
            taskWorkerEventArgs.Parameters = paramaters;
            _IsBusy = true;
            try
            {
                task = Task.Run(() => TaskAction(this, taskWorkerEventArgs), token);
                await task;
            }
            finally
            {
                // Nothing really needs to be here.
            }

            taskComplete();
        }

        public void CancelTask()
        {
            if (source != null)
            {
                try
                {
                    source.Cancel();
                }
                finally
                {
                    // Nothing really needs to be here.
                }
            }

            _CancellationRequested = true;
            taskWorkerEventArgs.Cancelled = true;
            taskComplete();
        }

        // Completed event that runs on the initial task start calling thread.
        public void taskComplete()
        {
            // If the IsBusy is false, we just want to fizzle out. This means the Task was cancelled already.
            if (IsBusy == false)
            {
                return;
            }
            _IsBusy = false;

            TaskComplete(this, taskWorkerEventArgs);
            Dispose();
        }

        // Although this class method internally cleans up it is still good to expose this method just in case.
        public void Dispose()
        {
            try
            {
                if (task != null)
                {
                    task.Dispose();
                    task = null;
                }

                if (source != null)
                {
                    source.Dispose();
                    source = null;
                }
            }
            catch
            {
                // The above may fail is the task is somehow dead locked. Will look into this more.
            }

            taskWorkerEventArgs = new TaskWorkerEventArgs();
        }
    }
}
