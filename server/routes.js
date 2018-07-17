var express = require('express');
var router = express.Router();

var logIt = function (req, res, next) {
	console.log(req.protocol + " " + req.method  + " " + req.originalUrl)
	next()
}

var isAuthenticated = function (req, res, next) {
	if (req.isAuthenticated())
		return next();
	res.send({'error':'authentication required'});
}

router.all('*', logIt)

router.use('/login', require('./routes/login').router)
router.use('/signup', require('./routes/signup').router)
router.use('/skill', require('./routes/skill').router)

router.all('*', isAuthenticated)
router.use('/user', require('./routes/user').router)
router.use('/account', require('./routes/account').router)
router.use('/champion', require('./routes/champion').router)
router.use('/gameSettings', require('./routes/gameSettings').router)
router.use('/constellation', require('./routes/constellation').router)
router.use('/skill', require('./routes/skill').router)

router.use(function (req, res, next) {
	var err = new Error('Route not found: ' + req.originalUrl)
	err.status = 404
	next(err)
})

module.exports.router = router