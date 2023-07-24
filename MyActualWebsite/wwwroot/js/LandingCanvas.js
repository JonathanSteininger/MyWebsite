"use strict";
var LandingPageCanvasStorage;

window.onload = function () {
    let canvas = document.getElementById("LandingPageCanvas");
    let infoBox = document.getElementById("LandingPageInfoBox");
    LandingPageCanvasStorage = new LandingPageCanvas(canvas, infoBox);

    window.addEventListener("resize", UpdateCanvasSize);
}

function UpdateCanvasSize() {
    LandingPageCanvasStorage.UpdateSizeDrawing();
    console.log("hello")
}

class Point {
    constructor(Xpos, Ypos) {
        this.X = Xpos;
        this.Y = Ypos;
    }

    DeepClone() {
        return new Point(this.X, this.Y);
    }

    Compare(OtherPoint) {
        return OtherPoint.X == this.X && OtherPoint.Y == this.Y
    }

    ToString() {
        return `Xpos: ${this.X}, Ypos: ${this.Y}.`;
    }

}

class Canvas {
    constructor(element) {
        this.Canvas = element;
        this.ResolutionScale = 1;
        this.UpdateSizeDrawing();
        this.ctx = this.Canvas.getContext("2d");
    }



    //basic drawing
    DrawRect(point, width, height, color) {
        this.ctx.fillStyle = color;
        this.ctx.fillRect(point.X * this.ResolutionScale, point.Y * this.ResolutionScale, width * this.ResolutionScale, height * this.ResolutionScale);
    }
    DrawLine(startPoint, endPoint, width, color) {
        this.ctx.strokeStyle = color;
        this.ctx.lineWidth = width;
        this.ctx.beginPath();
        this.ctx.moveTo(startPoint.X * this.ResolutionScale, startPoint.Y * this.ResolutionScale);
        this.ctx.lineTo(endPoint.X * this.ResolutionScale, endPoint.Y * this.ResolutionScale);
        this.ctx.stroke();
    }
    DrawPath(points, width, color) {
        if (points.length < 2) throw new Error(`Less than 2 Points in Points array. Array: ${points} `) 
        this.ctx.lineWidth = width;
        this.ctx.strokeStyle = color;
        this.ctx.beginPath();
        this.ctx.moveTo(points[0].X * this.ResolutionScale, points[0].Y * this.ResolutionScale);
        for (let i = 1; i < points.length; i++) {
            this.ctx.lineTo(points[i].X * this.ResolutionScale, points[i].Y * this.ResolutionScale);
        }
        this.ctx.stroke();
    }
    FillShape(points, color) {
        if (points.length < 2) throw new Error(`Less than 2 Points in Points array. Array: ${points} `)
        this.ctx.fillStyle = color;
        this.ctx.beginPath();
        this.ctx.moveTo(points[0].X * this.ResolutionScale, points[0].Y * this.ResolutionScale);
        for (let i = 1; i < points.length; i++) {
            this.ctx.lineTo(points[i].X * this.ResolutionScale, points[i].Y * this.ResolutionScale);
        }
        this.ctx.closePath();
        this.fill();
    }
    OutlineShape(points, lineWidth, lineColor) {
        if (points.length < 2) throw new Error(`Less than 2 Points in Points array. Array: ${points} `)
        this.ctx.strokeStyle = lineColor;
        this.ctx.lineWidth = lineWidth;
        this.ctx.beginPath();
        this.ctx.moveTo(points[0].X * this.ResolutionScale, points[0].Y * this.ResolutionScale);
        for (let i = 1; i < points.length; i++) {
            this.ctx.lineTo(points[i].X * this.ResolutionScale, points[i].Y * this.ResolutionScale);
        }
        this.ctx.closePath();
        this.stroke();
    }
    DrawShape(points, lineWidth, lineColor, fillColor) {
        if (points.length < 2) throw new Error(`Less than 2 Points in Points array. Array: ${points} `)
        this.ctx.strokeStyle = lineColor;
        this.ctx.lineWidth = lineWidth;
        this.ctx.fillStyle = fillColor;
        this.ctx.beginPath();
        this.ctx.moveTo(points[0].X * this.ResolutionScale, points[0].Y * this.ResolutionScale);
        for (let i = 1; i < points.length; i++) {
            this.ctx.lineTo(points[i].X * this.ResolutionScale, points[i].Y * this.ResolutionScale);
        }
        this.ctx.closePath();
        this.stroke();
        this.fill();
    }


