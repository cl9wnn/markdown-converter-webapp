import {tokenStorage, showForm, createLoginForm} from "../Authorization/script.js";
import {createModal} from "../DocumentsPage/modal.js";
import {createAccessModal} from "./accessModal.js";


const sendBtn = document.getElementById("sendBtn");
const copyBtn = document.getElementById("copyBtn");
const saveHtmlBtn = document.getElementById("saveBtn");
const documentsBtn = document.getElementById("documentsBtn");

const saveDocumentBtn = document.getElementById("saveDocumentBtn");
const renameDocumentBtn = document.getElementById("renameDocumentBtn");
const deleteDocumentBtn = document.getElementById("deleteDocumentBtn");
const accessBtn = document.getElementById("accessBtn");


let documentId;
document.addEventListener('DOMContentLoaded', async () => {
    const currentUrl = window.location.pathname;
    const segments = currentUrl.split('/');
    documentId = segments[segments.length - 1];
    
    const project = await getDocument(documentId);
    await updateProjectPage(project);
    
    await getMarkdown(documentId);
    await convertToHtml();

});

documentsBtn.addEventListener('click', () => {
    window.location.href = '/documents';
});

sendBtn.addEventListener("click",  async () => {
    await convertToHtml();
});

saveDocumentBtn.addEventListener('click', async (event) => {
    event.preventDefault();
    await saveMarkdown(documentId);
});

renameDocumentBtn.addEventListener('click', () => {
    createModal('Изменить название', async (newTitle) => {
        await renameProject(newTitle, documentId);
    });
});
    
deleteDocumentBtn.addEventListener('click', async (event) => {
    event.preventDefault();
    await deleteDocument(documentId);
});

accessBtn.addEventListener('click', async (event) => {
    createAccessModal('Дать доступ', async (inputData) => {
        await giveAccess(inputData, documentId);
    });
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

saveHtmlBtn.addEventListener("click", () => {
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
        const response = await fetch(`/api/markdown/convert`, {
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
            alert(data.error);
        }
    } catch (error) {
        alert(error);
    }
}

async function getMarkdown(documentId) {
    let token = tokenStorage.get();

    try {
        const query = new URLSearchParams({ documentId }).toString();

        const response = await fetch(`/api/markdown/get?${query}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            }
        });

        if (response.ok) {
            const data = await response.json();
            document.getElementById('markdown-input').value = data.content;
        } else if (response.status === 401) {
            const loginSuccessful = await showForm(createLoginForm, '/auth/signin', 'Sign In');

            if (loginSuccessful) {
                console.log("Logged in successfully");
            }
        } else {
            const data = await response.json();
            alert(data.error);
        }
    } catch (error) {
        alert(error);
    }
}

async function saveMarkdown(documentId) {
    let token = tokenStorage.get();
    const rawMd = document.getElementById("markdown-input").value;

    try {
        const response = await fetch(`/api/markdown/save/${documentId}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify(rawMd)
        });

        if (response.ok) {

        } else if (response.status === 401) {
            const loginSuccessful = await showForm(createLoginForm, '/auth/signin', 'Sign In');

            if (loginSuccessful) {
            }
        } else {
            const data = await response.json();
            alert(data.error);
        }
    } catch (error) {
        alert(error);
    }
}

async function getDocument(documentId) {
    let token = tokenStorage.get();

    try {
        const response = await fetch(`/api/documents/${documentId}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            }
        });

        if (response.ok) {
            const data = await response.json();
            return data.project; 
        } else if (response.status === 401) {
            const loginSuccessful = await showForm(createLoginForm, '/auth/signin', 'Sign In');

            if (loginSuccessful) {
                return await getDocument(documentId);
            }
        } else {
            const data = await response.json();
            alert(data.error.error);
            window.location.href = '/';
        }
    } catch (error) {
        alert(error);
    }
}

async function deleteDocument(documentId) {
    let token = tokenStorage.get();

    try {
        const response = await fetch(`/api/documents/${documentId}/delete`, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            }
        });

        if (response.ok) {
            window.location.href = '/documents';
        } else if (response.status === 401) {
            const loginSuccessful = await showForm(createLoginForm, '/auth/signin', 'Sign In');

            if (loginSuccessful) {
                await deleteDocument(documentId);
            }
        } else {
            const data = await response.json();
            alert(data.error);
        }
    } catch (error) {
        alert(error);
    }
}

async function renameProject(newName, documentId) {

    let token = tokenStorage.get();

    try {
        const response = await fetch(`/api/documents/${documentId}/rename`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify(newName)
        });

        if (response.ok) {
            const data = await response.json();
            
            const project = await getDocument(documentId);
            await updateProjectPage(project);      
        
        } else if (response.status === 401) {
            const loginSuccessful = await showForm(createLoginForm, '/auth/signin', 'Sign In');

            if (loginSuccessful) {
                await renameProject(newName, documentId);
            }
        } else {
            const data = await response.json();
            alert(data.error);
        }
    } catch (error) {
        alert(error);
    }
}

async function giveAccess(inputData, documentId){
    let token = tokenStorage.get();

    try {
        const response = await fetch(`/api/documentAccess/${documentId}/set-permission`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify({
                permission: inputData.role,
                email: inputData.email
            })
        });

        if (response.ok) {
        } else if (response.status === 401) {
            const loginSuccessful = await showForm(createLoginForm, '/auth/signin', 'Sign In');

            if (loginSuccessful) {
                await giveAccess(inputData, documentId);
            }
        } else {
            const data = await response.json();
            alert(data.error);
        }
    } catch (error) {
        alert(error);
    }
}

async function updateProjectPage(project){
    const projectName = document.getElementById("projectName");
    const authorName = document.getElementById("authorText");
    projectName.innerHTML = project.name;
    authorName.innerHTML = `Author: ${project.authorName}`;
}

