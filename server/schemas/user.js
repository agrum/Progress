let mongoose = require('mongoose')
let validator = require('validator')

module.exports = function () {
	let user = new mongoose.Schema({
		email: {
			type: String,
			required: true,
			unique: true,
			lowercase: true,
			validate: (value) => {
				return validator.isEmail(value)
			}
		}
	})

	mongoose.model('user', user)
}