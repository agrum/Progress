var app = require('express')()
var http = require('http').Server(app)
var io = require('socket.io')(http)
var mongo = require('mongodb').MongoClient
var private = require('./private').private
let mongoose = require('mongoose')

var server =
	"west-shard-00-00-1wi8d.mongodb.net:27017," +
	"west-shard-00-01-1wi8d.mongodb.net:27017," +
	"west-shard-00-02-1wi8d.mongodb.net:27017"
var database = "west"
var options = "ssl=true&replicaSet=West-shard-0&authSource=admin"
var credentials = "agrum:" + encodeURI(private.mongoPWD)
//var uri = "mongodb://"+credentials+"@"+server+"/"+database
var uri = "mongodb://" + credentials + "@" + server + "/" + database + "?" + options

app.db = mongoose
app.db.connect(uri)
	.then(() => {
		console.log('Database connection successful')
		require('./models')()

		http.listen(3000, function () {
			console.log('Example app listening on port 3000!')
		})
	})
	.catch(err => {
		console.error('Database connection error')
		console.error(err)
	})

require('./routes')(app)

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