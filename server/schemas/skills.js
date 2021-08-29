let mongoose = require('mongoose');

module.exports = function()
{
    let metricSchema = new mongoose.Schema({
        name: String,
        tags: [ mongoose.Schema.Types.Mixed ],
        numeric: mongoose.Schema.Types.Mixed,
    });

	let schema = new mongoose.Schema({
        type: String,
		name: String,
		description: String,
		details: String,
		category: mongoose.Schema.Types.Mixed,
        metrics: [ metricSchema ],
	})
	schema.set('collection', 'skills')

	module.exports = mongoose.model('skills', schema)
}