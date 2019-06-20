var express = require('express');
var async = require('async');
var router = express.Router();

router.get('/', function (req, res, next) {
    req.app.db.models.skills
        .find({})
        .then(document => {
            res.send(document);
        })
        .catch(err => {
            console.error('skills error')
            console.error(err)
        })
})

router.get('/uppercase', function (req, res, next) {
    req.app.db.models.skills
        .updateMany({type: 'Kit'}, {category: 'Kit'})
        .then(document => {

        })
        .catch(err => {
            console.error('skills error')
            console.error(err)
        })
    req.app.db.models.skills
        .updateMany({type: 'Ability'}, {category: 'Ability'})
        .then(document => {

        })
        .catch(err => {
            console.error('skills error')
            console.error(err)
        })
    req.app.db.models.skills
        .updateMany({type: 'Class'}, {category: 'Class'})
        .then(document => {
            res.send(document);
        })
        .catch(err => {
            console.error('skills error')
            console.error(err)
        })
})

router.put('/', function(req, res, next) {
    req.app.db.models.skills
    .updateOne({_id: req.body._id}, req.body, { upsert : true })
    .then(presetDocument => {
        res.send({result: 'success'});
    })
    .catch(err => {
        console.error('add preset error')
        console.error(err)
    })
})

module.exports.router = router