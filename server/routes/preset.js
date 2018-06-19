var express = require('express');
var router = express.Router();

router.post('/', function(req, res, next) {
    var preset = JSON.parse(req.body.preset);
    req.app.db.models.presets
    .create(preset)
    .then(presetDocument => {
        console.error('add preset success')
        console.error(presetDocument)
        req.app.db.models.champions
        .update(
			{ _id: req.session.west.champion },
			{ $push: { 'presets': presetDocument._id }})
        .then(championDocument => {
            //championDocument.presets.push(presetDocument._id);
            //championDocument.save();

            console.error('add preset to champion success')
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
    var preset = JSON.parse(req.body.preset);
    req.app.db.models.presets
    .update(
		{ _id: preset._id},
		{ $set: preset })
    .then(presetDocument => {
        //presetDocument.set(preset);
        //presetDocument.save();
        console.error('update preset success')
        res.send(presetDocument);
    })
    .catch(err => {
        console.error('update preset error')
        console.error(err)
    })
})

router.delete('/:id', function(req, res, next) {
    console.error('remove preset')
    req.app.db.models.presets
    .remove({ _id: req.params.id })
    .then(presetDocument => {
		console.error('remove preset success')
		req.app.db.models.champions
		.update(
			{ _id: req.session.west.champion },  
			{ $pull: { 'presets': { _id: req.params.id }}})
        .then(championDocument => {
            console.error('remove preset to champion success')
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