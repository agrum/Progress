var express = require('express');
var router = express.Router();

router.get('/:name', function(req, res, next) {
    req.app.db.models.gameSettings
    .findOne({'name':req.params.name})
    .then(document => {
        res.send(document);
        })
    .catch(err => {
        console.error('gameSettings error')
        console.error(err)
    })
})

module.exports.router = router