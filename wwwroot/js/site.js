// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function linkClicked(selectedLink) {
    if (selectedLink == null || selectedLink == undefined) {
        return;
    }
    allLinks = document.getElementsByClassName('nav-link');
    if (allLinks != null && allLinks != undefined) {
        for (let i = 0; i < allLinks.length; i++) {
            allLinks[i].classList.remove('active-link');
        }
    }
    selectedLink.classList.add('active-link');
}