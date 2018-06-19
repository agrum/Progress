var express = require('express');
var async = require('async');
var router = express.Router();

router.get('/', function (req, res, next) {
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

router.get('/convert', function (req, res, next) {
    req.app.db.models.skills
        .find({})
        .then(documents => {
            let recur = (document, category, object) =>
            {
                console.log("BEFORE")
                console.log(object)
                var objConstructor = {}.constructor

                for (var key in object) {
                    if (object[key].constructor === objConstructor)
                    {
                        console.log(key + " is object")
                        recur(document, key, object[key])
                    }
                    else
                    {
                        let type = 1;
                        if (key == "cooldown" || key == "castTime" || key == "subtractive" || key == "cost" || key == "fraction")
                            type = 0;
                        console.log(key + " is value")
                        document.metrics2.push({
                            name: key,
                            category: category,
                            value: object[key],
                            upgType: type,
                            upgCost: 1,
                        })
                    }
                }
                console.log("AFTER")
                console.log(document)
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
                console.error('skills no error')
                res.send({});
            })
        })
        .catch(err => {
            console.error('skills error')
            console.error(err)
        })
})

module.exports.router = router