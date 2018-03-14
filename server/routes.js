'use strict';

exports = module.exports = function(app) {
    app.use('/gameSettings', require('./routes/gameSettings').gameSettings);
    
    app.use(function(req, res, next) {
        var err = new Error('Not Found');
        err.status = 404;
        next(err);
    });
};
