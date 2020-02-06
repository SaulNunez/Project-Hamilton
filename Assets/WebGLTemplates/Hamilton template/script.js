function makeCanvasFillScreen(){
    let canvas = document.getElementById("game_render");
    canvas.width = window.innerWidth;
    canvas.height = document.innerHeight;
    canvasW = canvas.width;
    canvasH = canvas.height;
}

function initBlocky(){
    let codeDiv = document.getElementById('code_editor');
    let workspace = Blockly.inject(codeDiv,
      {toolbox: document.getElementById('toolbox')});
    window.addEventListener('resize', (e) => {
        Blockly.svgResize(workspace);
    }, false);

}

makeCanvasFillScreen();
initBlocky();
console.log("Hello world");
