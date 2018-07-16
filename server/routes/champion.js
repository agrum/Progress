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
		console.error(document)
		req.session.west.champion = document
        console.error('get champion success')
        res.send(document);
        })
    .catch(err => {
        console.error('get champion error')
        console.error(err)
    })
})

router.post('/', function(req, res, next) {
	console.log(req.body)

    var champion = JSON.parse(req.body.champion);
    req.app.db.models.champions
    .create(champion)
    .then(championDocument => {
        console.error('add champion success')
        console.error(championDocument)
        req.app.db.models.accounts
        .update(
			{ _id: req.user.account },
			{ $push: { 'champions': championDocument._id }})
        .then(accountDocument => {
            //accountDocument.champions.push(championDocument._id);
            //accountDocument.save();
            console.error('add champion to account success')
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
    var champion = JSON.parse(req.body.champion);
    req.app.db.models.champions
    .update(
		{ _id: champion._id},
		{ $set: champion })
    .then(championDocument => {
        //championDocument.set(champion);
        //championDocument.save();
        console.error('update champion success')
        res.send(championDocument);
    })
    .catch(err => {
        console.error('update champion error')
        console.error(err)
    })
})

router.delete('/:id', function(req, res, next) {
    console.error('remove champion')
    req.app.db.models.champions
    .remove({ _id: req.params.id })
    .then(championDocument => {
		console.error('remove champion success')
		req.app.db.models.accounts
		.update(
			{ _id: req.user.account },  
			{ $pull: { 'champions': { _id: req.params.id }}})
        .then(accountDocument => {
            console.error('remove preset to champion success')
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

router.post('/:id/skillUpgrade', function(req, res, next) {
    console.log(">> router.post('/:id/skillUpgrade'")
    req.app.db.models.champions
    .findOne({ '_id' : req.params.id })
    .then(document =>
    {
        let report = JSON.parse(req.body.report);
        console.log(report)

        //make sure the abs-sum of diffs is equal to points spent
        var pointsSpentCheckSum = 0;
        for (var i = 0; i < report.upgrades.length; ++i)
            pointsSpentCheckSum += Math.abs(report.upgrades[i].diff)

        //return if mismatch
        if (report.pointsSpent != pointsSpentCheckSum)
            return res.status(500).send(
                {error: "Mismatch point spent in report"})

        //return if more points than available were spent
        if (report.pointsSpent > document.specializationPoints)
            return res.status(500).send(
                {error: "Try to use more points than available"})

        //find skill upgrade in save (if any)
        let skillUpgrade = {}
        var skillUpgradeIndex = -1;
        for (var i = 0; i < document.skillUpgrades.length; ++i)
        {
            if (document.skillUpgrades[i].skill == report.skill)
            {
                skillUpgrade = document.skillUpgrades[i]
                skillUpgradeIndex = i;
                break
            }
        }
        if (skillUpgradeIndex == -1)
        {
            skillUpgrade.skill = report.skill;
            skillUpgrade.upgrades = [];
            document.skillUpgrades.push(skillUpgrade)
            skillUpgrade = document.skillUpgrades[document.skillUpgrades.length - 1]
        }

        //apply upgarde
        for (var i = 0; i < report.upgrades.length; ++i)
        {
            var found = false;

            //update if already leveled
            for (var j = 0; j < skillUpgrade.upgrades.length; ++j)
            {
                if (skillUpgrade.upgrades[j]._id == report.upgrades[i]._id)
                {
                    skillUpgrade.upgrades[j].metric += report.upgrades[i].diff
                    foudn = true;
                    break
                }
            }

            //add if new metrixc leveled
            if (!found)
            {
                skillUpgrade.upgrades.push(
                    {
                        metric: report.upgrades[i].metric,
                        level: report.upgrades[i].diff,
                    })
            }
        }

        //save to mongo
        document.save()

        console.log("<< router.post('/:id/skillUpgrade'")
        res.send(document)
    })
})

module.exports.router = router