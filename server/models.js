module.exports = function () {
	require('./schemas/users')();
	require('./schemas/gameSettings')();
	require('./schemas/constellations')();
	require('./schemas/abilities')();
	require('./schemas/classes')();
	require('./schemas/kits')();
	require('./schemas/accounts')();
	require('./schemas/presets')();
}