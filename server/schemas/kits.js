let mongoose = require('mongoose');

module.exports = function()
{
	let schema = new mongoose.Schema({
		name: String,
		description: String,
		metrics: mongoose.Schema.Types.Mixed
	})
	schema.set('collection', 'kits')

	module.exports = mongoose.model('kits', schema)
}