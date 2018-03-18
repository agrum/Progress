let mongoose = require('mongoose');

module.exports = function()
{
	let gameSettings = new mongoose.Schema({
		name: String,
		numberOfTeams: Number,
		numberOfPlayersPerTeam: Number,
		map: String
	})
	gameSettings.set('collection', 'gameSettings')

	module.exports = mongoose.model('gameSettings', gameSettings)
}