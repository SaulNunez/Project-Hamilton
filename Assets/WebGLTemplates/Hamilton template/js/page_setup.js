let editor = document.getElementById("code_editor");
let sideWindow = document.getElementById("code_editor_sidewindow");
let runButton = document.getElementById("run_program_button");

function initBlocky() {
    let workspace = Blockly.inject(editor,
        { toolbox: document.getElementById('toolbox') });
    window.addEventListener('resize', (e) => {
        Blockly.svgResize(workspace);
    }, false);
}

const CHECK_NOTHING = 0;
const CHECK_IF = 1;
const CHECK_CICLES = 2;
const CHECK_FUNC = 3;
//Acepta 3 argumentos
//Una representación XML inicial para el editor de codigo
//Un tipo de revision donde checamos si el codigo tiene un tipo de sentencia
//Un objeto con el nombre de la variable como llave y el valor esperado como el valor esperado
function loadCodeEditor(loadId, initialStateXml, checkType, variablesExpected) {
    if(initialStateXml){
        const xml = Blocky.Xml.textToDom(initialStateXml);
        Blockly.Xml.domToWorkspace(xml, editor);
    }

    //Iniciar sandbox para codigo
    runButton.onclick = () => {
        const code = Blockly.JavaScript.workspaceToCode(workspace);

    }

    sideWindow.style.display = "flex";
}

function closeCodeEditor() {
    sideWindow.style.display = "none";
}
