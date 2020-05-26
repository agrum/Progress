using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Utility
{
    public class Scheduler
    {
        MonoBehaviour parent;
        public delegate IEnumerator Coroutine();

        public double Creation { get; private set; } = 0;
        public double Now
        {
            get
            {
                return Time.time - Creation;
            }
        }

        public Scheduler(MonoBehaviour parent_)
        {
            parent = parent_;
            Creation = Time.time;
        }

        public void Start(Coroutine process, ref IEnumerator coroutine)
        {
            if (coroutine != null)
            {
                parent.StopCoroutine(coroutine);
            }
            coroutine = process();
            parent.StartCoroutine(coroutine);
        }

        public void Stop(ref IEnumerator coroutine)
        {
            if (coroutine != null)
            {
                parent.StopCoroutine(coroutine);
                coroutine = null;
            }
        }

        public IEnumerator Wait(double duration)
        {
            yield return new WaitForSeconds((float) duration);
        }

        public IEnumerator WaitUntil(double time)
        {
            if (time <= Now)
            {
                yield break;
            }
            yield return Wait(time - Now);
        }
    }
}
