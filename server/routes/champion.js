var express = require('express');
var router = express.Router();

router.get('/:id', function(req, res, next) {
    req.app.db.models.champions
    .findOne({ '_id' : req.params.id })
    .populate({
        path: 'presets',
        model: 'presets'
    })
    .then(document => {
		console.error(document)
		req.session.west.champion = document
        console.error('get champion success')
        res.send(document);
        })
    .catch(err => {
        console.error('get champion error')
        console.error(err)
    })
})

router.post('/', function(req, res, next) {
    var champion = JSON.parse(req.body.champion);
    req.app.db.models.champions
    .create(champion)
    .then(championDocument => {
        console.error('add champion success')
        console.error(championDocument)
        req.app.db.models.accounts
        .update(
			{ _id: req.user.account },
			{ $push: { 'champions': championDocument_id }})
        .then(accountDocument => {
            //accountDocument.champions.push(championDocument._id);
            //accountDocument.save();
            console.error('add champion to account success')
            res.send(championDocument);
          })
        .catch(err => {
            console.error('add champion to account error')
            console.error(err)
        })
    })
    .catch(err => {
        console.error('add champion error')
        console.error(err)
    })
})

router.put('/', function(req, res, next) {
    var champion = JSON.parse(req.body.champion);
    req.app.db.models.champions
    .update(
		{ _id: champion._id},
		{ $set: champion })
    .then(championDocument => {
        //championDocument.set(champion);
        //championDocument.save();
        console.error('update champion success')
        res.send(championDocument);
    })
    .catch(err => {
        console.error('update champion error')
        console.error(err)
    })
})

router.delete('/:id', function(req, res, next) {
    console.error('remove champion')
    req.app.db.models.champions
    .remove({ _id: req.params.id })
    .then(championDocument => {
		console.error('remove champion success')
		req.app.db.models.accounts
		.update(
			{ _id: req.user.account },  
			{ $pull: { 'champions': { _id: req.params.id }}})
        .then(accountDocument => {
            console.error('remove preset to champion success')
            res.send(championDocument);
          })
        .catch(err => {
            console.error('remove preset to champion error')
            console.error(err)
        })
        res.send(document);
    })
    .catch(err => {
        console.error('remove champion error')
        console.error(err)
    })
})

module.exports.router = router