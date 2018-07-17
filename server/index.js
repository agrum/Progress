var express = require('express')
var app = require('express')()
var http = require('http').Server(app)
var io = require('socket.io')(http)
var mongo = require('mongodb').MongoClient
var private = require('./private').private
let mongoose = require('mongoose')
var bodyParser = require('body-parser')
var cookieParser = require('cookie-parser')
var passport = require('passport')
var session = require('express-session')
var mongoStore = require('connect-mongo')(session);

var server =
	"west-shard-00-00-1wi8d.mongodb.net:27017," +
	"west-shard-00-01-1wi8d.mongodb.net:27017," +
	"west-shard-00-02-1wi8d.mongodb.net:27017"
var database = "west"
var options = "ssl=true&replicaSet=West-shard-0&authSource=admin"
var credentials = "agrum:" + encodeURI(private.mongoPWD)
var uri = "mongodb://" + credentials + "@" + server + "/" + database + "?" + options

app.db = mongoose
app.db.connect(uri)
	.then(() => {
		console.log('Database connection successful')
		require('./models')()

		//configure http
		http.listen(3000, function () {
			console.log('Example app listening on port 3000!')
		})
	})
	.catch(err => {
		console.error('Database connection error')
		console.error(err)
	})


// Configuring Passport
app.use(session(
	{
		secret: private.expressSession,
		cookie: { maxAge: 3600000 * 24 * 3 }, //3days
		saveUninitialized: true,
		resave: true,
		store: new mongoStore({
			url: uri,
			collection: 'sessions'
		})
	}))
app.use(passport.initialize())
app.use(passport.session())
app.passport = passport;
require('./passport')(app);

/*app.put('*', function(req, res, next){
	var data = "";
	req.on('data', function(chunk){ data += chunk})
	req.on('end', function(){
		req.rawBody = data;
		console.log(req.rawBody)
		req.body = JSON.parse(data);
		next();
	})
 })*/
app.use(express.json());
app.put('*', function(req, res, next){
	console.log(req.body)
	next();
 })
app.use(cookieParser())
app.all('*', require('./routes').router)

/*io.on('connection', function(socket){
	console.log('a user connected');
	socket.on('chat message', function(msg){
		console.log('message: ' + msg);
		io.emit('chat message', msg);
	});
	socket.on('disconnect', function(){
		console.log('user disconnected');
	});
});*/