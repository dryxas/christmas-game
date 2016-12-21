function Eglute() {
  this.r = 200;
  this.step = PI / 40;

  this.show = function() {
    translate(width/2, height/2);
    for (var i = this.r; i >= 0; i--) {
      var x = cos(this.step * i) * i;
      var y = sin(this.step * i) * i;
      var x2 = cos(this.step * i + PI) * i;
      var y2 = sin(this.step * i + PI) * i;
      stroke(255);
      strokeWeight(2);
      point(x, y);
      point(x2, y2);
    }
  }
}
