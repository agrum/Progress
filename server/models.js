module.exports = function () {
	require('./schemas/user')();
	require('./schemas/gameSettings')();
	require('./schemas/constellation')();
	require('./schemas/abilities')();
	require('./schemas/classes')();
	require('./schemas/kits')();
	require('./schemas/shapes')();
}