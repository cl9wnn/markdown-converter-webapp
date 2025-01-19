import {tokenStorage, showForm, createLoginForm, createRegisterForm} from "../Authorization/script.js";

const sendBtn = document.getElementById("sendBtn");
const copyBtn = document.getElementById("copyBtn");
const saveBtn = document.getElementById("saveBtn");

document.addEventListener('DOMContentLoaded', () => {
});

sendBtn.addEventListener("click",  async () => {
    await convertToHtml();
});

copyBtn.addEventListener("click", async () => {
    const htmlContent = document.getElementById("markdown-result").innerHTML;

    if (htmlContent.trim()) { 
        try {
            await navigator.clipboard.writeText(htmlContent);
        } catch (error) {
            console.error("Ошибка при копировании: ", error);
        }
    } else {
        console.error("Нет HTML-кода для копирования.");
    }
});

function generateFullHtml(content) {
    return `
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Converted Markdown</title>
</head>
<body>
    <div id="content">
        ${content}
    </div>
</body>
</html>
    `;
}

saveBtn.addEventListener("click", () => {
    const resultElement = document.getElementById("markdown-result");
    if (!resultElement) {
        alert("Элемент с результатом не найден.");
        return;
    }

    const htmlContent = resultElement.innerHTML.trim(); 

    if (htmlContent) {
        const fullHtml = generateFullHtml(htmlContent);
        const blob = new Blob([fullHtml], { type: "text/html" });

        const link = document.createElement("a");
        link.href = URL.createObjectURL(blob);
        link.download = "converted.html";
        link.style.display = "none";

        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
    } else {
        alert("Нет HTML-кода для сохранения.");
    }
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

export async function saveProject(title) {
    let token = tokenStorage.get();
    const rawMd = document.getElementById("markdown-input").value;

    try {
        const response = await fetch(`/api/documents/create`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify({
                name:title,
                mdContent: rawMd
            })
        });

        if (response.ok) {

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

