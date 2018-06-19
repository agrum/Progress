module.exports = function () {
	require('./schemas/users')();
	require('./schemas/gameSettings')();
	require('./schemas/constellations')();
	require('./schemas/skills')();
	require('./schemas/champions')();
	require('./schemas/accounts')();
	require('./schemas/presets')();
}