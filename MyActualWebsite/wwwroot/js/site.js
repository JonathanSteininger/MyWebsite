
class Toggelable {
    constructor(classString) {
        this.closed = true;
        if (classString == "none") {
            this.classElement = null;
        } else {
            this.classElement = document.getElementsByClassName(classString)[0];
            if (this.classElement == null) {
            this.classElement = document.getElementById(classString);
            }
        }
        this.className = classString;
    }

    toggleClass(classToToggle) {
        let temp = classToToggle.split(' ');
        for (let i = 0; i < temp.length; i++) {
            this.classElement.classList.toggle(temp[i]);
        }
    }
}

class DisplayManager {
    constructor() {
        let temp = document.getElementsByClassName("horizontal-display");
        this.Displays = [];
        this.Keys = [];
        for (let i = 0; i < temp.length; i++) {
            this.Displays.push(new HorizontalDisplay(temp[i]));
            this.Keys.push(temp[i].id);
        }
    }

    UpdateDisplaysPosions() {
        for (let i = 0; i < this.Displays.length; i++) {
            this.Displays[i].UpdatePosition();
        }
    }

    MoveUp(id) {
        this.getDisplay(id).MoveUp()
    }
    MoveDown(id) {
        this.getDisplay(id).MoveDown()
    }
    UpdatePostion(id) {
        this.getDisplay(id).UpdatePosition();
    }
    getDisplay(id) {
        for (let i = 0; i < this.Keys.length; i++) {
            if (this.Keys[i] == id) {
                return this.Displays[i];
            }
        }
        console.error("Display ID not found. ID:", id)
        return this.Displays[0];
    }
}
class HorizontalDisplay {
    constructor(element) {
        this.window = element;
        this.panel = element.children[0];
        this.elementKey = element.id;
        this.currentState = 0;
        this.maxStates = this.panel.children.length;
        this.UpdatePosition();
    }
    GetChildWidth() {
        if (this.panel.children.length < 1) return 0;
        var child = this.panel.children[0];
        var width = child.offsetWidth + ((parseInt(getComputedStyle(child).marginLeft) + parseInt(getComputedStyle(child).marginRight)));
        return width;
    }

    MoveUp() {
        if (this.panel.children.length < 1) return;
        this.currentState++;
        if (this.currentState >= this.maxStates) {
            this.currentState = 0;
        }
        this.UpdatePosition();
    }
    MoveDown() {
        if (this.panel.children.length < 1) return;
        this.currentState--;
        if (this.currentState < 0) {
            this.currentState = this.maxStates - 1;
        }
        this.UpdatePosition();
    }
    UpdatePosition() {
        if (this.panel.children.length < 1) return;
        let center = this.window.clientWidth / 2;
        let childWidth = this.GetChildWidth();

        let ChildCenterOffset = childWidth/2;
        let centerChildPos = center - ChildCenterOffset;

        let childrenOffset = childWidth * this.currentState;
        let finalPos = centerChildPos - childrenOffset;

        this.panel.style.marginLeft = finalPos + "px";
    }
}




window.onload = PageLoaded;

var footer;
var PastHeight;

function DisplayLeft(id) {
    if (HorizontalDisplays == null) return;
    HorizontalDisplays.MoveDown(id);
}
function DisplayRight(id) {
    if (HorizontalDisplays == null) return;
    HorizontalDisplays.MoveUp(id);
}

/**
 * runs when the page loads. 
 * used to initialize variables.
 * create event listeners.
 * etc
 */
function PageLoaded() {
    window.addEventListener("resize", ResizeEvent);
    SetupFooter();
    SetUpHorizontalDisplays();
}

/**
 * runs when the page resizes.
 * @param {any} evtArgs used the pass data about the resize event.
 */
function ResizeEvent(evtArgs) {
    FooterResizeCheck();
    if (HorizontalDisplays != null) HorizontalDisplays.UpdateDisplaysPosions();
}

var HorizontalDisplays;
function SetUpHorizontalDisplays() {
    HorizontalDisplays = new DisplayManager();
    HorizontalDisplays.UpdateDisplaysPosions();
}

/**
 * sets up the footers size checks.
 */
function SetupFooter() {
    GetFooter();
    FooterResizeCheck();
}


/**
 * checks if the footer has changed size.
 * if the footers height has changed, will update the body with a new bottom margin size equal to the footer size.
 * 
 * @returns exit statment
 */
function FooterResizeCheck() {
    if (PastHeight == null) {
        OnFooterSizeChanged();
        return;
    }
    if (GetFooterHeight() != PastHeight) {
        OnFooterSizeChanged();
    }

}
/**
 * updates elements based on the new footer size.
 * 
 * curretnly updated the bodies bottom margin to the footers height.
 */
function OnFooterSizeChanged() {
    let size = footer.clientHeight;
    document.body.style.marginBottom = size + "px";


    PastHeight = GetFooterHeight();
}

/**
 * return the footer's height
 * 
 * @returns footer's height
 */
function GetFooterHeight() {
    if (footer == null) GetFooter();
    return footer.clientHeight;
}
/**
 * sets the footer from the page.
 */
function GetFooter() {
    let list = document.getElementsByTagName("footer");
    footer = list[0];
    return footer;
}




var toggelablesArray = [new Toggelable("none")];

function ToggleClass(className, classToggled) {
    if (ContainsClass(className)) {
        GetElement(className).toggleClass(classToggled);
    } else {
        addToggelable(className).toggleClass(classToggled);
    }
}

function ToggleAllOfClass(className, classesToggled) {
    var elements = document.getElementsByClassName(className);
    for (let i = 0; i < elements.length; i++) {
        let classes = classesToggled.split(' ');
        for (let k = 0; k < classes.length; k++) {
            elements[i].classList.toggle(classes[k]);
        }
    }
}

function addToggelable(className) {
    var temp = new Toggelable(className);
    toggelablesArray.push(temp);
    return temp;
}

function GetElement(className) {
    for (let i = 0; i < toggelablesArray.length; i++) {
        if (toggelablesArray[i].className == className) {
            return toggelablesArray[i];
        }
    }
    return null;
}

function ContainsClass(className) {
    for (let i = 0; i < toggelablesArray.length; i++) {
        if (toggelablesArray[i].className == className) {
            return true;
        }
    }
    return false;
}

