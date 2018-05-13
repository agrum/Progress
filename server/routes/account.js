var express = require('express');
var router = express.Router();

router.get('/:id', function(req, res, next) {
    req.app.db.models.accounts
    .findOne({ '_id' : req.params.id })
    .populate({
        path: 'presets',
        model: 'presets'
    })
    .then(document => {
        console.error(document)
        console.error('accounts success')
        res.send(document);
        })
    .catch(err => {
        console.error('accounts error')
        console.error(err)
    })
})

router.post('/preset', function(req, res, next) {
    var preset = JSON.parse(req.body.preset);
    req.app.db.models.presets
    .create(preset)
    .then(presetDocument => {
        console.error('add preset success')
        console.error(presetDocument)
        req.app.db.models.accounts
        .findOne({ _id: req.user.account })
        .then(accountDocument => {
            accountDocument.presets.push(presetDocument._id);
            accountDocument.save();

            console.error('add preset to account success')
            res.send(presetDocument);
          })
        .catch(err => {
            console.error('add preset to account error')
            console.error(err)
        })
    })
    .catch(err => {
        console.error('add preset error')
        console.error(err)
    })
})

router.delete('/preset/:id', function(req, res, next) {
    console.error('remove preset')
    req.app.db.models.presets
    .remove({ _id: req.params.id })
    .then(document => {
        console.error('remove preset success')
        res.send(document);
    })
    .catch(err => {
        console.error('remove preset error')
        console.error(err)
    })
})

module.exports.router = router