using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Model
{
    public class ModifierDetector : IModifierOperator
    {
        public event OnTriggerCreated TriggerCreated;

        float detectionRadius;
        Unit unit;
        Dictionary<object, Unit> unitInRadiusMap = new Dictionary<object, Unit>();

        ModifierDetector(Unit unit_, float detectionRadius_)
        {
            unit = unit_;
            detectionRadius = detectionRadius_;
        }

        public void Update()
        {
            if (detectionRadius == -1)
                return;

            List<Unit> leaveUnitList = new List<Unit>();
            //trigger leave on lost units
            foreach (var almostUnit in unitInRadiusMap)
            {
                var unit = almostUnit.Value;
                if ((unit.Position - unit.Position).sqrMagnitude > detectionRadius)
                {
                    TriggerCreated(new Trigger(Trigger.EType.UntiLeave, unit));
                    leaveUnitList.Add(unit);
                }
            }

            //remove lsot units
            foreach (var unit in leaveUnitList)
                unitInRadiusMap.Remove(unit);

            //trigger enter on new units
            List<Unit> unitInRadiusList = new List<Unit>();
            if (unitInRadiusMap.Count != unitInRadiusList.Count)
            {
                foreach (var unit in unitInRadiusList)
                {
                    if (!unitInRadiusMap.ContainsKey(unit))
                    {
                        unitInRadiusMap.Add(unit, unit);
                        TriggerCreated(new Trigger(Trigger.EType.UnitEnter, unit));
                    }
                }
            }
        }
    }
}
