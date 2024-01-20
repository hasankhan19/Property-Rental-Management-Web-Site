const navBar = document.querySelector("nav"),
      menuBtns = document.querySelectorAll(".menu-icon"),
      overlay = document.querySelector(".overlay");

menuBtns.forEach(menuBtn => menuBtn.addEventListener("click", () => navBar.classList.toggle("open")));
overlay.addEventListener("click", () => navBar.classList.remove("open"));

document.addEventListener("DOMContentLoaded", function () {
  const loginButton = document.getElementById("loginButton");
  const loginForm = document.getElementById("loginForm");
  const closeButton = document.getElementById("closeButton");

  function toggleForm(form) {
    form.style.display = (form.style.display === "none" || form.style.display === "") ? "block" : "none";
  }

  loginButton.addEventListener("click", () => toggleForm(loginForm));
  closeButton.addEventListener("click", () => toggleForm(loginForm));
});

function toggleRegistrationBox() {
  const registrationBox = document.getElementById("registrationBox");
  registrationBox.style.display = (registrationBox.style.display === "none" || registrationBox.style.display === "") ? "block" : "none";
}

let isRegistrationBoxOpen = false;

function toggleRegistrationAndLoginForm() {
  const registrationBox = document.getElementById("registrationBox");
  const loginForm = document.getElementById("loginForm");

  loginForm.style.display = "none";

  if (isRegistrationBoxOpen) {
    registrationBox.style.display = "none";
  } else {
    registrationBox.style.display = "block";
    isRegistrationBoxOpen = true;
    document.getElementById("loginButton").disabled = true;
  }
}

function closeRegistrationBox() {
  const registrationBox = document.getElementById("registrationBox");

  registrationBox.style.display = "none";
  isRegistrationBoxOpen = false;
  document.getElementById("loginButton").disabled = false;
}
