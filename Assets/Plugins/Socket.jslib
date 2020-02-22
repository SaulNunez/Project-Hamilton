mergeInto(LibraryManager.library, {
    StartSocket: function(gameObjectToCall, connectMethod, disconnectMethod) {
        openSocket(Pointer_stringify(gameObjectToCall),
                    Pointer_stringify(connectMethod), 
                    Pointer_stringify(disconnectMethod));
    },
    EndConnection: function() {
        stopSocket();
    }
});