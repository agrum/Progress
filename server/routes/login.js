var express = require('express');
var router = express.Router();

router.post('/', function (req, res, next) {
    console.log(req.body)
    if (!(req.body.email &&
        req.body.password)) {
        return res.send("Invalid request:" + req.body)
    }

    req._passport.instance.authenticate('local', function (err, user, info) {
        if (err) {
            console.log("A " + err)
            res.json({ success: false, message: err });
            return;
        }

        if (!user) {
            res.send(info)
            console.log("B" + info)
            return
        }


        req.logIn(user, function(err)
        {
            if(err)
            {
                info.error = err
            }
            req.session.west = {}
            console.log("C" + info)
            return res.send(info)
        })
    })(req, res)
})

module.exports.router = router