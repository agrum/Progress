let mongoose = require('mongoose');

module.exports = function()
{
	let schema = new mongoose.Schema({
		idName: String,
		name: {
			type: String,
			required: true,
		},
		event: {
			type: String,
			required: true,
		},
		condition: String,
		conditionParameters:{
			type: [mongoose.Schema.Types.ObjectId],
			required: true,
			ref: 'numericValues',
		},
	})
	schema.pre('save', function (next) {
		if (schemaDefs.triggers.indexOf(this.event) == -1){
			return next({
				error: "schema.combatTriggers",
				desc: "invalid event: " + this.event})
		}
		next();
	})
	schema.set('collection', 'combatTriggers')

	module.exports = mongoose.model('combatEffects', schema)
}