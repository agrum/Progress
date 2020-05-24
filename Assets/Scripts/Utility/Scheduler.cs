using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Utility
{
    public class Scheduler : MonoBehaviour
    {
        //delegate IEnumerator Event();
        //SortedList<double, event Event> oldTasks = null;
        //SortedList<double, TaskCompletionSource<bool>> tasks = new SortedList<double, TaskCompletionSource<bool>>();
        //int oldCursor = 0;
        //int cursor = 0;

        public double Creation { get; private set; } = Time.time;
        public double Now
        {
            get
            {
                return Time.time - Creation;
            }
        }

        //public void Tick(double dt)
        //{
        //    Time += 0;

        //    if (oldTasks != null)
        //    { 
        //        while (oldCursor < oldTasks.Count && oldTasks.Keys[oldCursor] <= Time)
        //        {
        //            tasks[oldCursor].TrySetResult(true);
        //            ++oldCursor;
        //        }

        //        if (oldCursor == oldTasks.Count)
        //        {
        //            oldTasks = null;
        //            oldCursor = 0;
        //        }
        //    }

        //    while (cursor < tasks.Count && tasks.Keys[cursor] <= Time)
        //    {
        //        tasks[cursor].TrySetResult(true);
        //        ++cursor;
        //    }

        //    if (oldTasks == null && cursor > 1000)
        //    {
        //        oldTasks = tasks;
        //        oldCursor = cursor;
        //        tasks = new SortedList<double, TaskCompletionSource<bool>>();
        //        cursor = 0;
        //    }
        //}

        public IEnumerator Wait(double duration)
        {
            yield return new WaitForSeconds((float) duration);
        }

        public IEnumerator WaitUntil(double time)
        {
            if (time <= Time.time - Creation)
            {
                yield break;
            }
            Wait(time - Time.time - Creation);
            //var completion = new TaskCompletionSource<bool>();
            //tasks.Add(time, completion);
            //await completion.Task;
        }
    }
}
