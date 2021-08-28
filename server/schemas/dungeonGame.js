let mongoose = require('mongoose')
let validator = require('validator')
let bcrypt = require('bcryptjs')

module.exports = function () {
    let participantSchema = new mongoose.Schema({
        user: {
			type: [mongoose.Schema.Types.ObjectId],
			ref: 'users',
        },
        account: {
			type: [mongoose.Schema.Types.ObjectId],
			ref: 'accounts',
        },
        preset: {
			type: [mongoose.Schema.Types.ObjectId],
			ref: 'presets',
        },
        difficulty: Number,
    });

    let schema = new mongoose.Schema({
        participants: [participantSchema],
        difficulty: Number,
    })
    schema.set('collection', 'dungeonGames')

    mongoose.model('dungeonGames', schema)
}