

jarEl.src = `https://i.imgur.com/HqW6jqX.png`;

cupBase[1] += 10;
boxBody = new p2.Body({ mass: 0, position: [cupBase[0], cupBase[1]] });
boxShape = new p2.Box({ width: 140, height: 30 });
boxShape.material = this.materials.wall;
boxBody.addShape(boxShape);
this.world.addBody(boxBody);
this.world.addBody(this.createRectangleBody([cupBase[0] + 200, cupBase[1] + 133], 400, 30, 85));
this.world.addBody(this.createRectangleBody([cupBase[0] - 200, cupBase[1] + 133], 400, 30, -85));
