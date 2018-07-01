let mongoose = require('mongoose');

module.exports = function()
{
	let upgrade = new mongoose.Schema({
		metricCategory: {
			type: String,
			required: true,
        },
		metricName: {
			type: String,
			required: true,
        },
		level: {
			type: Number,
			required: true,
		},
	})

	let skillUpgrade = new mongoose.Schema({
		skill: {
			type: mongoose.Schema.Types.ObjectId,
			required: true,
			ref: 'skills',
		},
		upgrades : [upgrade]
	})

	let schema = new mongoose.Schema({
		name: {
			type: String,
			unique: true,
			required: true,
			trim: true,
		},
		level: {
			type: Number,
			required: true,
		},
		classes: {
			type: [mongoose.Schema.Types.ObjectId],
			required: true,
			ref: 'skills',
		},
        presets: {
			type: [mongoose.Schema.Types.ObjectId],
			required: true,
			ref: 'presets',
        },
        skillUpgrades: [skillUpgrade],
	})
	schema.set('collection', 'champions')

	module.exports = mongoose.model('champions', schema)
}