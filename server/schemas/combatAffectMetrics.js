let mongoose = require('mongoose');

module.exports = function()
{
	let schema = new mongoose.Schema({
		idName: String,
        metric: {
			type: String,
			required: true,
		},
		amount: {
			type: [mongoose.Schema.Types.ObjectId],
			required: true,
			ref: 'numericValues',
		},
		condition: String,
		conditionParameters: {
			type: [mongoose.Schema.Types.ObjectId],
			required: true,
			ref: 'numericValues',
		},
		temporary: {
			type: Boolean,
			required: true,
		},
	})
	schema.pre('save', function (next) {
		if (schemaDefs.unitMetrics.indexOf(this.metric) == -1){
			return next({
				error: "schema.combatEffects",
				desc: "invalid affectedMetric: " + this.metric})
		}
		next();
	})
	schema.set('collection', 'combatAffectMetrics')

	module.exports = mongoose.model('combatAffectMetrics', schema)
}