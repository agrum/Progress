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
		details: String,
        metrics: mongoose.Schema.Types.Mixed,
        metrics2: [ metricSchema ],
        modifiers: {
			type: [mongoose.Schema.Types.ObjectId],
			required: true,
			ref: 'combatModifiers',
        },
	})
	schema.set('collection', 'skills')

	module.exports = mongoose.model('skills', schema)
}