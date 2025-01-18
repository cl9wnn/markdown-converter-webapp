import {tokenStorage, showForm, createLoginForm, createRegisterForm} from "../Authorization/script.js";

const sendBtn = document.getElementById("sendBtn");

sendBtn.addEventListener("click",  async () => {
    await convertToHtml();
});

document.addEventListener('DOMContentLoaded', () => {
});

async function convertToHtml() {
    let token = tokenStorage.get();
    const rawMd = document.getElementById("markdown-input").value;
    
    try {
        const response = await fetch(`/api/convert`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify({ rawMd: rawMd }) 
        });

        if (response.ok) {
            const data = await response.json();
            document.getElementById("markdown-result").innerHTML = data.html;

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
