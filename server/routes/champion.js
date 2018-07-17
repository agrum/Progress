var express = require('express');
var router = express.Router();

router.get('/:id', function(req, res, next) {
    req.app.db.models.champions
    .findOne({ '_id' : req.params.id })
    .populate({
        path: 'presets',
        model: 'presets'
    })
    .then(document => {
		req.session.west.champion = document
        res.send(document);
        })
    .catch(err => {
        console.error('get champion error')
        console.error(err)
    })
})

router.post('/', function(req, res, next) {
    req.app.db.models.champions
    .create(req.body)
    .then(championDocument => {
        req.app.db.models.accounts
        .update(
			{ _id: req.user.account },
			{ $push: { 'champions': championDocument._id }})
        .then(accountDocument => {
            res.send(championDocument);
          })
        .catch(err => {
            console.error('add champion to account error')
            console.error(err)
        })
    })
    .catch(err => {
        console.error('add champion error')
        console.error(err)
    })
})

router.put('/', function(req, res, next) {
    req.app.db.models.champions
    .update(
		{ _id: req.body._id},
		{ $set: req.body })
    .then(championDocument => {
        res.send(championDocument);
    })
    .catch(err => {
        console.error('update champion error')
        console.error(err)
    })
})

router.delete('/:id', function(req, res, next) {
    req.app.db.models.champions
    .remove({ _id: req.params.id })
    .then(championDocument => {
		req.app.db.models.accounts
		.update(
			{ _id: req.user.account },  
			{ $pull: { 'champions': { _id: req.params.id }}})
        .then(accountDocument => {
            res.send(championDocument);
          })
        .catch(err => {
            console.error('remove preset to champion error')
            console.error(err)
        })
    })
    .catch(err => {
        console.error('remove champion error')
        console.error(err)
    })
})

router.all('/:id', function(req, res, next) {
    if (req.session.west.champion._id == req.params.id)
        return next()
    
	res.send({'error':'operating on non active champion'});
})

router.post('/:id/skillUpgrade', function(req, res, next) {
    req.app.db.models.champions
    .findOne({ '_id' : req.params.id })
    .then(document =>
    {
        //make sure the abs-sum of diffs is equal to points spent
        var pointsSpentCheckSum = 0;
        for (var i = 0; i < req.body.upgrades.length; ++i)
            pointsSpentCheckSum += Math.abs(req.body.upgrades[i].diff)

        //return if mismatch
        if (req.body.pointsSpent != pointsSpentCheckSum)
            return res.status(500).send(
                {error: "Mismatch point spent in report"})

        //return if more points than available were spent
        if (req.body.pointsSpent > document.specializationPoints)
            return res.status(500).send(
                {error: "Try to use more points than available"})

        //find skill upgrade in save (if any)
        let skillUpgrade = {}
        var skillUpgradeIndex = -1;
        for (var i = 0; i < document.skillUpgrades.length; ++i)
        {
            if (document.skillUpgrades[i].skill == req.body.skill)
            {
                skillUpgrade = document.skillUpgrades[i]
                skillUpgradeIndex = i;
                break
            }
        }
        if (skillUpgradeIndex == -1)
        {
            skillUpgrade.skill = req.body.skill;
            skillUpgrade.upgrades = [];
            document.skillUpgrades.push(skillUpgrade)
            skillUpgrade = document.skillUpgrades[document.skillUpgrades.length - 1]
        }

        //apply upgarde
        for (var i = 0; i < req.body.upgrades.length; ++i)
        {
            var found = false;

            //update if already leveled
            for (var j = 0; j < skillUpgrade.upgrades.length; ++j)
            {
                if (skillUpgrade.upgrades[j]._id == req.body.upgrades[i]._id)
                {
                    skillUpgrade.upgrades[j].metric += req.body.upgrades[i].diff
                    foudn = true;
                    break
                }
            }

            //add if new metrixc leveled
            if (!found)
            {
                skillUpgrade.upgrades.push(
                    {
                        metric: req.body.upgrades[i].metric,
                        level: req.body.upgrades[i].diff,
                    })
            }
        }

        //save to mongo
        document.save()

        res.send(document)
    })
})

router.use('/:id/preset', require('./preset').router)

module.exports.router = router