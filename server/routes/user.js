var express = require('express');
var router = express.Router();

router.get('/', function(req, res, next) {
    req.app.db.models.users
    .findOne({ '_id' : req.user._id }, 'email username account')
    .then(document => {
        console.error('users no error')
        console.log(document)
        res.send(document);
        })
    .catch(err => {
        console.error('users error')
        console.error(err)
    })
})

module.exports.router = router