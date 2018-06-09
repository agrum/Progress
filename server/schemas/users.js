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
			trim: true,
		},
		password: {
			type: String,
			required: true,
		},
		account: {
			type: mongoose.Schema.Types.ObjectId,
			required: true,
			ref: 'accounts',
		}
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
	
	schema.statics.validatePassword = function(password, hash, done) {
		var bcrypt = require('bcrypt');
		bcrypt.compare(password, hash, function(err, res) {
		  done(err, res);
		});
	  };

	mongoose.model('users', schema)
}