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
        //Cargar codigo al sandbox y agregar mi funcion de verificacion alla

        //Revisar que el codigo tiene los objetos necesarios
        if (checkType != CHECK_NOTHING) {
            let syntaxTree = acorn.parse(code);
            syntaxTree.find((expr) => {
                let shouldContinue = false;

                switch (checkType) {
                    case CHECK_IF:
                        shouldContinue = expr.type == "ConditionalExpression"
                            || expr.type == "IfStatement";
                        break;
                    case CHECK_CICLES:
                        shouldContinue = expr.type == "ForStatement"
                            || expr.type == "WhileStatement"
                            || expr.type == "DoWhileStatement";
                        break;
                    case CHECK_FUNC:
                        shouldContinue = expr.type == "FunctionExpression";
                        break;
                }

                return shouldContinue;
            });
        }

        const interpreter = new Interpreter(code, (interpreter, scope) => {
            interpreter.setProperty(scope, 'highlightBlock',
                interpreter.createNativeFunction(id => (editor.highlightBlock(id))));

            // Add an API function for the alert() block.
            interpreter.setProperty(scope, 'alert',
                interpreter.createNativeFunction(text => alert(arguments.length ? text : '')));

            // Add an API function for the prompt() block.
            interpreter.setProperty(scope, 'prompt',
                interpreter.createNativeFunction((text) => prompt(text)));
        });

        interpreter.run();
    }

    sideWindow.style.display = "flex";
}

function closeCodeEditor() {
    sideWindow.style.display = "none";
}

initBlocky();
