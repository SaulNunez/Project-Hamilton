var editor = document.getElementById("code_editor");
var sideWindow = document.getElementById("code_editor_sidewindow");
var runButton = document.getElementById("run_program_button");
var documentationParent = document.getElementById("documentacion");
var instructionsParent = document.getElementById("instructions");
var outputAlert = document.getElementById("code_output");
var errorAlert = document.getElementById("code_errors");

function initBlocky() {
    var workspace = Blockly.inject(editor,
        { toolbox: document.getElementById('toolbox') });
    window.addEventListener('resize', (e) => {
        Blockly.svgResize(workspace);
    }, false);
}

initBlocky();

function loadCodeEditor(initialStateXml, instructions, documentation, gameObject, callback) {
    if(initialStateXml){
        const readyWorkspaceXml = Blocky.Xml.textToDom(initialStateXml);
        Blockly.Xml.domToWorkspace(readyWorkspaceXml, editor);
    }

    documentationParent.innerHTML = documentation;
    instructionsParent.innerHTML = instructions;

    //Iniciar sandbox para codigo
    runButton.onclick = () => {
        const code = Blockly.JavaScript.workspaceToCode(workspace);
        unityInstance.SendMessage(gameObject, callback, code);
    }

    sideWindow.style.display = "flex";
    outputAlert.style.display = "none";
    errorAlert.style.display = "none";
}

function setErrorInEditor(errorString){
    errorAlert.innerHTML = errorString;
    errorAlert.style.display = "block";
}

function setOutputInEditor(outputString){
    outputAlert.innerHTML = outputString;
    outputAlert.style.display = "block";
}

function closeCodeEditor() {
    sideWindow.style.display = "none";
}
