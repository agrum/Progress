var express = require('express');
var async = require('async');
var router = express.Router();

router.get('/outdoor', function (req, res, next) {
    req.app.db.models.outdoorLayouts
        .find({})
        .then(document => {
            res.send(document);
        })
        .catch(err => {
            console.error('skills error')
            console.error(err)
        })
})

module.exports.router = router