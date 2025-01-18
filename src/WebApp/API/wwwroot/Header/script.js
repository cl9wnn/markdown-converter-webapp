import { tokenStorage, showForm, createLoginForm, createRegisterForm } from "../Authorization/script.js";

document.addEventListener('DOMContentLoaded', () => {
    updateHeaderButtons();
});

async function handleSignIn() {
    const success = await showForm(createLoginForm);
    if (success) {
        await updateHeaderButtons(); 
    }
    return success;
}

async function handleSignUp() {
    const success = await showForm(createRegisterForm);
    if (success) {
        await updateHeaderButtons(); 
    }
    return success;
}

function updateHeaderButtons() {
    const headerButtonsContainer = document.querySelector('.header-buttons');
    const token = tokenStorage.get();

    headerButtonsContainer.innerHTML = ''; 

    if (token) {
        const accountBtn = document.createElement('button');
        accountBtn.className = 'header-buttons';
        accountBtn.id = 'accountBtn';
        accountBtn.textContent = 'Account';
        accountBtn.addEventListener('click', () => {
            window.location.href = '/account'; 
        });
        headerButtonsContainer.appendChild(accountBtn);
    } else {
        const registerBtn = document.createElement('button');
        registerBtn.className = 'header-buttons';
        registerBtn.id = 'registerBtn';
        registerBtn.textContent = 'Register';
        registerBtn.addEventListener('click', async (event) => {
            event.preventDefault();
            await handleSignUp();
        });

        const loginBtn = document.createElement('button');
        loginBtn.className = 'header-buttons';
        loginBtn.id = 'loginBtn';
        loginBtn.textContent = 'Login';
        loginBtn.addEventListener('click', async (event) => {
            event.preventDefault();
            await handleSignIn();
        });

        headerButtonsContainer.appendChild(registerBtn);
        headerButtonsContainer.appendChild(loginBtn);
    }
}
