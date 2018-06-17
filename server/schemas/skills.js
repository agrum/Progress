let mongoose = require('mongoose');

module.exports = function()
{
	let schema = new mongoose.Schema({
        type: String,
		name: String,
		description: String,
		metrics: mongoose.Schema.Types.Mixed
	})
	schema.set('collection', 'skills')

	module.exports = mongoose.model('skills', schema)
}