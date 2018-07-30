let schemaDefs = require('..schemas/schemaDefs');

var ability = {
	cooldown: {
		value: 12,
		canUpgrade: true,
		upgradeCost: 1,
	},
	charge: 1,
	modifier: {
		idName: "Knock",
		removable: false,
		triggers: [
			{
				anyOf:[
					{
						event: schemaDefs.trigger.InputSkillUp,
						condition: "0 == 1",
						conditionParameters: [
							"Self.Unit.IsAlive",
							1,
						]
					}
				],
				effects: [
					{
						targets: "Self.Unit",
						actions: [
							{
								action: "ApplyPhysics",
								overrideMoveInput: true,

							}
						]
					}
				],
			}
		]
	}
}