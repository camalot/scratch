let shaker = "clear"; // red or clear


/***************************/

let shakers = {
    red: 'vT9tPIQ',
    clear: 'xoFXCla'
};

jarEl.src = `https://i.imgur.com/${shakers[shaker]}.png`;
boxBody = new p2.Body({ mass: 0, position: [cupBase[0], cupBase[1] - 15] });
boxShape = new p2.Box({ width: 100, height: 5 });
boxShape.material = this.materials.wall;
boxBody.addShape(boxShape);

this.world.addBody(boxBody);
// right vwall
this.world.addBody(this.createRectangleBody([cupBase[0] + 40, cupBase[1] + 40], 120, 10, 85));
// left vwall
this.world.addBody(this.createRectangleBody([cupBase[0] - 40, cupBase[1] + 40], 120, 10, -85));