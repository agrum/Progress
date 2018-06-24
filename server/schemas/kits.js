let mongoose = require('mongoose');

module.exports = function()
{
	let schema = new mongoose.Schema({
        type: String,
		name: String,
		description: String,
		details: String,
        metrics: mongoose.Schema.Types.Mixed,
	})
	schema.set('collection', 'kits')

	module.exports = mongoose.model('kits', schema)
}