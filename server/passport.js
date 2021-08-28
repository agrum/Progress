let bcrypt = require('bcryptjs')

exports = module.exports = function (app) {
	var LocalStrategy = require('passport-local').Strategy;

	app.passport.use(new LocalStrategy(
		{
			usernameField: 'email',
			passwordField: 'password'
		},
		function (email, password, done) {
			app.db.models.users.findOne({ 'email': email }, function (err, user) {
				if (err) {
					return done(err)
				}

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
		}
	))


	app.passport.serializeUser(function (user, done) {
		done(null, user._id)
	})

	app.passport.deserializeUser(function (id, done) {
		app.db.models.users.findById(id, function (err, user) {
			done(err, user)
		})
	})
}