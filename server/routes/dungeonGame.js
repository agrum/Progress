var express = require('express');
var router = express.Router();

router.post('/create', function(req, res, next) {
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

router.post('/join/:id', function(req, res, next) {
    req.app.db.models.accounts
    .findOne({ _id: req.user.account })
    .then(accountDocument => {
        let preset;
        if (accountDocument.presets.length > 0)
            preset = accountDocument.presets[0];

        req.app.db.models.dungeonGames
        .findOne({ _id: req.params.id })
        .then(accountDocument => {
            accountDocument.presets.push(presetDocument._id);
            accountDocument.save();

            res.send(presetDocument);
          })
        .catch(err => {
            console.error('add preset to account error')
            console.error(err)
        })
    })
    .catch(err => {
        console.error('add preset to account error')
        console.error(err)
    })
})

router.get('/:id', function(req, res, next) {
    req.app.db.models.gameSettings
    .findOne({'name':req.params.name})
    .then(document => {
        console.error('gameSettings no error')
        res.send(document);
        })
    .catch(err => {
        console.error('gameSettings error')
        console.error(err)
    })
})

module.exports.router = router