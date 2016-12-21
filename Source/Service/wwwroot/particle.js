function Particle(x, y) {
  this.pos = createVector(x, y);
  this.vel = createVector(random(-2, 2), random(0, -5));
  this.acc = createVector(0, 0);
  this.gravity = createVector(0, 0.13);
  this.lifecycle = 255;

  this.applyForce = function(force) {
    this.acc.add(force);
  }

  this.update = function() {
    this.applyForce(this.gravity);
    this.vel.add(this.acc);
    this.pos.add(this.vel);
    this.acc.mult(0);
    this.lifecycle -= 4;
  }

  this.show = function() {
    strokeWeight(2);
    stroke(255, this.lifecycle);
    point(this.pos.x, this.pos.y);
  }
}
