var particles = [];

var x = 0;
var y = 0;
var zaisliukai = [];
var maxZaisliuku = 6;
var snowflake;
var tree;
var pointerBall;
var score = 0;
var jsFps = 0;
var gameDuration = 30;
var currentDuration = 0;
var gameStarted = false;
var gameEnded = false;
var startText;
var highScore = 0;
var timeCounter;
//var source;
//var eglute;

function preload() {
  snowflake = loadImage("snowflake_large.png");
  tree = loadImage("tree.gif");
  pointerBall = loadImage("ball.png");
}

function setup() {
  //debugger;
  // var video = document.getElementById('video');
  // tracking.ColorTracker.registerColor("custom", function(r, g, b) {
  //   if (r > 30 && r < 100 && g > 60 && g < 180 && b > 120 && b < 255) {
  //     return true;
  //   }
  //   return false;
  // });
  //
  // var tracker = new tracking.ColorTracker("custom");
  // tracking.track('#video', tracker, { camera: true });
  //
  // tracker.on('track', function(event) {
  //   event.data.forEach(function(rect) {
  //     if (rect.color === 'custom') {
  //       x = parseInt((2 * rect.x + rect.width) / 2);
  //       y = parseInt((2 * rect.y + rect.height) / 2);
  //
  //       x = map(x, 0, width, width, 0);
  //
  //       // document.getElementById("x").value = x;
  //       // document.getElementById("y").value = y;
  //     }
  //   });
  // });

  createCanvas(1280, 720);
  zaisliukai.push(new Zaisliukas());
  startText = new StartText();
  //startGame();
  //eglute = new Eglute();

  //var lastChange = new Date;
  var source = new EventSource("http://localhost:5000/api/blobs");
  source.addEventListener("change", function () {


      // var thisLoop = new Date;
      // if (thisLoop - lastChange < 500) {
      //   count++;
      // } else {
      //   document.getElementById("count").innerHTML = 'Changes: ' + count * 2 + ' fps';
      //   count = 0;
      //   lastChange = thisLoop;
      // }

      //count++;
      //debugger;
      var json = JSON.parse(event.data);
      if (json) {
        x = json[0].x;
        y = json[0].y;
        x = map(x, 0, 1280, width, 0);
        y = map(y, 0, 720, 0, height);
      }
      //count++;
      //document.getElementById("result").innerHTML = event.data;
  });
}

//var lastLoop = new Date;

function draw() {
  background(0);

  // var thisLoop = new Date;
  // if (thisLoop - lastLoop < 500) {
  //   jsFps++;
  // } else {
  //   document.getElementById("jsFps").innerHTML = 'JS: ' + jsFps * 2 + ' fps';
  //   jsFps = 0;
  //   lastLoop = thisLoop;
  // }


  image(tree, width/2, height/2);
  if (!gameStarted) {
    startText.show('Start');

  }
  if (startText.hovered() && !gameStarted) {
    //debugger;
    startGame();
  }

  //eglute.show();

  if ((random(0, 1) < 0.03 && zaisliukai.length < maxZaisliuku) || zaisliukai.length == 0) {
    zaisliukai.push(new Zaisliukas());
  }



  if (gameStarted) {
    textSize(24);
    fill(0, 102, 153);
    text('Score: ' + score, 100, 40);

    if (currentDuration > 0) {
      text(currentDuration + 's', width - 80, 40);
    } else {
      endGame();
    }
  } else if (gameEnded) {
    textSize(40);
    fill (255, 200);
    textAlign(CENTER);
    text('Your score: ' + score, width/2, 100);
    textSize(32);
    text('Highscore: ' + highScore, width/2, 200);
  }

  noStroke();
  fill(255, 150);
  //image(pointerBall, x, y, 30, 30);
  ellipse(x, y, 15, 15);

  for (var i = zaisliukai.length - 1; i >= 0 ; i--) {
    zaisliukai[i].update();
    zaisliukai[i].show();


    if (zaisliukai[i].checkHover(x, y)) {
      zaisliukai[i].expload();
      if (gameStarted) {
        score += 100;
      }

      //zaisliukai.push(new Zaisliukas());
    }
    if (zaisliukai[i].out) {
      if (gameStarted) {
        score -= 20;
      }

    }
    if (zaisliukai[i].remove) {
      zaisliukai.splice(i, 1);
    }


  }


  //console.log(zaisliukai.length);


  // var treeWidth = 300;
  // var treeHeight = 400;
  // strokeWeight(5);
  // stroke(255);
  // noFill();
  // triangle(width/2 - treeWidth/2, treeHeight, width/2, height - treeHeight, width/2 + treeWidth/2, treeHeight);
}

function startGame() {
  score = 0;
  currentDuration = gameDuration;
  gameStarted = true;
  gameEnded = false;
  timeCounter = setInterval(function() {
    currentDuration--;
    if (currentDuration == 0) {
      endGame();
    }
  }, 1000);
}

function endGame() {
  gameStarted = false;
  gameEnded = true;
  clearInterval(timeCounter);
  if (highScore < score) {
    highScore = score;
  }
}

function StartText() {


  this.w = 200;
  this.h = 100;
  this.x = width/2 - this.w/2;
  this.y = height - 110;
  this.fontsize = 40;


  this.show = function(passedText) {
    noFill();
    stroke(255);
    rect(this.x, this.y, this.w, this.h, 5);

    textSize(this.fontsize);
    fill(0, 102, 153);
    textAlign(CENTER);
    //debugger;
    text(passedText, width/2, this.y + this.h / 2 + this.fontsize / 4);
  }

  this.hovered = function() {
    //debugger;
    if (x > this.x && x < this.x + this.w && y > this.y && y < this.y + this.h) {
      return true;
    } else {
      return false;
    }
  }

}
