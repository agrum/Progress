let mongoose = require('mongoose');

module.exports = function()
{
	let physicsSchema = new mongoose.Schema({
		type: {
			type: String,
			enum: ["Balistic", "Homing"],
			default: "Balistic"
		},
		direction: {
			type: String,
			required: true,
		},
		speed: {
			type: [mongoose.Schema.Types.ObjectId],
			required: true,
			ref: 'numericValues',
		},
		distance: {
			type: [mongoose.Schema.Types.ObjectId],
			required: true,
			ref: 'numericValues',
		},
		collideWith: {
			type: [String],
			required: true,
		},
		updateDirection: {
			type: Boolean,
			required: true,
		},
		angularSpeed: {
			type: [mongoose.Schema.Types.ObjectId],
			ref: 'numericValues',
		},
		pivotX: {
			type: [mongoose.Schema.Types.ObjectId],
			ref: 'numericValues',
		},
		pivotY: {
			type: [mongoose.Schema.Types.ObjectId],
			ref: 'numericValues',
		},
	})
	let schema = new mongoose.Schema({
		idName: String,
		shape: {
			type: String,
			required: true,
		},
		size: {
			type: [mongoose.Schema.Types.ObjectId],
			required: true,
			ref: 'numericValues',
		},
		modifiers: {
			type: [mongoose.Schema.Types.ObjectId],
			required: true,
			ref: 'combatModifiers',
		},
		collideWith: {
			type: [String],
			required: true,
		},
		parentDependent: {
			type: Boolean,
			default: true,
		},
		health: {
			type: [mongoose.Schema.Types.ObjectId],
			ref: 'numericValues',
		},
		shield: {
			type: [mongoose.Schema.Types.ObjectId],
			ref: 'numericValues',
		},
		armor: {
			type: [mongoose.Schema.Types.ObjectId],
			ref: 'numericValues',
		},
		lifespan: {
			type: [mongoose.Schema.Types.ObjectId],
			ref: 'numericValues',
		},
		physics: {
			type: physicsSchema
		}
	})
	schema.pre('save', function (next) {
		if (schemaDefs.projectile.shape.indexOf(this.direction) == -1){
			return next({
				error: "schema.combatProjectiles",
				desc: "invalid direction: " + this.direction})
		}
		if (schemaDefs.projectile.direction.indexOf(this.shape) == -1){
			return next({
				error: "schema.combatProjectiles",
				desc: "invalid shape: " + this.shape})
		}
		next();
	})
	schema.set('collection', 'combatUnits')

	module.exports = mongoose.model('combatUnits', schema)
}