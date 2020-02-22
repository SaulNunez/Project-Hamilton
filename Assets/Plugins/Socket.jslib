mergeInto(LibraryManager.library, {
    StartSocket: function(gameObjectToCall, connectMethod, disconnectMethod) {
        openSocket(Pointer_stringify(gameObjectToCall),
                    Pointer_stringify(connectMethod), 
                    Pointer_stringify(disconnectMethod));
    },
    SendToSocket: function(eventDescriptor, data){
        sendToSocket(Pointer_stringify(eventDescriptor), Pointer_stringify(data));
    },
    AddEvent: function(eventDescriptor, handlerMethod){
        addEvent(Pointer_stringify(eventDescriptor), Pointer_stringify(handlerMethod));
    },
    EndConnection: function() {
        stopSocket();
    }
});