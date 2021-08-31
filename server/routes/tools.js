var express = require('express');
var async = require('async');
var router = express.Router();

router.post('/skillsUpdate', function(req, res, next) {
    console.log(req.body)
    req.app.db.models.skills
    .updateOne({name: req.body.name}, req.body, { upsert : true })
    .then(presetDocument => {
        res.send({result: 'success'});
    })
    .catch(err => {
        console.error('update skills error')
        console.error(err)
    })
})

module.exports.router = router