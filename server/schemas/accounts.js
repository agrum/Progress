let mongoose = require('mongoose');

module.exports = function()
{
	let schema = new mongoose.Schema({
        presets: {
			type: [mongoose.Schema.Types.ObjectId],
			required: true,
			ref: 'presets',
		}
	})
	schema.set('collection', 'accounts')

	module.exports = mongoose.model('accounts', schema)
}