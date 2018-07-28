let mongoose = require('mongoose');

module.exports = function()
{
	let trigger = new mongoose.Schema({
		anyOf: {
			type: [mongoose.Schema.Types.ObjectId],
			required: true,
			ref: 'combatTriggers',
        },
		effects: {
			type: [mongoose.Schema.Types.ObjectId],
			required: true,
			ref: 'combatEffects',
		},
		stacksConsumed: {
			type: mongoose.Schema.Types.ObjectId,
			ref: 'numericValues',
		},
	})

	let schema = new mongoose.Schema({
		idName: String,
		name: {
			type: String,
			required: true,
		},
		triggers: {
			type: [trigger],
			required: true,
		},
		duration: {
			type: [mongoose.Schema.Types.ObjectId],
			required: true,
			ref: 'numericValues',
		},
		tickPeriod: { //numeric values of effects are dt / tickPeriod, dt being time now - max(last tick, creation time)
			type: [mongoose.Schema.Types.ObjectId],
			required: true,
			ref: 'numericValues',
		},
		removable: {
			type: Boolean,
			required: false,
		},
		stack: {
			type: [mongoose.Schema.Types.ObjectId],
			required: true,
			ref: 'numericValues',
		},
		stackMethod: {
			type: String,
			deafult: defs.modifier.stackMethod[0]
		},
		stackRefresh: {
			type: String,
			default: defs.modifier.stackMethod[0]
		},
	})
	schema.pre('save', function (next) {
		next();
	})
	schema.set('collection', 'combatEffects')

	module.exports = mongoose.model('combatEffects', schema)
}