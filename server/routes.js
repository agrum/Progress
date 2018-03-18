var express = require('express');
var router = express.Router();

router.use('/gameSettings', require('./routes/gameSettings').router)
router.use('/signup', require('./routes/signup').router)

router.use(function (req, res, next) {
	var err = new Error('Not Found')
	err.status = 404
	next(err)
})

module.exports.router = router