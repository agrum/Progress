var app = require('express')();
var http = require('http').Server(app);
var io = require('socket.io')(http);
var mongo = require('mongodb').MongoClient;
var private = require('./private').private;

app.db = null;
var uri = "mongodb://agrum:"+encodeURI(private.mongoPWD)+"@west-shard-00-00-1wi8d.mongodb.net:27017,west-shard-00-01-1wi8d.mongodb.net:27017,west-shard-00-02-1wi8d.mongodb.net:27017/test?ssl=true&replicaSet=West-shard-0&authSource=admin";
mongo.connect(uri, function(err, client) {
  app.db = db = client.db("west");
  
  http.listen(3000, function () {
    console.log('Example app listening on port 3000!')
  });
});

require('./routes')(app);

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

process.on('exit', function() {
    app.db.close();
});