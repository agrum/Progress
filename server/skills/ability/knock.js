let action = require('..schemas/defs/action');
let alignment = require('..schemas/defs/alignment');
let attribute = require('..schemas/defs/attribute');
let compare = require('..schemas/defs/compare');
let extract = require('..schemas/defs/extract');
let integration = require('..schemas/defs/integration');
let modifierAttribute = require('..schemas/defs/modifierAttribute');
let numericUpgradeType = require('..schemas/defs/numericUpgradeType');
let projectile = require('..schemas/defs/projectile');
let stack = require('..schemas/defs/stack');
let subject = require('..schemas/defs/subject');
let trigger = require('..schemas/defs/trigger');
let unitCondition = require('..schemas/defs/unitCondition');

let modBuilder = require('..schemas/helper/modifierBuilder');

var ability = {
	metrics2:[
		modBuilder.Numeric(
			"Cooldown", 
			"Cooldown", 
			numeric.category.Misc, 
			12, 
			integration.Add, 
			numeric.upgradeType.Redu, 
			1),
		modBuilder.Numeric(
			"KnockDistance", 
			"Travel and knock back distance", 
			numeric.category.Desc, 
			4, 
			integration.Add, 
			numeric.upgradeType.Redu, 
			1),
		modBuilder.Numeric(
			"OnHitDamage", 
			"On hit damage", 
			numeric.category.Desc, 
			-50, 
			integration.Add, 
			integration.Add, 
			1),
		modBuilder.Numeric(
			"OnKnockBackDamage", 
			"On knock back collide", 
			numeric.category.Desc, 
			modBuilder.AttributeReference(0.1, subject.Target, extract.Max, attribute.Health),
			integration.Mult, 
			numeric.upgradeType.Inc, 
			1),
	],
	modifier: {
		idName: "Knock_Skill",
		removable: false,
		alignment: alignment.Neutral,
		stacking: modBuilder.Stack(stack.method.One, stack.refresh.No, 1, stack.Permanent),
		tickPeriod: "Cooldown",
		triggers: [
			modBuilder.TriggeredEffect(//skill trigger
				[
					modBuilder.Trigger(
						trigger.InputSkillUp,
						[
							modBuilder.Condition(modifierAttribute.Stack, compare.Greater, 0),
							modBuilder.Condition(modBuilder.AttributeReference(1, subject.Self, attribute.Health, extract.Current), compare.GreaterOrEqual, 0),
						])
				],
				[
					{
						targets: subject.Self,
						actions: [
							{
								action: "ApplyPhysics",
								params: {
									overrideMoveInput: true,
									physics: {
										direction: subject.Self,
										speed: 10,
										distance: "KnockDistance",
										collideWith: [
											"All"
										],
										angularSpeed: 0,
										pivotX: 0,
										pivotY: 0,
									}
								}
							},
							{
								action: "ApplyModifier",
								params: {
									target: subject.Self,
									modifier: {
										idName: "Knock_PunchPhase",
										removable: false,
										alignment: alignment.Neutral,
										triggers: [
											{
												anyOf: [
													{
														event: schemaDefs.trigger.CollideWith,
														conditions: [
															"Trigger.Unit.IsEnemy == True",
															"Trigger.Unit.IsAlive == True",
														]
													}
												],
												effects: [
													{
														targets: "Trigger.Unit",
														actions: [
															{
																action: "AffectUnitMetric",
																params: {
																	metric: "Life",
																	amount: "SkillMetric.OnHitDamage",
																	method: defs.Add,
																	temporary: false,
																}
															},
															{
																action: "ApplyPhysics",
																params: {
																	overrideMoveInput: true,
																	physics: {
																		direction: "Unit",
																		speed: 10,
																		distance: "SkillMetric.KnockDistance",
																		collideWith: [
																			"All"
																		],
																		angularSpeed: 0,
																		pivotX: 0,
																		pivotY: 0,
																	}
																}
															},
															{
																action: "ApplyModifier",
																params: {
																	idName: "Knock_KnockBackPhase",
																	removable: true,
																	alignment: "Hostile",
																	duration: 0.4,
																	triggers: [
																		{
																			anyOf: [
																				{
																					event: schemaDefs.trigger.CollideWith,
																					conditions: [
																						"Trigger.Unit.IsEnvironment == True",
																					]
																				}
																			],
																			effects: [
																				{
																					targets: "Unit",
																					actions: [
																						{
																							action: "AffectUnitMetric",
																							params: {
																								metric: "Life",
																								amount: "SkillMetric.OnKnockBackDamage",
																								method: defs.Add,
																								temporary: false,
																							},
																						},
																					],
																				},
																			],
																		},
																	],
																},
															},
														],
													},
												],
											},
										],
									},
								},
							},
							{
								conditions: [
									"Self.Stack == Self.MaxStack"
								],
								action: "AffectModifierMetric",
								params: {
									metric: "TickPeriod",
									amount: -1,
									method: "Set",
									temproary: false,
								},
							},
							{
								action: "AffectModifierMetric",
								params: {
									metric: "Stack",
									amount: -1,
									method: defs.Add,
									temproary: false,
								},
							},
						],
					},
				],
			},
			{//cooldown
				anyOf: [
					{
						event: "Trigger.Tick"
					},
					{
						event: "Trigger.Begin"
					},
				],
				effects: [
					{
						targets: "Self",
						actions: [
							{
								conditions: [
									"Self.Stack < Self.MaxStack"
								],
								action: "AffectModifierMetric",
								params: {
									metric: "Stack",
									amount: 1,
									method: defs.Add,
									temproary: false,
								},
							},
							{
								conditions: [
									"Self.Stack == Self.MaxStack"
								],
								action: "AffectModifierMetric",
								params: {
									metric: "TickPeriod",
									amount: "SkillMetric.Cooldown",
									method: "Set",
									temproary: false,
								},
							},
						],
					},
				],
			},
		],
	},
},