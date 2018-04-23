var express = require('express');
var router = express.Router();

var isAuthenticated = function (req, res, next) {
	if (req.isAuthenticated())
		return next();
	res.send({'error':'authentication required'});
}

router.all('*', (req, res, next) => { console.log(req._parsedOriginalUrl); return next(); })
router.use('/login', require('./routes/login').router)
router.use('/signup', require('./routes/signup').router)
router.use('/constellation', require('./routes/constellation').router)
router.use('/ability', require('./routes/ability').router)

router.all('*', isAuthenticated)
router.use('/gameSettings', require('./routes/gameSettings').router)

router.use(function (req, res, next) {
	var err = new Error('Not Found')
	err.status = 404
	next(err)
})

module.exports.router = router