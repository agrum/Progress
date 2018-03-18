let mongoose = require('mongoose')
let validator = require('validator')
let bcrypt = require('bcrypt')

module.exports = function () {
	let schema = new mongoose.Schema({
		email: {
			type: String,
			required: true,
			unique: true,
			lowercase: true,
			validate: (value) => {
				return validator.isEmail(value)
			}
		},
		username: {
			type: String,
			unique: true,
			required: true,
			trim: true
		},
		password: {
			type: String,
			required: true,
		},
	})
	schema.pre('save', function (next) {
		var user = this;
		bcrypt.hash(user.password, 12, function (err, hash) {
			if (err) {
				return next(err);
			}
			user.password = hash;
			next();
		})
	})
	schema.set('collection', 'users')

	mongoose.model('users', schema)
}