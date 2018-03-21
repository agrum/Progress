var express = require('express');
var router = express.Router();

router.get('/', function (req, res, next) {
    res.sendFile('ui/signup.html', { root: './' })
})

router.post('/', function (req, res, next) {
    console.log(req.body)
    if (!(req.body.email &&
        req.body.username &&
        req.body.password)) {
        return res.send("Invalid request:" + req.body)
    }

    var userData = {
        email: req.body.email,
        username: req.body.username,
        password: req.body.password
    }

    //use schema.create to insert data into the db
    req.app.db.models.users.create(userData, function (err, user) {
        if (err) {
            return next(err)
        } else {
            return res.send({})
        }
    })
})

module.exports.router = router