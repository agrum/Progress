var express = require('express');
var router = express.Router();

router.get('/', function(req, res, next) {
    req.app.db.models.classes
    .find({})
    .then(document => {
        console.error('classes no error')
        res.send(document);
        })
    .catch(err => {
        console.error('classes error')
        console.error(err)
    })
})

module.exports.router = router