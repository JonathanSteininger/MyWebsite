
class Toggelable {
    constructor(classString) {
        this.closed = true;
        if (classString == "none") {
            this.classElement = null;
        } else {
            this.classElement = document.getElementsByClassName(classString)[0];
        }
        this.className = classString;
    }

    toggle() {
        if (this.classElement == null) return;
        if (this.closed) {
            this.Open();
        } else {
            this.Close();
        }
    }
    Open() {
        this.closed = false;
        this.classElement.style.height = "auto";

    }
    Close() {
        this.closed = true;
        this.classElement.style.height = "0px";
    }
}


window.onload = PageLoaded;

var footer;
var PastHeight;


/**
 * runs when the page loads. 
 * used to initialize variables.
 * create event listeners.
 * etc
 */
function PageLoaded() {
    SetupFooter();
    //SetUpStatBars();
}

/*

var statBarList;
function SetUpStatBars() {
    statBarList = document.getElementsByClassName("statBar");

}

function DrawBars() {
    statBarList.forEach(bar => DrawBar(bar));
}

function DrawBar(bar) {
    var barCanvas = bar.children[0];
    var percentage = bar.

}
*/

/**
 * sets up the footers size checks.
 */
function SetupFooter() {
    GetFooter();
    window.addEventListener("resize", ResizeEvent);
    FooterResizeCheck();

}


/**
 * runs when the page resizes.
 * @param {any} evtArgs used the pass data about the resize event.
 */
function ResizeEvent(evtArgs) {
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
function ToggleClosed(className) {
    if (ContainsClass(className)) {
        GetElement(className).toggle();
    } else {
        addToggelable(className).toggle();
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

