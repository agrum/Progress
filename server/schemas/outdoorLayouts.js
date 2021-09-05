let mongoose = require('mongoose');

module.exports = function()
{
    let environmentSchema = new mongoose.Schema({
        name: String,
        vertices: [ [ Number ] ],
        variety: String,
        heightDelta: String,
    });

    let linearObstacleSchema = new mongoose.Schema({
        name: String,
        vertices: [ [ Number ] ],
        variety: String,
    });

	let schema = new mongoose.Schema({
		name: String,
		baselineEnvironment: String,
        environments: [ environmentSchema ],
        linearObstacles: [ linearObstacleSchema ],
        center: [ Number ],
        size: [ Number ],
	})
	schema.set('collection', 'outdoorLayouts')

	module.exports = mongoose.model('outdoorLayouts', schema)
}