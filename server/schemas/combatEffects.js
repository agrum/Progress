let mongoose = require('mongoose');

module.exports = function()
{
	let schema = new mongoose.Schema({
		idName: String,
		action: {
			type: String,
			required: true,
		},
		referenceKind: String,
		reference: {
			type: [mongoose.Schema.Types.ObjectId],
			refPath: 'referenceKind',
		},
		target: String,
	})
	schema.pre('save', function (next) {
		if (schemaDefs.actions.indexOf(this.action) == -1){
			return next({
				error: "schema.combatEffects",
				desc: "invalid action: " + this.action})
		}
		if (action == defs.actions[0]){ //affect metrics
			this.referenceKind = "combatMetricEffects";
		}
		else if (action == defs.actions[1]){ //apply modifier
			this.referenceKind = "combatModifiers";
		}
		else if (action == defs.actions[2]){ //create projectile
			this.referenceKind = "combatProjectiles";
		}
		else if (action == defs.actions[3]){ //create unit
			this.referenceKind = "combatUnits";
		}

		next();
	})
	schema.set('collection', 'combatEffects')

	module.exports = mongoose.model('combatEffects', schema)
}