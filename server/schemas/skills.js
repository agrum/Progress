let mongoose = require('mongoose');

module.exports = function()
{
    let metricSchema = new mongoose.Schema({
        name: String,
        category: String,
        value: Number,
        upgType: Number,
        upgCost: Number,
    });

	let schema = new mongoose.Schema({
        type: String,
		name: String,
		description: String,
        metrics: mongoose.Schema.Types.Mixed,
        metrics2: [ metricSchema ],
	})
	schema.set('collection', 'skills')

	module.exports = mongoose.model('skills', schema)
}