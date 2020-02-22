let socket = null;
let socketGameobjectHandler = "";

function openSocket(gameObjectToCall, connectMethod, disconnectMethod){
    if(!socket){
        socket = io();

        socketGameobjectHandler = gameObjectToCall;

        socket.on('connect', () => {
            unityInstance.SendMessage(gameObjectToCall, connectMethod);
        });

        socket.on('disconnect', (reason) => {
            unityInstance.SendMessage(socketGameobjectHandler, disconnectMethod, reason);
        });
    }
}

function addEvent(event, connectMethod){
    socket.on(event, (data) => {
        unityInstance.SendMessage(socketGameobjectHandler, connectMethod, data.toString());
    });
}

function stopSocket(){
    socket = null;
}