mergeInto(LibraryManager.library, {
    StartSocket: function(objectToAlert, event) {
        openSocket(Pointer_stringify(objectToAlert), Pointer_stringify(event));
    },
    EndConnection: function() {
        stopSocket();
    }
});