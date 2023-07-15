let bcrypt = require('bcryptjs')

exports = module.exports = function (app) {
	var LocalStrategy = require('passport-local').Strategy;

	app.passport.use(new LocalStrategy(
		{
			usernameField: 'email',
			passwordField: 'password'
		},
		function (email, password, done) {
			app.db.models.users.findOne({ 'email': email })
			.then(user => {
				if (!user) {
					return done(null, false, { message: 'Unknown user' })
				}

				app.db.models.users.validatePassword(password, user.password, function (err, isValid) {
					if (err) {
						return done(err)
					}

					if (!isValid) {
						return done(null, false, { message: 'Invalid password' })
					}

					return done(null, user, { message: 'Login successful' })
				})
			})
			.catch(err => {
				return done(err)
			})
		}
	))


	app.passport.serializeUser(function (user, done) {
		done(null, user._id)
	})

	app.passport.deserializeUser(function (id, done) {
		app.db.models.users.findById(id)
		.then(user => {
			done(null, user)
		})
		.catch(err => {
			done(err, null)
		})
	})
}