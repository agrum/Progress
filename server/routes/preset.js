var express = require('express');
var router = express.Router();

router.post('/', function(req, res, next) {
    req.app.db.models.presets
    .create(req.body)
    .then(presetDocument => {
        req.app.db.models.champions
        .update(
			{ _id: req.session.west.champion._id },
			{ $push: { 'presets': presetDocument._id }})
        .then(championDocument => {
            res.send(presetDocument);
          })
        .catch(err => {
            console.error('add preset to champion error')
            console.error(err)
        })
    })
    .catch(err => {
        console.error('add preset error')
        console.error(err)
    })
})

router.put('/', function(req, res, next) {
    req.app.db.models.presets
    .update(
		{ _id: req.body._id},
		{ $set: req.body })
    .then(presetDocument => {
        res.send(presetDocument);
    })
    .catch(err => {
        console.error('update preset error')
        console.error(err)
    })
}) 

router.delete('/:presetId', function(req, res, next) {
    req.app.db.models.presets
    .remove({ _id: req.params.presetId })
    .then(presetDocument => {
		req.app.db.models.champions
		.update(
			{ _id: req.session.west.champion._id },  
			{ $pull: { 'presets': { _id: req.params.presetId }}})
        .then(championDocument => {
            res.send(presetDocument);
          })
        .catch(err => {
            console.error('remove preset to champion error')
            console.error(err)
        })
    })
    .catch(err => {
        console.error('remove preset error')
        console.error(err)
    })
})

module.exports.router = router