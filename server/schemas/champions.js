let mongoose = require('mongoose');

module.exports = function()
{
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
        upgrades: {
			type: [mongoose.Schema.Types.ObjectId],
			required: true,
			ref: 'skills',
        },
	})
	schema.set('collection', 'champions')

	module.exports = mongoose.model('champions', schema)
}