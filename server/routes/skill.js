var express = require('express');
var async = require('async');
var router = express.Router();

router.get('/', function (req, res, next) {
    req.app.db.models.skills
        .find({})
        .then(document => {
            res.send(document);
        })
        .catch(err => {
            console.error('skills error')
            console.error(err)
        })
})

router.get('/copy', function (req, res, next) {
    var queries = []
    queries.push((cb) => {
        req.app.db.models.abilities
        .find({})
        .then(documents => {
            for (var i = 0; i < documents.length; ++i)
                documents[i].type = "ability"
            req.app.db.models.skills
            .insertMany(documents)
            .then(documents_ =>
            {
                cb(null, "ok");
            })
        })
    });
    queries.push((cb) => {
        req.app.db.models.kits
        .find({})
        .then(documents => {
            for (var i = 0; i < documents.length; ++i)
                documents[i].type = "kit"
            req.app.db.models.skills
            .insertMany(documents)
            .then(documents_ =>
            {
                cb(null, "ok");
            })
        })
    });
    queries.push((cb) => {
        req.app.db.models.classes
        .find({})
        .then(documents => {
            for (var i = 0; i < documents.length; ++i)
                documents[i].type = "class"
            req.app.db.models.skills
            .insertMany(documents)
            .then(documents_ =>
            {
                cb(null, "ok");
            })
        })
    });

    async.parallel(queries, () =>
    {
        res.send({});
    })
})

router.get('/convert', function (req, res, next) {
    req.app.db.models.skills
        .find({})
        .then(documents => {
            let recur = (document, category, object) =>
            {
                var objConstructor = {}.constructor

                for (var key in object) {
                    if (object[key].constructor === objConstructor)
                    {
                        recur(document, key, object[key])
                    }
                    else
                    {
                        let type = 1;
                        if (key == "cooldown" || key == "castTime" || key == "subtractive" || key == "cost" || key == "fraction")
                            type = 0;
                        document.metrics2.push({
                            name: key,
                            category: category,
                            value: object[key],
                            upgType: type,
                            upgCost: 1,
                        })
                    }
                }
            }

            var queries = []
            for (i = 0; i < documents.length; i++) { 
                let document = documents[i]
                queries.push((cb) =>
                {
                    document.metrics2 = [];
                    recur(document, "misc", document.metrics)
                    document.save();
                    cb(null, "ok");
                })
            }

            async.parallel(queries, () =>
            {
                res.send({});
            })
        })
        .catch(err => {
            console.error('skills error')
            console.error(err)
        })
})

module.exports.router = router