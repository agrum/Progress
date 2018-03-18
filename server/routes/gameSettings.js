var express = require('express');
var gameSettings = express.Router();

gameSettings.get('/:name', function(req, res, next) {
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
});

module.exports.gameSettings = gameSettings;