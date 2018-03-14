var express = require('express');
var gameSettings = express.Router();

gameSettings.get('/:name', function(req, res, next) {
    req.app.db.collection("gameSettings")
    .findOne({'name':req.params.name})
    .then(function(document){
        res.send(document);
    });
});

module.exports.gameSettings = gameSettings;