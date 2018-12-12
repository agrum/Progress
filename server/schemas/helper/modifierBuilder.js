var assert = require('assert');

let action = require('..schemas/defs/action');
let alignment = require('..schemas/defs/alignment');
let attribute = require('..schemas/defs/attribute');
let compare = require('..schemas/defs/compare');
let extract = require('..schemas/defs/extract');
let integration = require('..schemas/defs/integration');
let modifierAttribute = require('..schemas/defs/modifierAttribute');
let numeric = require('..schemas/defs/numeric');
let projectile = require('..schemas/defs/projectile');
let stack = require('..schemas/defs/stack');
let subject = require('..schemas/defs/subject');
let trigger = require('..schemas/defs/trigger');
let unitCondition = require('..schemas/defs/unitCondition');

var modBuilder = {};

var validate = (object_, value_) =>
{
	for (var key in object_) {
		if (!object_.hasOwnProperty(key)) continue;
	
		if (value_ == object_[key])
			return true;
	}
	return false;
}

modBuilder.Condition = (left_, compare_, right_) =>
{
	assert(validate(compare, compare_))

	return [
		left_,
		compare_,
		right_,
	]
}

modBuilder.AttributeReference = (base_, subject_, extract_, attribute_) =>
{
	return [
		base_,
		subject_,
		extract_,
		attribute_,
	]
}

modBuilder.AnyOfTrigger = (triggers_) =>
{
	assert(typeof triggers_ === typeof [])

	return {
		anyOf: triggers_,
	}
} 

modBuilder.TriggeredEffect = (triggers_, effects_) =>
{
	assert(typeof triggers_ === typeof [])
	assert(typeof effects_ === typeof [])

	return {
		anyOf: triggers_,
		effects: effects_,
	}
}

modBuilder.Trigger = (trigger_, conditions_) =>
{
	assert(validate(trigger, trigger_))
	assert(typeof max_ === typeof [])

	return {
		trigger: trigger_,
		conditions: conditions_,
	}
} 

modBuilder.Stack = (method_, refresh_, max_, duration_) => 
{
	assert(validate(stack.method, method_))
	assert(validate(stack.refresh, refresh_))
	assert(typeof max_ === typeof 1 && max_ >= 1)
	assert(typeof duration_ === {} || (typeof duration_ === typeof 1 && duration_ >= 1))

	return {
		method: method_,
		refresh: refresh_,
		max: max_,
		isPermanent: typeof duration_ === {},
		duration: (typeof duration_ === {}) ? 0 : duration_,
	}
}

modBuilder.Nmeric = (id_, name_, category_, value_, integration_, upgType_, upgCost_) => 
{
	assert(typeof id_ === typeof "")
	assert(typeof name_ === typeof "")
	assert(validate(numeric.category, category_))
	assert(validate(integration, integration_))
	assert(validate(numeric.upgradeType, upgType_))
	assert(typeof upgCost_ === typeof 1 && upgCost_ >= 0)
	if (typeof value_ === typeof [])
	{
		assert(value_.length == 4)
		assert(typeof value_[0] === typeof 1)
		assert(validate(subject, value[1]))
		assert(validate(extract, value[2]))
		assert(validate(attribute, value[3]))
	}
	else
		assert(typeof value_ === typeof 1)

	return {
		idName: id_,
		name: name_,
		category: category_,
		value: value_,
		integration: integration_,
		upgType: upgType_,
		upgCost: upgCost_,
	}
}

module.exports = modBuilder