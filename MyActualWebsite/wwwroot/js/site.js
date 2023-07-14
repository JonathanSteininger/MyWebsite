

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
    console.log(evtArgs);
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