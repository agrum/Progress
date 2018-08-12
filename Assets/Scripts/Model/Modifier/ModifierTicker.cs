using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Model
{
    public class TickOperation
    {
        public bool Active = true;
        public float NextTickTime { get; private set; }

        private ModifierTicker Ticker;

        public TickOperation(ModifierTicker ticker_)
        {
            Ticker = ticker_;
            NextTickTime += Ticker.TickPeriod;
        }

        public void Visit()
        {
            while (Active && Time.time > NextTickTime)
            {
                Ticker.Update();
                NextTickTime += Ticker.TickPeriod;
            }
        }
    }

    public class ModifierTicker : IModifierOperator
    {
        public delegate void OnScheduleTickOperation(TickOperation tickOperation_);

        public event OnTriggerCreated TriggerCreated = delegate { };
        public event OnScheduleTickOperation ScheduleTickOperation = delegate { };

        public delegate void OnPauseTicker();
        public delegate void OnResetTicker();
        
        public float TickPeriod { get; private set; }
        public TickOperation NextOperation { get; private set; }

        ModifierTicker(float tickPeriod_)
        {
            TickPeriod = tickPeriod_;
            ResetTicker();
        }

        ~ModifierTicker()
        {
            TriggerCreated = null;
            ScheduleTickOperation = null;
        }

        public void Update()
        {
            TriggerCreated(new Trigger(Trigger.EType.Tick, null));
        }

        public void PauseTicker()
        {
            NextOperation.Active = false;
        }

        public void ResetTicker()
        {
            if (NextOperation != null)
                NextOperation.Active = false;

            NextOperation = new TickOperation(this);
            ScheduleTickOperation(NextOperation);
        }
    }
}
