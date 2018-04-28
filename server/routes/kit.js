var express = require('express');
var router = express.Router();

router.get('/', function(req, res, next) {
    req.app.db.models.kits
    .find({})
    .then(document => {
        console.error('kits no error')
        res.send(document);
        })
    .catch(err => {
        console.error('kits error')
        console.error(err)
    })
})

module.exports.router = router