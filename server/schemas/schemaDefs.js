var defs = {};

defs.numericValue = {};
defs.numericValue.types = [
	"Reference",
	"Flat",
]
defs.numericValue.methods = [
	"Add",
	"Multiply",
]
defs.numericValue.upgradeTypes = {};
defs.numericValue.upgradeTypes.Inc = "Inc";
defs.numericValue.upgradeTypes.Dec = "Inc";
defs.numericValue.upgradeTypesArray = [
	defs.numericValue.upgradeTypes.Inc,
	defs.numericValue.upgradeTypes.Dec,
]
defs.numericValue.referenceTargets = [
	"Self",
	"Target",
	"Cursor",
	"Parent",
	"Children",
]
defs.unitMetrics = [
	"Life",
	"Health",
	"Shield",
	"Armor",
	"Stamina",
	"Maximum Life",
	"Maximum Health",
	"Maximum Shield",
	"Maximum Armor",
	"Maximum Stamina",
	"Life Fraction",
	"Health Fraction",
	"Shield Fraction",
	"Armor Fraction",
	"Stamina Fraction",
	"Speed",
	"Vision",
	"Size",
	"Damage Mitigation",
	"Ailment Mitigation",
	"Attack Rate",
	"Attack Range",
	"Attack Damage",
	"Stamina Recovery Rate",
	"Stamina Recovery Wait",
	"Distance",
	"Is Active",
	"Is Self",
	"Is Ally",
	"Is Player"
]
defs.modifier = {}
defs.modifier.stackMethod = {}
defs.modifier.stackMethod.Cumulative = "Cumulative"
defs.modifier.stackMethod.DiminishingReturn = "Diminishing Return"
defs.modifier.stackMethod.One = "One"
defs.modifier.stackMethodArray = [
	defs.modifier.stackMethod.Cumulative,
	defs.modifier.stackMethod.DiminishingReturn,
	defs.modifier.stackMethod.One,
]
defs.modifier.stackRefresh = {}
defs.modifier.stackRefresh.RefreshIfSameSourceOrStronger = "Refresh If Same Source Or Stronger"
defs.modifier.stackRefresh.FullRefresh = "Full Refresh"
defs.modifier.stackRefresh.NoRefresh = "No Refresh"
defs.modifier.stackRefresh = [
	defs.modifier.stackRefresh.RefreshIfSameSourceOrStronger,
	defs.modifier.stackRefresh.FullRefresh,
	defs.modifier.stackRefresh.NoRefresh,
]
defs.projectile = {}
defs.projectile.direction = [
	"Self's Direction",
	"Target's Direction",
	"Self To Target",
	"Target To Self",
]
defs.projectile.shape = [
	"Circle",
	"5To1", //laser
	"3To1",
	"1To1",
	"1To3",
	"1To5", //wall
]
defs.projectile.collideWith = [
	"All",
	"Ally", 
	"Ally Player",
	"Enemy",
	"Enemy Player",
	"Environment", 
	"Projectile", 
]
defs.trigger.InputSkillDown = "Input Skill Down"
defs.trigger.InputSkillUp = "Input Skill Up"
defs.trigger.InputMove = "Input Move"
defs.trigger.Initialized = "Initialized"
defs.trigger.Destroyed = "Destroyed"
defs.trigger.UnitCreated = "Unit Created"
defs.trigger.UnitDestroyed = "Unit Destroyed"
defs.trigger.ModifierApplied = "Modifier Applied"
defs.trigger.ModifierRemoved = "Modifier Removed"
defs.trigger.Tick = "Tick"
defs.trigger.Entered = "Entered"
defs.trigger.Left = "Left"
defs.trigger.HitDealt = "Hit Dealt"
defs.trigger.HitReceived = "Hit Received"
defs.trigger.StatusChanged = "Status Changed"
defs.triggers = [
	defs.trigger.InputSkillDown, //skill, number
	defs.trigger.InputSkillUp, //skill, number
	defs.trigger.InputMove, //lookDirection, moveDirection
	defs.trigger.Initialized, 
	defs.trigger.Destroyed,
	defs.trigger.UnitCreated, //unit
	defs.trigger.UnitDestroyed, //unit
	defs.trigger.ModifierApplied, //modifier
	defs.trigger.ModifierRemoved, //modifier
	defs.trigger.Tick, //dt
	defs.trigger.Entered, //unit
	defs.trigger.Left, //unit
	defs.trigger.HitDealt, //projectile, sourceUnit, destinationUnit
	defs.trigger.HitReceived, //projectile, sourceUnit, destinationUnit
	defs.trigger.StatusChanged, //casting, stunned, knocked, etc... ON/OFF
]
defs.action.AffectMetric = "Affect Metric"
defs.action.ApplyModifier = "Apply Modifier"
defs.action.CreateUnit = "Create Unit"
defs.action.AddStack = "Add Stack"
defs.action.RemoveStack = "Remove Stack"
defs.action.Trigger = "Trigger"
defs.action.ReapeatTrigger = "Reapeat Trigger"
defs.action.Destroy = "Destroy"
defs.action.AcquireTransform = "Acquire Transform"
defs.action.ReleaseTransform = "Release Transform"
defs.actions = [
	defs.action.AffectMetric, //must remain 0
	defs.action.ApplyModifier, //must remain 1
	defs.action.CreateUnit, // must remain 2
	defs.action.AddStack,
	defs.action.RemoveStack,
	defs.action.Trigger,
	defs.action.ReapeatTrigger,
	defs.action.Destroy,
	defs.action.AcquireTransform,
	defs.action.ReleaseTransform,
]


module.exports = defs