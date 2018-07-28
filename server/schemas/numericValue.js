let mongoose = require('mongoose');
let schemaDefs = require('./schemaDefs');

module.exports = function()
{
	let schema = new mongoose.Schema({
		type: {
			type: String,
			required: true,
		},
        value: {
			type: Number,
			required: true,
		},
		method: {
			type: String,
			required: true,
		},
		canUpgrade: {
			type: Boolean,
			required: true,
			default: false,
		},
		upgradeType: {
			type: String,
		},
		upgradeCost: {
			type: Number,
			default: 1.00001 //to force double type
		},
		referenceTarget: String,
		referenceMetric: String,
	})
	schema.pre('save', function (next) {
		var typeReference = schemaDefs.numericValue.types[0];
		if (schemaDefs.numericValue.types.indexOf(this.type) == -1)
			return next({
				error: "schema.numericValue",
				desc: " invalid type: " + this.type})
		if (schemaDefs.numericValue.methods.indexOf(this.method) == -1)
			return next({
				error: "schema.numericValue",
				desc: " invalid method: " + this.method})
		if (this.type == typeReference && schemaDefs.numericValue.referenceTargets.indexOf(this.referenceTarget) == -1)
			return next({
				error: "schema.numericValue",
				desc: " invalid referenceTarget: " + this.referenceTarget})
		if (this.value == 0)
			return next({
				error: "schema.numericValue",
				desc: " invalid value zero"})

		if (this.type == typeReference)
		{
			if (schemaDefs.numericValue.referenceTargets == undefined)
				return next({
					error: "schema.numericValue",
					desc: " missing referenceTarget"})
			if (schemaDefs.unitMetrics == undefined)
				return next({
					error: "schema.numericValue",
					desc: " missing referenceTarget"})
		}
		next();
	})
	schema.set('collection', 'numericValues')

	module.exports = mongoose.model('numericValues', schema)
}