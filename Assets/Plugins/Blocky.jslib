mergeInto(LibraryManager.library, {
    ShowEditor: function(loadId, initialStateXml, checkType, variablesExpected){
        loadCodeEditor(loadId, Pointer_stringify(initialStateXml), checkType, Pointer_stringify(variablesExpected));
    },
    CloseCodeEditor: function(){
        closeCodeEditor();
    }
});