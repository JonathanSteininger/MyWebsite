"use strict";
var LandingPageCanvasStorage;

window.onload = function () {
    let canvas = document.getElementById("LandingPageCanvas");
    let infoBox = document.getElementById("LandingPageInfoBox");
    LandingPageCanvasStorage = new LandingPageCanvas(canvas, infoBox);

    window.addEventListener("resize", UpdateCanvasSize);
    window.addEventListener("mousemove", MouseMoved);
    window.addEventListener("mousedown", MousePressed);
    window.addEventListener("mouseup", MouseReleased);
    if (document.visibilityState != null) {
        PageVisible = document.visibilityState == "visible";
        document.addEventListener("visibilitychange", VisibilityChanged);
    } else {
        console.warn("Page Visibility not determinable. Canvas performance checks may be wrong. Visuals can decrease due to this.");
    }

    PageLoaded();
}


var FrameRate = 50;
var CenterBoxCollision = true;

const particleAmount = 600;
const MaxStartSpeed = 150 / FrameRate;
var lineThickness = 2;
const particleAcceleration = 1;
const TrailDistance = 5;
var CollisionEnergyLoss = 0.5;

var DrawDebug = false;

var FollowMouse = false;

const TargetAmount = 1;
const GravityStrength = 90 / FrameRate;
var GravityMultiplier = 1;
var GravityFallOff = true;
var GravityFallOffScale = 200;
var pointGenerateTimeout = 2;


const TempSpeedUP = 1;


const respawnRadius = 70;
const respawnSpeedMax = 20 / FrameRate;

const PerformanceThreshold = 1.1;

var DeltaTimeActive = false;
var removeParticlesIfSlow = true;
var ReduceFrameRateIfSlow = true;

var minParticles = 250;

const MinFrameRate = 30;
const MinTimeStop = 1000 / (MinFrameRate * TempSpeedUP);
const MinTarget = 1000 / (MinFrameRate * TempSpeedUP) * PerformanceThreshold;

function VisibilityChanged() {
    if (document.visibilityState == "visible") {
        PageVisible = true;

    } else {
        PageVisible = false;
    }
}
var PageVisible = true;
function MousePressed(evt) {
    if (evt.pageY > LandingPageCanvasStorage.Canvas.clientHeight + (LandingPageCanvasStorage.Canvas.getBoundingClientRect().top + window.scrollY)) {
        if(FollowMouse) LandingPageCanvasStorage.PointGenerateTimerTracker = LandingPageCanvasStorage.Time + pointGenerateTimeout;
        FollowMouse = false;
        return;
    }
    FollowMouse = true;
    GravityMultiplier = -3;
    setTimeout(() => GravityMultiplier = 1, 200);
}
function MouseReleased(evt) {
   // GravityMultiplier = 1;
}
function MouseMoved(evt) {
    if (evt.pageY > LandingPageCanvasStorage.Canvas.clientHeight + (LandingPageCanvasStorage.Canvas.getBoundingClientRect().top + window.scrollY)) {
        if(FollowMouse) LandingPageCanvasStorage.PointGenerateTimerTracker = LandingPageCanvasStorage.Time + pointGenerateTimeout;
        FollowMouse = false;
        return;
    }
    FollowMouse = true;
    LandingPageCanvasStorage.TargetPoint.X = evt.pageX;
    LandingPageCanvasStorage.TargetPoint.Y = evt.pageY - (LandingPageCanvasStorage.Canvas.getBoundingClientRect().top + window.scrollY);
}


