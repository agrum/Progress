module.exports = function () {
	require('./schemas/accounts')();
	require('./schemas/champions')();
	require('./schemas/constellations')();
	require('./schemas/gameSettings')();
	require('./schemas/presets')();
	require('./schemas/skills')();
	require('./schemas/users')();
}