let mongoose = require('mongoose');

module.exports = function()
{
	let schema = new mongoose.Schema({
		name: String,
		numberOfTeams: Number,
		numberOfPlayersPerTeam: Number,
		map: String,
        presetLength: Number,
        numberOfAbilities: Number,
        numberOfClasses: Number,
        numberOfKits: Number,
	})
	schema.set('collection', 'gameSettings')

	module.exports = mongoose.model('gameSettings', schema)
}