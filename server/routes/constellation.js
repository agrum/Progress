var express = require('express');
var async = require('async');
var constellationObjectSource = require('../constellationObject').constellationObject;
var router = express.Router();

router.get('/', function(req, res, next) {
    req.app.db.models.constellations
    .find({})
    .then(document => {
        res.send(document);
        })
    .catch(err => {
        console.error('constellations error')
        console.error(err)
    })
})

module.exports.router = router