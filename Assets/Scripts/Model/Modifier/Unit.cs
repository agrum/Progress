using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Model
{
    public class ScheduledModifierTick
    {
        public float Time { get; private set; }
        public Modifier Modifier { get; private set; }

        public ScheduledModifierTick(float time_, Modifier modifier_)
        {
            Time = time_;
            Modifier = modifier_;
        }
    }

    public class Unit
    {
        List<Modifier> ModifierList = new List<Modifier>();
        List<ScheduledModifierTick> ScheduledModifierTickList = new List<ScheduledModifierTick>();

        public void ApplyModifier(Modifier modifier_)
        {
            ModifierList.Add(modifier_);

            if (modifier_.TickPeriod > 0)
            {

            }
        }

        public void RemoveModifier(Modifier sourceModifier_, string idName_)
        {
            int index = 0;
            foreach (var modifier in ModifierList)
            {
                if (modifier.SourceModifier == sourceModifier_ && modifier.IdName == idName_)
                {
                    modifier.Kill();
                    ModifierList.RemoveAt(index);
                    break;
                }
                ++index;
            }
        }

        void Tick(float time_)
        {
            int processedCount = 0;
            SortedList<float, Modifier> nextScheduledModifierTickList = new SortedList<float, Modifier>();

            //tick throught all attached modifiers and kill the expired ones
            foreach (var modifier in ModifierList)
                if (modifier.ExpirationTime <= time_)
                    modifier.Kill();

            //tick throught the scheduled operations
            foreach (var scheduledModifierTick in ScheduledModifierTickList)
            {
                if (!scheduledModifierTick.Modifier.ToDestroy)
                {
                    if (scheduledModifierTick.Time <= time_)
                    {
                        //call tick with the precisie dt (not the desired period, can't guarantee it
                        //HERE, TICK IS HERE
                        float tickTime = scheduledModifierTick.Time;
                        while (tickTime <= time_)
                        {
                            scheduledModifierTick.Modifier.Tick();
                            tickTime += scheduledModifierTick.Modifier.TickPeriod;
                        }
                        //HERE, TICK IS HERE

                        //schedule next tick from current time
                        if (scheduledModifierTick.Modifier.Ticks)
                            nextScheduledModifierTickList.Add(
                                tickTime,
                                scheduledModifierTick.Modifier);
                    }
                    else
                        break;
                }
                ++processedCount;
            }

            //remove dead modifiers
            while (processedCount > 0)
                ScheduledModifierTickList.RemoveAt(--processedCount);

            //add new schedules
            int destinationIndex = 0;
            foreach (var nextScheduledModifierTick in nextScheduledModifierTickList)
            {
                while (destinationIndex < ScheduledModifierTickList.Count 
                    && ScheduledModifierTickList[destinationIndex].Time < nextScheduledModifierTick.Key)
                    ++destinationIndex;

                ScheduledModifierTickList.Insert(destinationIndex, new ScheduledModifierTick(nextScheduledModifierTick.Key, nextScheduledModifierTick.Value));
                ++destinationIndex;
            }
        }
    }
}
