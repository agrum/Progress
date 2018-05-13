let mongoose = require('mongoose');

module.exports = function()
{
	let schema = new mongoose.Schema({
		name: String,
		constellation: {
			type: mongoose.Schema.Types.ObjectId,
			required: true,
			ref: 'constellations',
		},
		abilities: {
			type: [mongoose.Schema.Types.ObjectId],
			required: true,
			ref: 'abilities',
		},
		classes: {
			type: [mongoose.Schema.Types.ObjectId],
			required: true,
			ref: 'classes',
		},
		kits: {
			type: [mongoose.Schema.Types.ObjectId],
			required: true,
			ref: 'kits',
		},
	})
	schema.set('collection', 'presets')

	module.exports = mongoose.model('presets', schema)
}