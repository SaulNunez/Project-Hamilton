mergeInto(LibraryManager.library, {
    ShowEditor: function(loadId, initialStateXml, checkType, variablesExpected){
        loadEditor(loadId, Pointer_stringify(initialStateXml), checkType, Pointer_stringify(variablesExpected));
    }
});