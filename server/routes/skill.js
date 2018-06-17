var express = require('express');
var router = express.Router();

router.get('/', function(req, res, next) {
    req.app.db.models.skills
    .find({})
    .then(document => {
        console.error('skills no error')
        res.send(document);
        })
    .catch(err => {
        console.error('skills error')
        console.error(err)
    })
})

module.exports.router = router