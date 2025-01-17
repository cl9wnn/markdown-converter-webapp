import {tokenStorage, showForm, createLoginForm, createRegisterForm} from "../Authorization/script.js";

const sendBtn = document.getElementById("sendBtn");
const signupButton = document.getElementById("registerBtn");
const signinButton = document.getElementById("loginBtn");

sendBtn.addEventListener("click",  async () => {
    await convertToHtml();
});

document.addEventListener('DOMContentLoaded', () => {
    signinButton.addEventListener('click', async (event) => {
        event.preventDefault();
        await handleSignIn();
    });

    signupButton.addEventListener('click', async (event) => {
        event.preventDefault();
        await handleSignUp();
    });
});

async function convertToHtml() {
    let token = tokenStorage.get();
    const rawMd = "Ваш markdown текст"; 

    try {
        const response = await fetch(`/api/convert`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify({ rawMd })
        });

        if (response.ok) {
            console.log('ok');
        } else if (response.status === 401) {
            const loginSuccessful = await showForm(createLoginForm, '/auth/signin', 'Sign In');

            if (loginSuccessful) {
            }
        } else {
            const data = await response.json();
            alert(data);
        }
    } catch (error) {
        alert(error);
    }
}

async function handleSignIn() {
    const success = await showForm(createLoginForm);
    if (success) {
        console.log('success');
    }
    return success;
}

async function handleSignUp() {
    const success = await showForm(createRegisterForm);
    if (success) {
        console.log('success');
    }
    return success;
}
