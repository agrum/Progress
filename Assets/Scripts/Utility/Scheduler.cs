using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Utility
{
    public class Scheduler
    {
        SortedList<double, TaskCompletionSource<bool>> oldTasks = null;
        SortedList<double, TaskCompletionSource<bool>> tasks = new SortedList<double, TaskCompletionSource<bool>>();
        int oldCursor = 0;
        int cursor = 0;

        public double Time { get; private set; } = 0;

        public void Tick(double dt)
        {
            Time += 0;

            if (oldTasks != null)
            { 
                while (oldCursor < oldTasks.Count && oldTasks.Keys[oldCursor] <= Time)
                {
                    tasks[oldCursor].TrySetResult(true);
                    ++oldCursor;
                }

                if (oldCursor == oldTasks.Count)
                {
                    oldTasks = null;
                    oldCursor = 0;
                }
            }

            while (cursor < tasks.Count && tasks.Keys[cursor] <= Time)
            {
                tasks[cursor].TrySetResult(true);
                ++cursor;
            }

            if (oldTasks == null && cursor > 1000)
            {
                oldTasks = tasks;
                oldCursor = cursor;
                tasks = new SortedList<double, TaskCompletionSource<bool>>();
                cursor = 0;
            }
        }

        public async Task Wait(double duration)
        {
            await WaitUntil(Time + duration);
        }

        public async Task WaitUntil(double time)
        {
            if (time <= Time)
            {
                return;
            }
            var completion = new TaskCompletionSource<bool>();
            tasks.Add(time, completion);
            await completion.Task;
        }
    }
}
