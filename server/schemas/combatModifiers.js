let mongoose = require('mongoose');
let schemaDefs = require('./schemaDefs');

module.exports = function()
{
	let stackSchema = new mongoose.Schema({
		max: {
			type: mongoose.Schema.Types.Mixed,
			default: 1,
		},
		stackMethod: {
			type: String,
			deafult: schemaDefs.modifier.stackMethod.Cumulative
		},
		stackRefresh: {
			type: String,
			default: schemaDefs.modifier.stackRefresh.RefreshIfSameSourceOrStronger
		},
	})
	let triggerSchema = new mongoose.Schema({
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
	})

	let schema = new mongoose.Schema({
		idName: String,
		name: {
			type: String,
			required: true,
		},
		triggers: {
			type: [triggerSchema],
			required: true,
		},
		duration: {
			type: mongoose.Schema.Types.Mixed,
			default: -1,
		},
		tickPeriod: { //numeric values of effects are dt / tickPeriod, dt being time now - max(last tick, creation time)
			type: mongoose.Schema.Types.Mixed,
			default: -1,
		},
		removable: {
			type: Boolean,
			required: false,
			default: true,
		},
		stack: {
			type: stackSchema
		},
		listeningRadius: {
			type: Number,
			default: 0,
		},
	})
	schema.pre('save', function (next) {
		next();
	})
	schema.set('collection', 'combatEffects')

	module.exports = mongoose.model('combatEffects', schema)
}