function UpdateCanvasSize() {
    LandingPageCanvasStorage.UpdateSizeDrawing();
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




class LandingPageCanvas extends Canvas {
    constructor(element, infoBoxElement) {
        super(element);
        this.CenterBox = infoBoxElement;
        this.TopOffset = 0;
        this.UpdateSizeDrawing();
        this.Particles = [];
        this.Targets = [];
        this.CreateTargets();
        this.CreateParticles(particleAmount);
        this.Time = 0;
        this.PointGenerateTimerTracker = 0;
        this.TargetPoint = this.GenerateValidPoint();
        this.FastRendering = false;
        this.TimeStop = 1000 / (FrameRate * TempSpeedUP);
        this.TargetTime = 1000 / (FrameRate * TempSpeedUP) * PerformanceThreshold;
        this.SlowFrameHitsInARow = 0;
        this.Loops = 0;
        this.PastTime = Date.now();
        this.mainLoop(this);
        this.DeltaTimeMultiplier = 1;
        this.Lag = 1;
    }

    mainLoop(self) {
        self.Loops++;
        self.CheckPerformance();
        self.RandomPointGenerate();
        self.MoveParticles();
        if (PageVisible) {
            self.Draw();
        }
        self.Time += 1 / FrameRate;
        setTimeout(self.mainLoop, self.TimeStop * self.Lag, self);
        if (self.Lag != 1) self.Lag = 1;
    }

    
    

    CheckPerformance() {
        let CurrentTime = Date.now();
        let gapTime = CurrentTime - this.PastTime;
        if (DeltaTimeActive) {
            this.DeltaTimeMultiplier = gapTime / this.TargetTime;
        } else if (this.DeltaTimeMultiplier != 1) {
            this.DeltaTimeMultiplier = 1;
        }
        if (!PageVisible) {
            return;
        }
        if (!this.FastRendering) {
            if (gapTime > this.TargetTime && this.Loops > FrameRate) {
                this.SlowFrameHitsInARow++;
                if (this.SlowFrameHitsInARow >= 7) {
                    this.FastRendering = true;
                }
            } else {
                this.SlowFrameHitsInARow = 0;
            }
        } else {
            this.CheckFastRenderingPerformance(gapTime);
        }


        
        this.PastTime = CurrentTime;
    }

    LagF(amount) {
        this.Lag = amount;
    }
    CheckFastRenderingPerformance(gapTime) {
        if (removeParticlesIfSlow) {
            if (gapTime > this.TargetTime * 1.5) {
                this.SlowFrameHitsInARow++;
                if (this.SlowFrameHitsInARow >= 3) {
                    this.RemoveParticle();
                }
            } else {
                this.SlowFrameHitsInARow = 0;
            }
        }

        if (ReduceFrameRateIfSlow && this.TimeStop < MinTimeStop) {
            if (gapTime > this.TargetTime * 1.5) {
                this.SlowFrameHitsInARow++;
                if (this.SlowFrameHitsInARow >= FrameRate/2) {
                    this.DecreaseFrameRate();
                    this.SlowFrameHitsInARow = 0;
                }
            } else {
                this.SlowFrameHitsInARow = 0;
            }
        }
        
    }


    DecreaseFrameRate() {
        if (this.TimeStop == MinTimeStop) {
            return;
        }
        if (this.TimeStop < MinTimeStop) {
            this.TimeStop = MinTimeStop;
            this.TargetTime = MinTarget;
            FrameRate = MinFrameRate;
        }
        FrameRate *= 0.9;
        this.TimeStop *= 0.9;
        this.TargetTime *= 0.9;
        if (this.TimeStop < MinTimeStop) {
            this.TimeStop = MinTimeStop;
            this.TargetTime = MinTarget;
            FrameRate = MinFrameRate;
        }
    }
    RemoveParticle() {
        if (this.Particles.length > minParticles) {
            this.Particles.pop();
        }
    }

    
    

    MoveParticles() {
        let counter = 0;
        let garbage = new Array(this.Particles.length * 4);
        for (let i = 0; i < this.Particles.length; i++) { 
            let particle = this.Particles[i];

            particle.AddToHistory();


            let PastPoint = particle.Location.DeepClone();
            let PastVelocity = particle.Velocity.DeepClone();

            this.ParticleMove(particle, false, false);

            if (this.CheckValidPoint(particle.Location)) continue;

            garbage[counter++] = particle.Location;
            garbage[counter++] = particle.Velocity;
            particle.Location = PastPoint.DeepClone();
            particle.Velocity = PastVelocity.DeepClone();
            this.ParticleMove(particle, true, false);

            if (this.CheckValidPoint(particle.Location)) continue;
            garbage[counter++] = particle.Location;
            garbage[counter++] = particle.Velocity;
            particle.Location = PastPoint.DeepClone();
            particle.Velocity = PastVelocity.DeepClone();
            this.ParticleMove(particle, false, true);

            if (this.CheckValidPoint(particle.Location)) continue;
            garbage[counter++] = particle.Location;
            garbage[counter++] = particle.Velocity;
            particle.Location = PastPoint.DeepClone();
            particle.Velocity = PastVelocity.DeepClone();
            this.ParticleMove(particle, true, true);

            if (this.CheckValidPoint(particle.Location)) continue;
            garbage[counter++] = particle;
            this.Particles[i] = this.CreateParticle();
        }
        garbage = null;
    }

    ParticleMove(particle, flipX, flipY) {

        if (FollowMouse) {
            let X_Delta = this.TargetPoint.X - particle.Location.X;
            let Y_Delta = this.TargetPoint.Y - particle.Location.Y;
            let angle = Math.atan2(Y_Delta, X_Delta);

            let V_X_Delta = Math.cos(angle) * GravityStrength * GravityMultiplier;
            let V_Y_Delta = Math.sin(angle) * GravityStrength * GravityMultiplier;

            let FallOff = 1;
            if (GravityFallOff) {
                let Distance = Math.sqrt(Math.pow(X_Delta, 2) + Math.pow(Y_Delta, 2)) / GravityFallOff;
                FallOff = Math.log(Distance) / Distance;
            }

            particle.Velocity.X += V_X_Delta * FallOff * this.DeltaTimeMultiplier;
            particle.Velocity.Y += V_Y_Delta * FallOff * this.DeltaTimeMultiplier;
        } else {


            for (let i = 0; i < this.Targets.length; i++) { 
                let X_Delta = this.Targets[i].X - particle.Location.X;
                let Y_Delta = this.Targets[i].Y - particle.Location.Y;
                let angle = Math.atan2(Y_Delta, X_Delta);

                let V_X_Delta = Math.cos(angle) * GravityStrength;
                let V_Y_Delta = Math.sin(angle) * GravityStrength;

                let FallOff = 1;
                if (GravityFallOff) {
                    let Distance = Math.sqrt(Math.pow(X_Delta, 2) + Math.pow(Y_Delta, 2)) / GravityFallOff;
                    FallOff = Math.log(Distance) / Distance;
                }

                particle.Velocity.X += V_X_Delta * FallOff * this.DeltaTimeMultiplier;
                particle.Velocity.Y += V_Y_Delta * FallOff * this.DeltaTimeMultiplier;
            }
        }

        if (flipX) {
            particle.Velocity.X *= -1 * CollisionEnergyLoss;
        }
        if (flipY) {
            particle.Velocity.Y *= -1 * CollisionEnergyLoss;
        }
        particle.Location.X += particle.Velocity.X * this.DeltaTimeMultiplier;
        particle.Location.Y += particle.Velocity.Y * this.DeltaTimeMultiplier;
    }
    

    Draw() {
        this.DrawRect(new Point(0, 0), this.Width, this.Height, "#1A1A1B");
        if (DrawDebug) this.RenderDebug();
        if (this.FastRendering) {
            this.RenderLinesFast();
        } else {
            this.RenderLines();
        }
    }
    RenderDebug() {
        let color = "red";
        if (this.FastRendering) color = "blue";
        if (!FollowMouse) {
            for (let i = 0; i < this.Targets.length; i++) {
                this.DrawRect(this.Targets[i], 3, 3, color);
            }
        } else {
            this.DrawRect(this.TargetPoint, 3, 3, color);
        }
    }
    RenderLinesFast() {
        this.ctx.lineWidth = lineThickness * this.ResolutionScale;
        this.ctx.strokeStyle = this.Particles[0].Color;
        this.ctx.beginPath();
        for (let i = 0; i < this.Particles.length; i++) {
            let particleHistory = this.Particles[i].History;
            if (particleHistory.Length < 2) continue;
            this.ctx.moveTo(particleHistory.Get(0).X, particleHistory.Get(0).Y);
            this.ctx.lineTo(particleHistory.Get(particleHistory.Length - 1).X, particleHistory.Get(particleHistory.Length - 1).Y);
        }
        this.ctx.stroke();
    }
    RenderLines() {
        this.ctx.lineWidth = lineThickness * this.ResolutionScale;
        for (let i = 0; i < this.Particles.length; i++) {
            let particle = this.Particles[i];
            let particleHistory = particle.History;
            if (particleHistory.Length < 2) continue;
            this.ctx.strokeStyle = particle.Color;
            this.ctx.beginPath();
            this.ctx.moveTo(particle.History.Get(0).X * this.ResolutionScale, particle.History.Get(0).Y * this.ResolutionScale);
            for (let i = 1; i < particle.History.Length; i++) {
                this.ctx.lineTo(particle.History.Get(i).X * this.ResolutionScale, particle.History.Get(i).Y * this.ResolutionScale);
            }
            this.ctx.stroke();
        }

    }

    RandomPointGenerate() {
        if (this.Time >= this.PointGenerateTimerTracker && !FollowMouse) {
            this.PointGenerateTimerTracker = this.Time + pointGenerateTimeout;
            this.CreateTargets();
            GravityMultiplier = -3;
            setTimeout(() => GravityMultiplier = 1, 200);
        }
    }

    CreateTargets() {
        this.Targets = []
        for (let i = 0; i < TargetAmount; i++) {
            this.Targets.push(this.GenerateValidPoint());
        }
    }

    CreateParticles(amount) {
        for (let i = 0; i < amount; i++) {
            this.Particles.push(this.CreateParticle());
        }
    }
    CreateParticle() {
        let p = new Particle("#00adb5", this.GenerateValidPoint(), particleAcceleration / FrameRate);
        p.Velocity.X = (Math.random() * MaxStartSpeed) - MaxStartSpeed /2;
        p.Velocity.Y = (Math.random() * MaxStartSpeed) - MaxStartSpeed / 2;
        return p;
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
        return this.CheckValidPointEdge(point) && this.CheckValidPointBox(point);
    }



    CheckValidPointBox(point) {
        if (!CenterBoxCollision) return true;
        if (point.X > this.BoxLeft && point.X < this.BoxRight) {
            if (point.Y > this.BoxTop && point.Y < this.BoxBottom) {
                return false;
            }
        }
        return true;
    }
    CheckValidPointEdge(point) {
        if (point.X < 0 || point.Y < this.TopOffset) return false;
        if (point.X >= this.Width) return false;
        if (point.Y >= this.Height) return false;
        return true;
    }

    UpdateSizeDrawing() {
        super.UpdateSizeDrawing();
        this.TopOffset = document.getElementById("nav").clientHeight;
        this.BoxLeft = this.CenterBox.offsetLeft;
        this.BoxRight = this.CenterBox.offsetLeft + this.CenterBox.clientWidth;
        this.BoxTop = this.CenterBox.offsetTop;
        this.BoxBottom = this.CenterBox.offsetTop + this.CenterBox.clientHeight;
    }

}


class Particle {
    constructor(color, startPoint, Acceleration) {
        this.Color = color;
        this.Location = startPoint;
        this.Velocity = new Velocity();
        this.Acceleration = Acceleration;
        this.History = new FixedQueuePoints(TrailDistance);
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
        this.History.Add(this.Location);
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

    GetTotalVelocity() {
        return Math.sqrt(Math.pow(this.X, 2) + Math.pow(this.Y, 2));
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
        if (this.Looped) {
            delete this.List[this.Pointer];
        }
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
class FixedQueuePoints extends FixedQueue {
    constructor(amount) {
        super(amount);
    }
    Add(point) {

        if (this.Looped) {
            this.List[this.Pointer].X = point.X;
            this.List[this.Pointer].Y = point.Y;
        } else {
            this.List[this.Pointer] = point.DeepClone();
        }
        this.Pointer++;
        this.CheckPointer();
        if (!this.Looped && this.Pointer == 0) this.Looped = true;
        if (this.Length < this.MaxLength) this.Length++;
    }

}