    UpdateSizeDrawing() {
        this.Height = this.Canvas.clientHeight;
        this.Width = this.Canvas.clientWidth;
        this.Canvas.width = this.Canvas.clientWidth * this.ResolutionScale;
        this.Canvas.height = this.Canvas.clientHeight * this.ResolutionScale;
    }
}
const FrameRate = 60;
const pointGenerateTimeout = 25;
const particleAmount = 25;
const particleAcceleration = 1;
const TrailDistance = 200;
var CenterBoxCollision = true;
var CollisionEnergyLoss = 0.7;

const GravityStrength = 1 / FrameRate;


class LandingPageCanvas extends Canvas {
    constructor(element, infoBoxElement) {
        super(element);
        this.CenterBox = infoBoxElement;
        this.Particles = [];
        this.CreateParticles(particleAmount);
        this.Time = 0;
        this.PointGenerateTimerTracker = 0;
        this.TargetPoint = this.GenerateValidPoint();
        this.mainLoop(this);
    }

    mainLoop(self) {
        self.RandomPointGenerate();
        self.MoveParticles();
        self.Draw();
        self.Time += 1 / FrameRate;
        setTimeout(self.mainLoop, 1000 / FrameRate, self);
    }

    RandomPointGenerate() {
        if (this.Time >= this.PointGenerateTimerTracker) {
            this.PointGenerateTimerTracker = this.Time + pointGenerateTimeout;
            this.TargetPoint = this.GenerateValidPoint();
        }
    }

    MoveParticles() {

        for (let i = 0; i < this.Particles.length; i++) { 
            let particle = this.Particles[i];
            particle.AddToHistory();

            let PastPoint = particle.Location.DeepClone();
            let PastVelocity = particle.Velocity.DeepClone();

            this.ParticleMove(particle, false, false)

            if (this.CheckValidPoint(particle.Location)) continue;

            particle.Location = PastPoint.DeepClone();
            particle.Velocity = PastVelocity.DeepClone();
            this.ParticleMove(particle, true, false)

            if (this.CheckValidPoint(particle.Location)) continue;

            particle.Location = PastPoint.DeepClone();
            particle.Velocity = PastVelocity.DeepClone();
            this.ParticleMove(particle, false, true)

            if (this.CheckValidPoint(particle.Location)) continue;

            particle.Location = PastPoint.DeepClone();
            particle.Velocity = PastVelocity.DeepClone();
            this.ParticleMove(particle, true, true)

            if (this.CheckValidPoint(particle.Location)) continue;
            this.Particles[i] = this.CreateParticle();

        }
    }

    ParticleMove(particle, flipX, flipY) {

        let X_Delta = this.TargetPoint.X - particle.Location.X;
        let Y_Delta = this.TargetPoint.Y - particle.Location.Y;
        let angle = Math.atan2(Y_Delta, X_Delta);
        particle.Velocity.X += Math.cos(angle) * GravityStrength;
        particle.Velocity.Y += Math.sin(angle) * GravityStrength;

        if (flipX) {
            particle.Velocity.X *= -1 * CollisionEnergyLoss;
        }
        if (flipY) {
            particle.Velocity.Y *= -1 * CollisionEnergyLoss;
        }
        particle.UpdatePos();
    }
    

    Draw() {
        this.DrawRect(new Point(0, 0), this.Width, this.Height, "#1A1A1B");
        this.DrawRect(this.TargetPoint, 5, 5, "blue");
        for (let i = 0; i < this.Particles.length; i++) {
            let particle = this.Particles[i];
            if (particle.History.Length < 2) continue;
            this.ctx.lineWidth = 1 * this.ResolutionScale;
            this.ctx.strokeStyle = particle.Color;
            this.ctx.beginPath();
            this.ctx.moveTo(particle.History.Get(0).X * this.ResolutionScale, particle.History.Get(0).Y * this.ResolutionScale);
            for (let i = 1; i < particle.History.Length; i++) {
                this.ctx.lineTo(particle.History.Get(i).X * this.ResolutionScale, particle.History.Get(i).Y * this.ResolutionScale);
            }
            this.ctx.stroke();
        }
    }

