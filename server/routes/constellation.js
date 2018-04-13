var express = require('express');
var async = require('async');
var constellationObjectSource = require('../constellationObject').constellationObject;
var router = express.Router();

router.get('/setup', function(req, res, next) {
    var constellationObject = JSON.parse(JSON.stringify(constellationObjectSource));
    async.series([
        callback1 =>
        {
            async.eachSeries(constellationObject.abilities,
                (item, callback2) =>
                {
                    req.app.db.models.abilities
                    .findOne({'name':item.id})
                    .then(document => {
                        item.id = document._id
                        callback2()
                    })
                    .catch(err => {
                        console.error('constellationSetup error at abilities')
                        console.error('item id ' + item.id)
                        console.error(err)
                        callback2()
                    })
                },
                err =>
                {
                    callback1()
                }
            )
        },
        callback1 =>
        {
            async.eachSeries(constellationObject.classes,
                (item, callback2) =>
                {
                    req.app.db.models.classes
                    .findOne({'name':item.id})
                    .then(document => {
                        item.id = document._id
                        callback2()
                    })
                    .catch(err => {
                        console.error('constellationSetup error at classes')
                        console.error('item id ' + item.id)
                        console.error(err)
                        callback2()
                    })
                },
                err =>
                {
                    callback1()
                }
            )
        },
        callback1 =>
        {
            async.eachSeries(constellationObject.kits,
                (item, callback2) =>
                {
                    req.app.db.models.kits
                    .findOne({'name':item.id})
                    .then(document => {
                        item.id = document._id
                        callback2()
                    })
                    .catch(err => {
                        console.error('constellationSetup error at kits')
                        console.error('item id ' + item.id)
                        console.error(err)
                        callback2()
                    })
                },
                err =>
                {
                    callback1()
                }
            )
        },
        callback1 =>
        {
            async.eachSeries(constellationObject.shapes,
                (item, callback2) =>
                {
                    req.app.db.models.shapes
                    .findOne({'name':item.id})
                    .then(document => {
                        item.id = document._id
                        callback2()
                    })
                    .catch(err => {
                        console.error('constellationSetup error at shapes')
                        console.error('item id ' + item.id)
                        console.error(err)
                        callback2()
                    })
                },
                err =>
                {
                    callback1()
                }
            )
        }
    ],
    err =>
    {
        req.app.db.models.constellations
        .create(constellationObject)
        .then(err => {
            res.send(constellationObject)
        })
    })
})


router.get('/:name', function(req, res, next) {
    console.log(req.params.name)
    req.app.db.models.constellations
    .findOne({'name':req.params.name})
    .then(document => {
        console.log('constellation no error')
        //console.log(document)
        res.send(document);
        })
    .catch(err => {
        console.error('constellation error')
        console.error(err)
    })
})

module.exports.router = router