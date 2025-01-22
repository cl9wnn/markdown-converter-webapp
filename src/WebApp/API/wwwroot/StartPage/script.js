import {createLoginForm, createRegisterForm, showForm} from "../Authorization/script.js";

const registerBtn = document.getElementById("registerBtn");
const loginBtn = document.getElementById("loginBtn");

registerBtn.addEventListener('click', async (event) => {
    event.preventDefault();
    await handleSignUp();
});

loginBtn.addEventListener('click', async (event) => {
    event.preventDefault();
    const successRes = await handleSignIn();
    console.log(successRes);
    
});

async function handleSignIn() {
    const result = await showForm(createLoginForm);

    if (result.success) {
        window.location.href = '/documents';
    }

    return result.success;
}
async function handleSignUp() {
    const result = await showForm(createRegisterForm);

    if (result.success) {
        window.location.href = '/documents';
    }
    return result.success;
}

