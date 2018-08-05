let schemaDefs = require('..schemas/schemaDefs');

var ability = {
	metrics2:[
		{
			idName: "Cooldown",
			name: "Cooldown",
			category: "misc",
			value: 12,
			upgType: 0,
			upgCost: 1,
		},
		{
			idName: "KnockDistance",
			name: "Travel and knock back distance",
			category: "desc",
			value: 4,
			upgType: 1,
			upgCost: 1,
		},
		{
			idName: "OnHitDamage",
			name: "On hit damage",
			category: "desc",
			value: -50,
			upgType: 1,
			upgCost: 1,
		},
		{
			idName: "OnKnockBackDamage",
			name: "On knock back collide",
			category: "desc",
			value: 0.1,
			reference: "ParentUnit.MaxLife",
			upgType: 1,
			upgCost: 1,
		},
	],
	modifier: {
		idName: "Knock_Skill",
		removable: false,
		alignment: "Neutral",
		maxStack: 1,
		tickPeriod: "SkillMetric.Cooldown",
		triggers: [
			{//skill trigger
				anyOf: [
					{
						event: schemaDefs.trigger.InputSkillUp,
						conditions: [
							"Self.Unit.IsAlive == True",
							"Self.Stack > 0",
						]
					}
				],
				effects: [
					{
						targets: "ParentUnit",
						actions: [
							{
								action: "ApplyPhysics",
								params: {
									overrideMoveInput: true,
									physics: {
										direction: "ParentUnit",
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
									target: "ParentUnit",
									modifier: {
										idName: "Knock_PunchPhase",
										removable: false,
										alignment: "Neutral",
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
																	method: "Add",
																	temporary: false,
																}
															},
															{
																action: "ApplyPhysics",
																params: {
																	overrideMoveInput: true,
																	physics: {
																		direction: "ParentUnit",
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
																					targets: "ParentUnit",
																					actions: [
																						{
																							action: "AffectUnitMetric",
																							params: {
																								metric: "Life",
																								amount: "SkillMetric.OnKnockBackDamage",
																								method: "Add",
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
									method: "Add",
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
									method: "Add",
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