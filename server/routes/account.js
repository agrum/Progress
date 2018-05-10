var express = require('express');
var router = express.Router();

router.get('/:id', function(req, res, next) {
    req.app.db.models.accounts
    .findOne({ '_id' : req.params.id })
    .populate('presets')
    .then(document => {
        console.error('accounts no error')
        res.send(document);
        })
    .catch(err => {
        console.error('accounts error')
        console.error(err)
    })
})

module.exports.router = router