    CreateParticles(amount) {
        for (let i = 0; i < amount; i++) {
            this.Particles.push(this.CreateParticle());
        }
    }
    CreateParticle() {
        return new Particle("#00adb5", this.GenerateValidPoint(), particleAcceleration / FrameRate);
    }

    GenerateValidPoint() {
        let point = new Point(0, 0);
        while (true) {
            point.X = Math.random() * this.Width;
            point.Y = Math.random() * this.Height;
            if (this.CheckValidPoint(point)) {
                break;
            }
        }
        return point;
    }

    CheckValidPoint(point) {
        return this.CheckValidPointBox(point) && this.CheckValidPointEdge(point);
    }
    CheckValidPointBox(point) {
        if (!CenterBoxCollision) return true;
        if (point.X > this.CenterBox.offsetLeft && point.X < this.CenterBox.offsetLeft + this.CenterBox.clientWidth) {
            if (point.Y > this.CenterBox.offsetTop && point.Y < this.CenterBox.offsetTop + this.CenterBox.clientHeight) {
                return false;
            }
        }
        return true;
    }
    CheckValidPointEdge(point) {
        if (point.X < 0 || point.Y < 0) return false;
        if (point.X >= this.Width) return false;
        if (point.Y >= this.Height) return false;
        return true;
    }


}


class Particle {
    constructor(color, startPoint, Acceleration) {
        this.Color = color;
        this.Location = startPoint;
        this.Velocity = new Velocity();
        this.Acceleration = Acceleration;
        this.History = new FixedQueue(TrailDistance);
        this.FlipHorizontal = false;
        this.FlipVerticle = false;
    }

    MoveTo(point) {
        this.UpdateSpeed(point);
        this.UpdatePos();
    }

    UpdateSpeed(point) {
        this.Velocity.X += this.Acceleration * this.HorizontalPointCheck(point);
        this.Velocity.Y += this.Acceleration * this.VerticalPointCheck(point);
        if (this.FlipHorizontal) {
            this.Velocity.X *= -1 * CollisionEnergyLoss;
            this.FlipHorizontal = false;
        }
        if (this.FlipVerticle) {
            this.Velocity.Y *= -1 * CollisionEnergyLoss;
            this.FlipVerticle = false;
        }

    }
    UpdatePos() {
        this.Location.X += this.Velocity.X;
        this.Location.Y += this.Velocity.Y;
    }

    AddToHistory() {
        this.History.Add(this.Location.DeepClone());
    }

    HorizontalPointCheck(point) {
        if (point.X < this.Location.X) return -1;
        if (point.X > this.Location.X) return 1;
        return 0;
    }
    VerticalPointCheck(point) {
        if (point.Y < this.Location.Y) return -1;
        if (point.Y > this.Location.Y) return 1;
        return 0;
    }

}

class Velocity{
    constructor(XSpeed = 0, YSpeed = 0) {
        this.X = XSpeed;
        this.Y = YSpeed;
    }

    DeepClone() {
        return new Velocity(this.X, this.Y);
    }
}

class FixedQueue {
    constructor(QueueLength) {
        this.List = new Array(QueueLength);
        this.MaxLength = QueueLength;
        this.Length = 0;
        this.Pointer = 0;
        this.Looped = false;
    }
    Add(object) {
        this.List[this.Pointer] = object;
        this.Pointer++;
        this.CheckPointer();
        if (!this.Looped && this.Pointer == 0) this.Looped = true;
        if (this.Length < this.MaxLength) this.Length++;
    }
    CheckPointer() {
        if (this.Pointer >= this.MaxLength) {
            this.Pointer = 0;
        }
    }
    Get(index) {
        if (index < 0) {
            throw new Error(`index Out of range. index was less than 0. Index: ${index}`);
        }
        if (index >= this.Length) {
            throw new Error(`index Out of range. index was Higher than current Length. Index: ${index}, CurrentLength: ${this.Length}`);
        }
        if (this.Looped) {
            let MovedIndex = this.Pointer + index;
            if (MovedIndex < 0) {
                MovedIndex += this.MaxLength;
            } else if (MovedIndex >= this.MaxLength) {
                MovedIndex -= this.MaxLength;
            }
            return this.List[MovedIndex];
        }
        return this.List[index];
    }

}