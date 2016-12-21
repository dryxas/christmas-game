function Zaisliukas() {
  this.r = 50;
  this.particles = [];
  this.exploaded = false;
  this.remove = false;
  this.out = false;
  this.numberOfParticles = 30;
  this.pos = createVector(random(this.r, width - this.r), random(this.r, height/2));
  this.vel = createVector(0, 0);
  this.acc = createVector(0, 0);
  this.gravity = createVector(random(-0.01, 0.01), random(0.01, 0.05));

  //this.particles.push(new Particle(random(this.pos.x - this.r, this.pos.x + this.r), random(this.pos.y - this.r, this.pos.y + this.r)));

  this.applyForce = function(force) {
    this.acc.add(force);
  }

  this.update = function() {
    if (!this.exploaded) {
      this.applyForce(this.gravity);
      this.vel.add(this.acc);
      this.pos.add(this.vel);
      this.acc.mult(0);
      if (this.pos.y > height + this.r) {
        this.particles = [];
        this.remove = true;
        this.out = true;
      }
    }
  }

  this.show = function() {
    //debugger;
    if (!this.exploaded) {
      // fill(255, 200);
      // noStroke();
      imageMode(CENTER);
      image(snowflake, this.pos.x, this.pos.y, this.r, this.r);
      //ellipse(this.x, this.y, this.r, this.r);
    } else {
      for (var i = this.particles.length - 1; i >= 0 ; i--) {

        this.particles[i].update();
        this.particles[i].show();
        if (this.particles[i].lifecycle <= 0) {
          this.particles.splice(i, 1);
        }
      }
      if (this.particles.length == 0) {
        this.remove = true;
      }
    }
  }

  this.checkHover = function(x, y) {
    var d = dist(this.pos.x, this.pos.y, x, y);
    if (d < this.r && !this.exploaded) {
      return true;
    } else {
      return false;
    }
  }

  this.expload = function() {
    this.exploaded = true;
    for (var i = 0; i < this.numberOfParticles; i++) {
      this.particles.push(new Particle(random(this.pos.x - this.r, this.pos.x + this.r), random(this.pos.y - this.r, this.pos.y + this.r)));
    }

  }
}
