let mongoose = require('mongoose')
let validator = require('validator')
let bcrypt = require('bcrypt')

module.exports = function () {
    let positionSchema = new mongoose.Schema({
        x: {
            type: Number
        },
        y: {
            type: Number
        }
    });

    let nodeSchema = new mongoose.Schema({
        id: {
            type: mongoose.Schema.Types.ObjectId
        },
        position: {
            type: positionSchema
        }
    });

    let schema = new mongoose.Schema({
        abilities: [nodeSchema],
        classes: [nodeSchema],
        kits: [nodeSchema],
        shapes: [nodeSchema],
        abilityToAbilityLinks: [[Number]],
        classToAbilityLinks: [[Number]],
        kitsToAbilityLinks: [[Number]],
        startingAbilities: [Number],
    })
    schema.set('collection', 'constellations')

    function isPair(val) {
        return val.length === 2;
    }

    mongoose.model('constellations', schema)
}