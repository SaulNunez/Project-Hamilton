let editor = document.getElementById("code_editor");
let sideWindow = document.getElementById("code_editor_sidewindow");

function makeCanvasFillScreen(){
    let canvas = document.getElementById("game_render");
    canvas.width = window.innerWidth;
    canvas.height = document.innerHeight;
    canvasW = canvas.width;
    canvasH = canvas.height;
}

function initBlocky(){
    let workspace = Blockly.inject(editor,
      {toolbox: document.getElementById('toolbox')});
    window.addEventListener('resize', (e) => {
        Blockly.svgResize(workspace);
    }, false);

}

function loadCodeEditor(initialStateXml, initFunction){
    const xml = Blocky.Xml.textToDom(initialStateXml);
    Blockly.Xml.domToWorkspace(xml, editor);
}

function openCodeEditor(){
    sideWindow.style.display = "block";
}

function closeCodeEditor(){
    sideWindow.style.display = "none";
}

makeCanvasFillScreen();
initBlocky();
