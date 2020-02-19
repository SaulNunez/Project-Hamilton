let socket = null;
let gameObjects = "";

function openSocket(gameObjectToCall, event){
    if(!socket){
        socket = io();

        socket.on('game', (socket) => {
        
        });
    }
}

function stopSocket(){
    socket = null;
}