var express = require('express');
var router = express.Router();

router.get('/', function(req, res, next) {
    req.app.db.models.abilities
    .find({})
    .then(document => {
        console.error('abilities no error')
        res.send(document);
        })
    .catch(err => {
        console.error('abilities error')
        console.error(err)
    })
})

module.exports.router = router