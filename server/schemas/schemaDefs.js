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
defs.numericValue.referenceTargets = [
	"Self",
	"Target",
	"Parent",
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
defs.modifier = {};
defs.modifier.stackMethod = [
	"Cumulative",
	"Diminishing Return",
	"One",
]
defs.modifier.stackRefresh = [
	"Refresh If Same Source Or Stronger",
	"Full Refresh",
	"No Refresh",
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
defs.triggers = [
	"Input Skill Down",
	"Input Skill Up",
	"Input Move",
	"Create",
	"Destroy",
	"Tick",
	"Get Closer",
	"Get Farther",
	"Death",
	"Hit Dealt",
	"Hit Received",
	"Collide",
	"Status Changed", //casting, stunned, knocked, etc... ON/OFF
	"Unit Create",
]
defs.actions = [
	"Affect Metric", //must remain 0
	"Apply Modifier", //must remain 1
	"Create Unit", // must remain 2
	"Trigger Modifier",
	"Reapeat Trigger",
	"Teleport",
	"Destroy",
	"Acquire Transform",
	"Release Transform",
]


module.exports = defs