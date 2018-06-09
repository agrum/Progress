module.exports = function () {
	require('./schemas/usesr')();
	require('./schemas/gameSettings')();
	require('./schemas/constellations')();
	require('./schemas/abilities')();
	require('./schemas/classes')();
	require('./schemas/kits')();
	require('./schemas/accounts')();
	require('./schemas/presets')();
}