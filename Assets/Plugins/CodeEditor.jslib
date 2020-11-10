mergeInto(LibraryManager.library, {
    ShowEditor: function(initialStateXml, instructions, documentation, gameObject, callback){
        loadCodeEditor(Pointer_stringify(initialStateXml), 
            Pointer_stringify(instructions), 
            Pointer_stringify(documentation),
            Pointer_stringify(gameObject),
            Pointer_stringify(callback)
            );
    },
    SetErrorInCodeEditor: function(errorsText) {
        setErrorInEditor(Pointer_stringify(errorsText));
    },
    SetOutputInEditor: function(outputText) {
        setOutputInEditor(Pointer_stringify(outputText));
    },
    CloseCodeEditor: function(){
        closeCodeEditor();
    }
});