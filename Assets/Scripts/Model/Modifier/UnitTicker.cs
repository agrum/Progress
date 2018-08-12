using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Model
{
    class UnitTicker
    {
        List<ModifierTicker> ModifierTickerList = new List<ModifierTicker>();
        List<TickOperation> ScheduledTickOperationList = new List<TickOperation>();
        SortedList<float, TickOperation> PendingNewTickOperationList = new SortedList<float, TickOperation>();

        public void ListensTo(Modifier modifier_)
        {
            if (modifier_.TickPeriod == 0)
                return;

            var modifierTicker = modifier_.GetOperator<ModifierTicker>();
            if (modifierTicker == null)
                return;

            OnScheduleTickOperation(modifierTicker.NextOperation);
            modifierTicker.ScheduleTickOperation += OnScheduleTickOperation;
        }

        public void Tick(float time_)
        {
            //tick throught the scheduled operations
            int processedCount = 0;
            foreach (var scheduledTickOperation in ScheduledTickOperationList)
            {
                if (scheduledTickOperation.Active)
                {
                    if (scheduledTickOperation.NextTickTime <= time_)
                    {
                        //call tick with the precisie dt (not the desired period, can't guarantee it
                        //HERE, TICK IS HERE
                        scheduledTickOperation.Visit();
                        //HERE, TICK IS HERE

                        //schedule next tick from current time
                        if (scheduledTickOperation.Active)
                            PendingNewTickOperationList.Add(
                                scheduledTickOperation.NextTickTime,
                                scheduledTickOperation);
                    }
                    else
                        break;
                }
                ++processedCount;
            }

            //remove dead modifiers
            ScheduledTickOperationList.RemoveRange(0, processedCount);

            //add new schedules
            int destinationIndex = 0;
            foreach (var nextScheduledTickOperation in PendingNewTickOperationList)
            {
                while (destinationIndex < ScheduledTickOperationList.Count
                    && ScheduledTickOperationList[destinationIndex].NextTickTime < nextScheduledTickOperation.Key)
                    ++destinationIndex;

                ScheduledTickOperationList.Insert(destinationIndex, nextScheduledTickOperation.Value);
                ++destinationIndex;
            }

            PendingNewTickOperationList.Clear();
        }

        private void OnScheduleTickOperation(TickOperation tickOperation_)
        {
            PendingNewTickOperationList.Add(tickOperation_.NextTickTime, tickOperation_);
        }
    }
}
