let mongoose = require('mongoose');

module.exports = function()
{
    let metricUpgradeSchema = new mongoose.Schema({
        sign: mongoose.Schema.Types.Mixed,
        maxUpgradeCount: Number,
        factor: Number,
    });

    let metricSchema = new mongoose.Schema({
        name: String,
        category: mongoose.Schema.Types.Mixed,
        numeric: mongoose.Schema.Types.Mixed,
        ugprade: metricUpgradeSchema,
    });

	let schema = new mongoose.Schema({
        type: String,
		name: String,
		description: String,
		details: String,
		category: mongoose.Schema.Types.Mixed,
        metrics: [ metricSchema ],
        passives: [ mongoose.Schema.Types.Mixed ],
        layers: [ mongoose.Schema.Types.Mixed ],
	})
	schema.set('collection', 'skills')

	module.exports = mongoose.model('skills', schema)
}