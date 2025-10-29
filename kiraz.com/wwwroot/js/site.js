// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// site.js

document.addEventListener("DOMContentLoaded", function () {
    // DOM yüklendiğinde çalışacak kod
    
    // Gerekli HTML elementlerini seçiyoruz
    var hamburgerButton = document.getElementById("hamburger-menu");
    var navigationMenu = document.getElementById("main-nav");

    // Hamburger butonuna tıklanma olayını dinliyoruz
    hamburgerButton.addEventListener("click", function () {
        // 'show-menu' class'ını ekleyip/kaldırıyoruz
        navigationMenu.classList.toggle("show-menu");
    });
});