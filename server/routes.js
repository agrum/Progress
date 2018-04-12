var express = require('express');
var router = express.Router();

var isAuthenticated = function (req, res, next) {
	console.log(req.cookies)
	console.log(req)
	if (req.isAuthenticated())
		return next();
	res.send({'error':'authentication required'});
}

router.use('/login', require('./routes/login').router)
router.use('/signup', require('./routes/signup').router)
router.use('/setupConstellation', require('./routes/setupConstellation').router)

router.all('*', isAuthenticated)
router.use('/gameSettings', require('./routes/gameSettings').router)

router.use(function (req, res, next) {
	var err = new Error('Not Found')
	err.status = 404
	next(err)
})

module.exports.router = router