let mongoose = require('mongoose');

module.exports = function()
{
	let schema = new mongoose.Schema({
        champions: {
			type: [mongoose.Schema.Types.ObjectId],
			required: true,
			ref: 'champions',
		}
	})
	schema.set('collection', 'accounts')

	module.exports = mongoose.model('accounts', schema)
}