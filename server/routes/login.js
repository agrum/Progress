var express = require('express');
var router = express.Router();

router.post('/', function (req, res, next) {
    console.log('post login')
    if (!(req.body.email &&
        req.body.password)) {
        return res.send("Invalid request:" + req.body)
    }

    req._passport.instance.authenticate('local', function (err, user, info) {
        if (err) {
            info.error = err
            res.send(info)
            return
        }

        if (!user) {
            res.send(info)
            return
        }


        req.logIn(user, function(err)
        {
            if(err)
            {
                info.error = err
            }
            return res.send(info)
        })
    })(req, res)
})

module.exports.router = router